using ConectCar.Framework.Backend.CommonQuery.Query;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Framework.Infrastructure.Data.ServiceBus.DataProviders;
using ConectCar.Framework.Infrastructure.Log;
using ConectCar.Framework.Infrastructure.Transactions;
using GeradorPassagensPendentesParkBatch.CommandQuery.Commands;
using GeradorPassagensPendentesParkBatch.CommandQuery.Messages;
using GeradorPassagensPendentesParkBatch.CommandQuery.Queries;
using GeradorPassagensPendentesParkBatch.CommandQuery.Resources;
using GeradorPassagensPendentesParkBatch.CommandQuery.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GeradorPassagensPendentesParkBatch.CommandQuery.Handlers
{
    public class GeradorPassagemPendenteParkHandler : Loggable
    {
        #region [Properties]

        private DbConnectionDataSource _readOnlyDataSource;
        private DbConnectionDataSource _dataSource;

        private DbConnectionDataSource _readOnlyDataSourcePark;
        private DbConnectionDataSource _dataSourcePark;

        private ServiceBusDataSourceBase _serviceBusDataSource;
        private PassagemPendenteTopicCommand _passagemPendenteTopicCommand;

        #endregion [Properties]

        #region [Ctor]

        /// <summary>
        /// Cria um handler gerador de passagens pendentes de processamento.
        /// </summary>
        public GeradorPassagemPendenteParkHandler()
        {
            try
            {
                var dataProvider = new DbConnectionDataSourceProvider();

                _readOnlyDataSource = dataProvider.GetDataSource(DbConnectionDataSourceType.ConectSysReadOnly);
                _dataSource = dataProvider.GetDataSource(DbConnectionDataSourceType.ConectSys);

                _dataSourcePark = new DbConnectionDataSource("ConectParkConnStr");
                _serviceBusDataSource = new ServiceBusDataSourceBase("TransacoesServiceBus", ServiceBusUtil.FactoriesCount);

                var topicNamePadrao = ServiceBusUtil.ObterNomeTopicPassagem();
                _passagemPendenteTopicCommand = new PassagemPendenteTopicCommand(_serviceBusDataSource, true, topicNamePadrao);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }
        }

        #endregion [Ctor]

        public async Task GerarPassagensPendentesAsync()
        {
            await Task.Run(() =>
            {
                GerarPassagemPendente();
            });
        }

        private void GerarPassagemPendente()
        {
            try
            {
                var obterConfiguracaoSistemaQuery = new ObterConfiguracaoSistemaQuery(false, _readOnlyDataSource, _dataSource);
                var configuracaoSistemaTtl = obterConfiguracaoSistemaQuery.Execute("ConfiguracaoTtlEmMinutos");
                var configuracaoQtdMaximaPassagensParaProcessar = obterConfiguracaoSistemaQuery.Execute("QuantidadeMaximaPassagensParaProcessar");

                if (configuracaoSistemaTtl != null)
                {
                    if (configuracaoQtdMaximaPassagensParaProcessar != null)
                    {
                        var tempoMaximoTtlEmMinutos = int.Parse(configuracaoSistemaTtl.Valor);
                        var qtdMaximaPassagens = int.Parse(configuracaoQtdMaximaPassagensParaProcessar.Valor);

                        Log.Debug("Obtendo Detalhes Pendentes de processamento.");

                        var qryDetalhePassagemPendenteEstacionamento = new ListarDetalhePassagemPendenteEstacionamentoQuery(_dataSourcePark);
                        var detalhesPassagemPendenteEstacionamento = qryDetalhePassagemPendenteEstacionamento.Execute(new ListarDetalhePassagemPendenteEstacionamentoFilter
                        {
                            QuantidadeMaximaPassagens = qtdMaximaPassagens,
                            QuantidadeMinutosTtl = tempoMaximoTtlEmMinutos
                        }).ToList();

                        Log.Info($"Foram encontrados {detalhesPassagemPendenteEstacionamento.Count()} Detalhe disponíveis para processamento.");

                        if (detalhesPassagemPendenteEstacionamento.Any())
                        {
                            Log.Info($"Foram encontradas {detalhesPassagemPendenteEstacionamento.Count()} Detalhes pendentes para envio ao barramento.");
                            var sucessoEnvio = EnviarDetalhesPassagemPendenteEstacionamentoPendentes(detalhesPassagemPendenteEstacionamento.ToList());
                            if (sucessoEnvio)
                            {
                                var sucessoAtualizacaoPassagemPendenteEstacionamento = AtualizarTtlDetalhesPassagemPendenteEstacionamento(detalhesPassagemPendenteEstacionamento, tempoMaximoTtlEmMinutos);
                                Log.Info(sucessoAtualizacaoPassagemPendenteEstacionamento
                                    ? "Sucesso na atualização do TTL das passagens pendentes."
                                    : "Houve falha na atualização do TTL das passagens pendentes.");
                            }
                            else
                            {
                                Log.Info("Houve falha no envio das mensagens para o barramento");
                            }
                        }
                    }
                    else
                    {
                        Log.Info("Parâmetro QtdMaximaPassagensParaProcessar não definido.");
                    }
                }
                else
                {
                    Log.Info("Parâmetro ConfiguracaoTtlEmMinutos não definido.");
                }
            }
            catch (Exception e)
            {
                Log.Error(string.Format(GeradorPassagemPendenteParkResource.Error, e.Message), e);
            }
        }

        private bool EnviarDetalhesPassagemPendenteEstacionamentoPendentes(IList<PassagemPendenteParkMessage> detalhesPendentes)
        {
            try
            {
                var topicName = ServiceBusUtil.ObterNomeTopicPassagem();
                Log.Info(string.Format(GeradorPassagemPendenteParkResource.EnviandoPassagem, detalhesPendentes.Count, topicName));

                foreach (var passagemPendenteParkMessage in detalhesPendentes)
                {
                    Log.Info($"Park - RegistroTransacaoId: {passagemPendenteParkMessage.RegistroTransacaoId} - Json: {JsonConvert.SerializeObject(passagemPendenteParkMessage)}");
                }
                _passagemPendenteTopicCommand.Execute(detalhesPendentes, topicName, $"sb_{topicName}");

                Log.Info(string.Format(GeradorPassagemPendenteParkResource.SucessoEnvio, topicName, detalhesPendentes.Count));
                return true;
            }
            catch (Exception e)
            {
                Log.Error(string.Format(GeradorPassagemPendenteParkResource.Error, e.Message), e);
                return false;
            }
        }

        private bool AtualizarTtlDetalhesPassagemPendenteEstacionamento(IEnumerable<PassagemPendenteParkMessage> detalhesPassagemPark, int tempoMaximoTtlEmMinutos)
        {
            var registroTransacaoIdMin = detalhesPassagemPark.Min(x => x.RegistroTransacaoId);
            var registroTransacaoIdMax = detalhesPassagemPark.Max(x => x.RegistroTransacaoId);
            var args = new AlterarDetalheParkCommandArgs
            {
                DataTtl = DateTime.Now.AddMinutes(tempoMaximoTtlEmMinutos),
                RegistroTransacaoIdMax = registroTransacaoIdMax ?? 0,
                RegistroTransacaoIdMin = registroTransacaoIdMin ?? 0
            };

            var alterarDetalheParkCommand = new AlterarDetalheParkCommand(_dataSourcePark);
            return TransactionContextHelper.ExecuteTransaction(alterarDetalheParkCommand.Execute, args);
        }
    }
}