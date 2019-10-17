using ConectCar.Framework.Backend.CommonQuery.Query;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Framework.Infrastructure.Data.ServiceBus.DataProviders;
using ConectCar.Framework.Infrastructure.Log;
using ConectCar.Framework.Infrastructure.Transactions;
using GeradorPassagensPendentesEDIBatch.CommandQuery.Commands;
using GeradorPassagensPendentesEDIBatch.CommandQuery.Messages;
using GeradorPassagensPendentesEDIBatch.CommandQuery.Queries;
using GeradorPassagensPendentesEDIBatch.CommandQuery.Resources;
using GeradorPassagensPendentesEDIBatch.CommandQuery.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeradorPassagensPendentesEDIBatch.CommandQuery.Handlers
{
    public class GeradorPassagemPendenteEdiHandler : Loggable
    {
        #region [Properties]

        private DbConnectionDataSource _readOnlyDataSource;
        private DbConnectionDataSource _dataSource;
        private ServiceBusDataSourceBase _serviceBusDataSource;

        private PassagemPendenteTopicCommand _passagemPendenteTopicCommand;

        #endregion [Properties]

        #region [Ctor]

        /// <summary>
        /// Cria um handler gerador de passagens pendentes de processamento.
        /// </summary>
        public GeradorPassagemPendenteEdiHandler()
        {
            try
            {
                var dataProvider = new DbConnectionDataSourceProvider();

                _readOnlyDataSource = dataProvider.GetDataSource(DbConnectionDataSourceType.ConectSysReadOnly);
                _dataSource = dataProvider.GetDataSource(DbConnectionDataSourceType.ConectSys);
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

                        Log.Debug("Obtendo Detalhes TRN pendentes de processamento.");

                        var qryDetalheTrn = new ListarDetalheTrnQuery(_readOnlyDataSource);
                        var detalhesTrn = qryDetalheTrn.Execute(new ListarDetalheTrnFilter
                        {
                            QuantidadeMaximaPassagens = qtdMaximaPassagens,
                            QuantidadeMinutosTtl = tempoMaximoTtlEmMinutos
                        });

                        Log.Info($"Foram encontrados {detalhesTrn.Count()} DetalheTRN disponíveis para processamento.");

                        if (detalhesTrn.Any())
                        {
                            //Identifica quais detalhes estão repetidos
                            MarcarDetalhesRepetidos(ref detalhesTrn);

                            Log.Info($"Foram encontradas {detalhesTrn.Count()} Detalhes Trn pendentes para envio ao barramento.");
                            var sucessoEnvio = EnviarDetalhesTrnPendentes(detalhesTrn.ToList());
                            if (sucessoEnvio)
                            {
                                var sucessoAtualizacaoTtl = AtualizarTtlDetalhesTrn(detalhesTrn, tempoMaximoTtlEmMinutos);
                                Log.Info(sucessoAtualizacaoTtl
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
                Log.Error(string.Format(GeradorPassagemPendenteEDIResource.Error, e.Message), e);
            }
        }

        private static void MarcarDetalhesRepetidos(ref List<PassagemPendenteEDIMessage> detalhesTrn)
        {
            foreach (var detalhe in detalhesTrn)
                detalhe.DetalheRepetido = true;

            var detalhesTrnUnicos = detalhesTrn.GroupBy(x => new { x.NumeroPraca, x.NumeroTag, x.Data, x.StatusCobranca }).Select(x => x.FirstOrDefault());
            foreach (var item in detalhesTrnUnicos)
            {
                if (item != null) item.DetalheRepetido = false;
            }
        }

        private bool EnviarDetalhesTrnPendentes(IList<PassagemPendenteEDIMessage> detalhesPendentes)
        {
            try
            {
                var topicName = ServiceBusUtil.ObterNomeTopicPassagem();
                Log.Info(string.Format(GeradorPassagemPendenteEDIResource.EnviandoPassagem, detalhesPendentes.Count, topicName));

                foreach (var passagemPendenteEdiMessage in detalhesPendentes)
                {
                    if (passagemPendenteEdiMessage.Data != null)
                    {
                        var date = passagemPendenteEdiMessage.Data ?? DateTime.Now;
                        passagemPendenteEdiMessage.Data = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, DateTimeKind.Utc);
                    }

                    Log.Info($"Json passagem pendente TRN - {Newtonsoft.Json.JsonConvert.SerializeObject(passagemPendenteEdiMessage)}");
                }

                Log.Info($"Json completo TRN - {Newtonsoft.Json.JsonConvert.SerializeObject(detalhesPendentes)}");

                _passagemPendenteTopicCommand.Execute(detalhesPendentes, topicName, $"sb_{topicName}");

                Log.Info(string.Format(GeradorPassagemPendenteEDIResource.SucessoEnvio, topicName, detalhesPendentes.Count));
                return true;
            }
            catch (Exception e)
            {
                Log.Error(string.Format(GeradorPassagemPendenteEDIResource.Error, e.Message), e);
                return false;
            }
        }

        private bool AtualizarTtlDetalhesTrn(List<PassagemPendenteEDIMessage> detalhesTrn, int tempoMaximoTtlEmMinutos)
        {
            var detalheTrnIdMin = detalhesTrn.Min(x => x.DetalheTrnId);
            var detalheTrnIdMax = detalhesTrn.Max(x => x.DetalheTrnId);
            var args = new AlterarDetalheTrnCommandArgs
            {
                DataTtl = DateTime.Now.AddMinutes(tempoMaximoTtlEmMinutos),
                DetalheTrnIdMax = detalheTrnIdMax,
                DetalheTrnIdMin = detalheTrnIdMin
            };

            var alterarDetalheTrnCommand = new AlterarDetalheTrnCommand(_dataSource);
            return TransactionContextHelper.ExecuteTransaction(alterarDetalheTrnCommand.Execute, args);
        }
    }
}