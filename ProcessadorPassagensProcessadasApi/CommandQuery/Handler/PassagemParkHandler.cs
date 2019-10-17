using AutoMapper;
using ConectCar.Framework.Infrastructure.Cqrs.Commands;
using ConectCar.Framework.Infrastructure.Cqrs.Handlers;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Framework.Infrastructure.Transactions;
using ConectCar.Transacoes.Domain.Dto;
using Newtonsoft.Json;
using ProcessadorPassagensProcessadasApi.CommandQuery.Commands;
using ProcessadorPassagensProcessadasApi.CommandQuery.Commands.Args;
using ProcessadorPassagensProcessadasApi.CommandQuery.Dtos;
using ProcessadorPassagensProcessadasApi.CommandQuery.Extension;
using ProcessadorPassagensProcessadasApi.CommandQuery.Response;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ConectCar.Transacoes.Domain.Model;
using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Handler
{
    public class PassagemParkHandler : DataSourceHandlerBase, IAdoDataSourceProvider,
        ICommand<List<PassagemAprovadaEstacionamentoDto>, PassagensAprovadasParkResponse>,
        ICommand<List<PassagemReprovadaEstacionamentoDto>, PassagensReprovadasParkResponse>
    {
        public PassagemParkHandler()
        {
        }

        public DbConnectionDataSourceProvider AdoDataSourceProvider => GetAdoProvider();

        protected override void Init()
        {
            AddProvider(new DbConnectionDataSourceProvider());
        }

        #region [Passagem Aprovadas]

        /// <summary>
        /// Recebe coleção de passagens aprovadas para processamento no ConectSys.
        /// </summary>
        /// <param name="passagensAprovadas">PassagemAprovadaEstacionamentoDto[]</param>
        /// <returns>PassagensAprovadasParkResponse</returns>
        public PassagensAprovadasParkResponse Execute(List<PassagemAprovadaEstacionamentoDto> passagensAprovadasPark)
        {
            var response = new PassagensAprovadasParkResponse();
            var execucaoId = Guid.NewGuid();
            response.ExecucaoId = execucaoId;

            Log.Info($"ID: {execucaoId} - Início - Execução Passagens Aprovadas Park.");

            if (passagensAprovadasPark != null && passagensAprovadasPark.Any())
            {
                SalvarAprovadasStagingConectSys(passagensAprovadasPark, response, execucaoId);
            }

            var jsonResponse = JsonConvert.SerializeObject(response);
            Log.Info($"ID: {execucaoId} - Fim - Execução Passagens Aprovadas Park. Retorno: {jsonResponse}.");

            return response;
        }

        private void SalvarAprovadasStagingConectSys(List<PassagemAprovadaEstacionamentoDto> passagensAprovadas, PassagensAprovadasParkResponse response, Guid execucaoId)
        {
            try
            {
                var transacoes = passagensAprovadas?.Select(c => c.TransacaoEstacionamento).ToList();
                var transacaoStaging = Mapper.Map<List<TransacaoPassagemEstacionamentoLoteStaging>>(transacoes);
                var idsTransacoes = transacoes.Select(x => x.RegistroTransacaoId).ToList();

                var extratos = passagensAprovadas?.Select(c => c.Extrato).ToList();
                var extratosStaging = Mapper.Map<List<ExtratoLoteStaging>>(extratos);

                var conveniadoInformacoes = passagensAprovadas?.Select(c => c.ConveniadoInformacoesRPS).ToList();
                var conveniadoInformacoesStaging = Mapper.Map<List<ConveniadoInformacoesRpsLoteStaging>>(conveniadoInformacoes);

                var detalhePassagens = passagensAprovadas?.SelectMany(c => c.TransacaoEstacionamento.Detalhes).ToList();
                var detalhePassagemStaging = new List<DetalhePassagemEstacionamentoLoteStaging>();
                detalhePassagens.ForEach(x => detalhePassagemStaging.Add(Mapper.Map<DetalhePassagemEstacionamentoLoteStaging>(x)));

                var pistaInformacoes = passagensAprovadas?.Select(c => c.PistaInformacoesRPS).ToList();
                var pistaInformacoesRpsStaging = Mapper.Map<List<PistaInformacoesRPSLoteStaging>>(pistaInformacoes);

                TransactionContextHelper.ExecuteTransaction((aprovadas) =>
                {
                    Log.Debug($"ID: {execucaoId} - Início - Staging Passagens Aprovadas Park - ConectSys.");
                    using (var datasourceConectSys = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSys))
                    {
                        try
                        {
                            datasourceConectSys.Connection.BulkInsert(transacaoStaging, "TransacaoPassagemEstacionamentoStaging");
                            datasourceConectSys.Connection.BulkInsert(extratosStaging, "ExtratoLoteStaging");
                            datasourceConectSys.Connection.BulkInsert(detalhePassagemStaging, "DetalhePassagemEstacionamentoStaging");
                            datasourceConectSys.Connection.BulkInsert(conveniadoInformacoesStaging, "ConveniadoInformacoesRPSStaging");
                            datasourceConectSys.Connection.BulkInsert(pistaInformacoesRpsStaging, "PistaInformacoesRpsStaging");

                            response.QtdConveniadoInformacoesRPSStaging = conveniadoInformacoesStaging.Count;
                            response.QtdDetalhePassagemEstacionamentoStaging = detalhePassagemStaging.Count;
                            response.QtdPistaInformacoesRPSStaging = pistaInformacoesRpsStaging.Count;
                            response.QtdTransacaoPassagemEstacionamentoStaging = transacaoStaging.Count;
                        }
                        catch (Exception ex)
                        {
                            Log.Fatal($"ID: {execucaoId} - Erro Fatal - Staging Passagem Aprovadas Park - ConectSys", ex);
                            response.SucessoStagingConectSys = false;
                        }
                    }
                    Log.Debug($"ID: {execucaoId} - Fim - Staging Passagens Aprovadas Park - ConectSys");
                }, passagensAprovadas);

                TransactionContextHelper.ExecuteTransaction((aprovadas) =>
                {
                    using (var dataSourceConectPark = new DbConnectionDataSource("ConectParkConnStr"))
                    {
                        var command = new AtualizarRegistroTransacaoProcessadoCommand(dataSourceConectPark);
                        command.Execute(new AtualizarRegistroTransacaoProcessadoArgs
                        {
                            RegistroTransacaoIds = idsTransacoes
                        });
                    }
                }, passagensAprovadas);
            }
            catch (Exception ex)
            {
                Log.Fatal($"ID: {execucaoId} - Erro Fatal. Erro: {ex.Message}", ex);
            }
        }

        #endregion [Passagem Aprovadas]

        #region [Passagem Reprovadas]

        public PassagensReprovadasParkResponse Execute(List<PassagemReprovadaEstacionamentoDto> passagensReprovadas)
        {
            var response = new PassagensReprovadasParkResponse();

            var execucaoId = Guid.NewGuid();
            response.ExecucaoId = execucaoId;

            Log.Info($"ID: {execucaoId} - Início - Execução Passagens Reprovadas Park.");

            if (passagensReprovadas != null && passagensReprovadas.Any())
            {
                SalvarReprovadasStagingConectSys(passagensReprovadas, response, execucaoId);
            }

            var jsonResponse = JsonConvert.SerializeObject(response);
            Log.Info($"ID: {execucaoId} - Fim - Execução Passagens Reprovadas Park. Retorno: {jsonResponse}.");

            return response;
        }

        private void SalvarReprovadasStagingConectSys(List<PassagemReprovadaEstacionamentoDto> passagensReprovadas, PassagensReprovadasParkResponse response, Guid execucaoId)
        {
            var sw = new Stopwatch();
            sw.Start();

            var transacoesRecusadas = passagensReprovadas?.Select(c => c.TransacaoEstacionamentoRecusada).ToList();
            var transacoesEstacionamentoRecusadasLoteStaging = Mapper.Map<List<TransacaoEstacionamentoRecusadaLoteStaging>>(transacoesRecusadas);
            var idsTransacoes = transacoesRecusadas.Select(x => x.RegistroTransacaoId).ToList();

            var detalhePassagens = passagensReprovadas?.SelectMany(c => c.TransacaoEstacionamentoRecusada.Detalhes).ToList();
            var detalhePassagemEstacionamentoRecusadasStaging = new List<DetalheTransacaoEstacionamentoRecusadaLoteStaging>();
            detalhePassagens.ForEach(x => detalhePassagemEstacionamentoRecusadasStaging.Add(Mapper.Map<DetalheTransacaoEstacionamentoRecusadaLoteStaging>(x)));

            try
            {
                TransactionContextHelper.ExecuteTransaction((reprovadas) =>
                {
                    Log.Debug($"ID: {execucaoId} - Início - Staging Passagens Reprovadas Park - ConectSys.");
                    using (var datasourceConectSys = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSys))
                    {
                        try
                        {
                            datasourceConectSys.Connection.BulkInsertTransacoes(transacoesEstacionamentoRecusadasLoteStaging,  "TransacaoEstacionamentoRecusadaLoteStaging");
                            datasourceConectSys.Connection.BulkInsertTransacoes(detalhePassagemEstacionamentoRecusadasStaging, "DetalhePassagemEstacionamentoRecusadaLoteStaging");

                            response.QtdTransacaoEstacionamentoRecusadaStaging = transacoesEstacionamentoRecusadasLoteStaging.Count;
                            response.QtdDetalhePassagemEstacionamentoRecusadaStaging = detalhePassagemEstacionamentoRecusadasStaging.Count;
                        }
                        catch (Exception ex)
                        {
                            Log.Fatal($"ID: {execucaoId} - Erro Fatal - Staging Passagens Reprovadas Park - ConectSys.", ex);
                            response.SucessoStagingConectSys = false;
                        }
                    }
                    Log.Debug($"ID: {execucaoId} - Fim - Staging Passagens Reprovadas Park - ConectSys.");
                }, passagensReprovadas);

                TransactionContextHelper.ExecuteTransaction((reprovadas) =>
                {
                    using (var dataSourceConectPark = new DbConnectionDataSource("ConectParkConnStr"))
                    {
                        var command = new AtualizarRegistroTransacaoProcessadoCommand(dataSourceConectPark);
                        command.Execute(new AtualizarRegistroTransacaoProcessadoArgs
                        {
                            RegistroTransacaoIds = idsTransacoes
                        });
                    }
                }, transacoesRecusadas);
            }
            catch (Exception ex)
            {
                Log.Fatal($"ID: {execucaoId} - Erro Fatal. Erro: {ex.Message}", ex);
            }

            sw.Stop();
            response.TempoExecucaoStagingConectSys = sw.Elapsed;
        }

        #endregion [Passagem Reprovadas]
    }
}