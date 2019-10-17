using ConectCar.Cadastros.Conveniados.Backend.CommonQuery.Query;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Framework.Infrastructure.Data.Rest.DataProviders;
using ConectCar.Framework.Infrastructure.Data.ServiceBus.DataProviders;
using ConectCar.Framework.Infrastructure.Log;
using LeitorPassagensPendentesBatch.CommandQuery.Commands;
using LeitorPassagensPendentesBatch.CommandQuery.Handlers.Request;
using LeitorPassagensPendentesBatch.CommandQuery.Messages;
using LeitorPassagensPendentesBatch.CommandQuery.Queries;
using LeitorPassagensPendentesBatch.CommandQuery.Resources;
using LeitorPassagensPendentesBatch.CommandQuery.Util;
using System;
using System.Collections.Async;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeitorPassagensPendentesBatch.CommandQuery.Handlers
{
    public class LeitorPassagemPendenteHandler : Loggable
    {
        #region [Properties]

        private DbConnectionDataSource _readOnlyDataSource;
        private DbConnectionDataSource _dataSource;
        private ServiceBusDataSourceBase _serviceBusDataSource;
        private RestDataSource _restDataSource;

        private ObterConcessionariasQuery _obterConcessionariasQuery;
        private ObterPassagensTopicQuery _obterPassagensQuery;
        private EnviarPassagemParaAkkaCommand _enviarPassagemParaAkkaCommand;

        #endregion

        #region [Ctor]

        public LeitorPassagemPendenteHandler(DbConnectionDataSource readOnlyDataSource,
            DbConnectionDataSource dataSource,
            ServiceBusDataSourceBase serviceBusDataSource,
            RestDataSource restDataSource
            )
        {
            _readOnlyDataSource = readOnlyDataSource;
            _dataSource = dataSource;
            _serviceBusDataSource = serviceBusDataSource;
            _restDataSource = restDataSource;

            var nomeTopicPadrao = ServiceBusUtil.ObterNome();

            _obterConcessionariasQuery = new ObterConcessionariasQuery(true, readOnlyDataSource, _dataSource);
            _obterPassagensQuery = new ObterPassagensTopicQuery(_serviceBusDataSource, false, ServiceBusUtil.BatchSize, nomeTopicPadrao);
            _enviarPassagemParaAkkaCommand = new EnviarPassagemParaAkkaCommand(_restDataSource);
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Responsável por executar a leitura de passagens e envio para processamento.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task Executar()
        {
            await Task.Run(async () =>
            {

                await EviarPassagensPendentesArtespParaProcessamento();

            });
        }

        /// <summary>
        /// Envia as mensagens de uma concessinária para processamento.
        /// </summary>
        /// <returns>Task</returns>
        private async Task EviarPassagensPendentesArtespParaProcessamento()
        {
            await Task.Run(async () =>
            {

                try
                {
                    var nomeTopic = ServiceBusUtil.ObterNome();
                    var subscriptionName = $"sb_{nomeTopic}";
                    
                    var qtdExecucoes = 1;

                    long qtdMensagensBarramento = ObterQuantidadeMensagensPendentesBarramento(nomeTopic, subscriptionName);

                    while (qtdMensagensBarramento > 0)
                    {                        
                        Log.Debug(String.Format(LeitorPassagensPendentesBatchResource.ObterPassagensPendentesBarramento, nomeTopic, qtdExecucoes));
                        var topicReceiveMessage = _obterPassagensQuery.ExecuteToConfirm((x) => x.MensagemItemId.ToString(), qtdMensagensBarramento);
                        if (topicReceiveMessage.Messages.Any())
                        {
                            var passagensPendentes = topicReceiveMessage.Messages;
                            var locks = topicReceiveMessage.Tokens;

                            Log.Info(string.Format(LeitorPassagensPendentesBatchResource.QtdPassagensPendentesBarramento, passagensPendentes.Count, nomeTopic));

                            var passagensPorCodigoProtocolo = passagensPendentes.GroupBy(x => x.ConcessionariaId);

                            await passagensPorCodigoProtocolo.ParallelForEachAsync(async passagens =>
                            {

                                var passagensEnviadas = await EnviarPassagensPendentesArtespParaProcessamentoNaApi(passagens.ToList(), passagens.Key);
                                if (passagensEnviadas)
                                {
                                    // confirma as passagens enviadas
                                    var mensagensItensIdEnviadas = passagens.Select(x => x.MensagemItemId.ToString());
                                    var locksConcessionaria = locks.Where(x => mensagensItensIdEnviadas.Contains(x.Key)).Select(x => x.Value);
                                    _obterPassagensQuery.ConfirmMessages(locksConcessionaria.ToList());
                                }


                            }, maxDegreeOfParalellism: 10);
                        }
                        else
                        {
                            break;
                        }
                        
                        qtdMensagensBarramento = ObterQuantidadeMensagensPendentesBarramento(nomeTopic, subscriptionName);
                    }



                }
                catch (Exception e)
                {
                    Log.Error(string.Format(LeitorPassagensPendentesBatchResource.Error, e.Message), e);
                }
            });
        }

        private long ObterQuantidadeMensagensPendentesBarramento(string nomeTopic, string subscriptionName)
        {
            return _serviceBusDataSource.GetActiveMessageCount(nomeTopic, subscriptionName);
        }

        private async Task<bool> EnviarPassagensPendentesArtespParaProcessamentoNaApi(List<PassagemPendenteMessage> passagensPendentes, int codigoProtocoloArtesp)
        {
            return await Task.Run(() =>
            {

                var passagensEnviadasComSucesso = false;

                try
                {
                    Log.Info(String.Format(LeitorPassagensPendentesBatchResource.InicioEnvioParaApiPRocessamento, passagensPendentes.Count, codigoProtocoloArtesp));
                    var request = new EnviarPassagensRequest(passagensPendentes, codigoProtocoloArtesp);
                    passagensEnviadasComSucesso = _enviarPassagemParaAkkaCommand.Execute(request);

                    if (passagensEnviadasComSucesso)
                        Log.Info(String.Format(LeitorPassagensPendentesBatchResource.FimEnvioParaApiPRocessamento, passagensPendentes.Count, codigoProtocoloArtesp));
                }
                catch (Exception ex)
                {
                    Log.Error(string.Format(LeitorPassagensPendentesBatchResource.Error, ex.Message), ex);
                }

                return passagensEnviadasComSucesso;

            });
        }

        #endregion


    }
}
