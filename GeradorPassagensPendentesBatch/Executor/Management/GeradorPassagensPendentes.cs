using System;
using System.Threading.Tasks;
using ConectCar.Framework.Infrastructure.Log;
using GeradorPassagensPendentesBatch.CommandQuery.Handlers;
using GeradorPassagensPendentesBatch.CommandQuery.Resources;
using GeradorPassagensPendentesBatch.Management.Interfaces;
using ConectCar.Cadastros.Conveniados.Backend.CommonQuery.Query;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Framework.Infrastructure.Data.ServiceBus.DataProviders;
using GeradorPassagensPendentesBatch.CommandQuery.Util;
using GeradorPassagensPendentesBatch.CommandQuery.Commands;
using ConectCar.Framework.Backend.CommonQuery.Query;

namespace GeradorPassagensPendentesBatch.Management
{
    /// <summary>
    /// Gerador de passagens pendentes de processamento.
    /// </summary>
    public class GeradorPassagensPendentes : Loggable, IGeradorPassagensPendentes
    {
        #region [Properties]

        private DbConnectionDataSource _readOnlyDataSource;
        private DbConnectionDataSource _dataSource;
        private DbConnectionDataSource _dataSourceMensageria;
        private ServiceBusDataSourceBase _serviceBusDataSource;
        private ObterConcessionariasQuery _obterConcessionariasQuery;
        private PassagemPendenteTopicCommand _passagemPendenteTopicCommand;

        #endregion

        #region [Ctor]

        /// <summary>
        /// Inicializa o gerador de passagens pendentes.
        /// </summary>
        public GeradorPassagensPendentes()
        {
            Log.Debug(GeradorPassagemPendenteResource.GetDataSource);

            var dataProvider = new DbConnectionDataSourceProvider();

            _readOnlyDataSource = dataProvider.GetDataSource(DbConnectionDataSourceType.ConectSysReadOnly);
            _dataSource = dataProvider.GetDataSource(DbConnectionDataSourceType.ConectSys);
            _dataSourceMensageria = dataProvider.GetDataSource(DbConnectionDataSourceType.Mensageria);
            _serviceBusDataSource = new ServiceBusDataSourceBase("TransacoesServiceBus", ServiceBusUtil.FactoriesCount, ServiceBusUtil.BatchFlushInterval, ServiceBusUtil.LockDuration);

            var topicNamePadrao = ServiceBusUtil.ObterNomeQueuePassagem();
            _passagemPendenteTopicCommand = new PassagemPendenteTopicCommand(_serviceBusDataSource, true, topicNamePadrao);
            _obterConcessionariasQuery = new ObterConcessionariasQuery(true, _readOnlyDataSource, _dataSource);
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Executa geração de passagens pendentes de forma assincrona.
        /// </summary>
        /// <returns>Task</returns>
        public async Task ExecuteAsync()
        {
            try
            {

                await Task.Run(async () =>
                {
                    try
                    {
                        var obterConfiguracaoSistemaQuery = new ObterConfiguracaoSistemaQuery(true, _readOnlyDataSource, _dataSource);
                        int tempoMaximoTtlEmMinutos = ObterTempoMaximoTtlEmMinutos(obterConfiguracaoSistemaQuery);
                        int qtdMaximaPassagensParaProcessar = ObterQtdMaximaPassagensParaProcessaar(obterConfiguracaoSistemaQuery);
                        var concessionarias = _obterConcessionariasQuery.Execute();

                        var handler = new GeradorPassagensPendentesHandler(_readOnlyDataSource
                            , _dataSource
                            , _dataSourceMensageria
                            , _serviceBusDataSource
                            , _obterConcessionariasQuery
                            , _passagemPendenteTopicCommand);


                        var count = 0;
                        var iteracoes = 100;
                        while (count < iteracoes)
                        {
                            count++;
                            Log.Debug($"{GeradorPassagemPendenteResource.InicioProcesso} - Execução {count}.");
                            await handler.GerarPassagensPendentesAsync(tempoMaximoTtlEmMinutos, qtdMaximaPassagensParaProcessar, concessionarias);
                            Log.Debug($"{GeradorPassagemPendenteResource.FinalProcesso} - Execução {count}.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(string.Format(GeradorPassagemPendenteResource.Error, ex.Message), ex);
                    }
                });


            }
            catch (Exception e)
            {
                Log.Error(string.Format(GeradorPassagemPendenteResource.Error, e.Message), e);
            }
        }

        /// <summary>
        /// Obtem quantidade máxima de passagens para processar.
        /// </summary>
        /// <param name="obterConfiguracaoSistemaQuery">ObterConfiguracaoSistemaQuery.</param>
        /// <returns>Quantidade máxima de passagens para processar.</returns>
        private int ObterQtdMaximaPassagensParaProcessaar(ObterConfiguracaoSistemaQuery obterConfiguracaoSistemaQuery)
        {
            Log.Debug(String.Format(GeradorPassagemPendenteResource.ObterConfiguracaoSistema, "QuantidadeMaximaPassagensParaProcessar"));
            var configuracaoQtdMaximaPassagensParaProcessar = obterConfiguracaoSistemaQuery.Execute("QuantidadeMaximaPassagensParaProcessar");
            if (configuracaoQtdMaximaPassagensParaProcessar == null)
            {
                throw new ArgumentNullException("configuracaoQtdMaximaPassagensParaProcessar", String.Format(GeradorPassagemPendenteResource.NaoExisteConfiguracaoSistema, "QuantidadeMaximaPassagensParaProcessar"));
            }

            var qtdMaximaPassagensParaProcessar = 0;
            if (!int.TryParse(configuracaoQtdMaximaPassagensParaProcessar.Valor, out qtdMaximaPassagensParaProcessar))
            {
                throw new ArgumentException(String.Format(GeradorPassagemPendenteResource.ConfiguracaoSistemaInvalida, "QuantidadeMaximaPassagensParaProcessar", configuracaoQtdMaximaPassagensParaProcessar.Valor), "qtdMaximaPassagensParaProcessar");
            }

            return qtdMaximaPassagensParaProcessar;
        }

        /// <summary>
        /// Obtem o tempo máximo do Ttl em minutos.
        /// </summary>
        /// <param name="query">ObterConfiguracaoSistemaQuery</param>
        /// <returns>Tempo máximo do Ttl em minutos.</returns>
        private int ObterTempoMaximoTtlEmMinutos(ObterConfiguracaoSistemaQuery query)
        {
            Log.Debug(String.Format(GeradorPassagemPendenteResource.ObterConfiguracaoSistema, "ConfiguracaoItemPendenteProcessamentoTtlEmMinutos"));
            var configuracaoSistemaTtl = query.Execute("ConfiguracaoItemPendenteProcessamentoTtlEmMinutos");
            if (configuracaoSistemaTtl == null)
            {
                throw new ArgumentNullException("configuracaoSistemaTtl", String.Format(GeradorPassagemPendenteResource.NaoExisteConfiguracaoSistema, "ConfiguracaoItemPendenteProcessamentoTtlEmMinutos"));
            }

            var tempoMaximoTtlEmMinutos = 0;
            if (!int.TryParse(configuracaoSistemaTtl.Valor, out tempoMaximoTtlEmMinutos))
            {
                throw new ArgumentException(String.Format(GeradorPassagemPendenteResource.ConfiguracaoSistemaInvalida, "ConfiguracaoItemPendenteProcessamentoTtlEmMinutos", configuracaoSistemaTtl.Valor)
                    , "tempoMaximoTtlEmMinutos");
            }

            return tempoMaximoTtlEmMinutos;
        }

        #endregion

    }
}
