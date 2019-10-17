using AutoMapper;
using ConectCar.Framework.Infrastructure.Cqrs.Commands;
using ConectCar.Framework.Infrastructure.Cqrs.Handlers;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Framework.Infrastructure.Transactions;
using ConectCar.Transacoes.Domain.Dto;
using Newtonsoft.Json;
using ProcessadorPassagensProcessadasApi.CommandQuery.Dtos;
using ProcessadorPassagensProcessadasApi.CommandQuery.Extension;
using ProcessadorPassagensProcessadasApi.CommandQuery.Response;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Handler
{
    public class PassagemEdiHandler : DataSourceHandlerBase, IAdoDataSourceProvider,
        ICommand<List<PassagemAprovadaEDIDto>, PassagensAprovadasEdiResponse>,
        ICommand<List<PassagemReprovadaEdiDto>, PassagensReprovadasEdiResponse>
    {
        public DbConnectionDataSourceProvider AdoDataSourceProvider => GetAdoProvider();

        protected override void Init()
        {
            AddProvider(new DbConnectionDataSourceProvider());
        }


        #region [PASSAGENS APROVADAS]

        /// <summary>
        /// Recebe coleção de passagens aprovadas para processamento no ConectSys.
        /// </summary>
        /// <param name="passagensAprovadasEdi"></param>
        /// <returns>PassagensAprovadasEdiResponse</returns>
        public PassagensAprovadasEdiResponse Execute(List<PassagemAprovadaEDIDto> passagensAprovadasEdi)
        {
            var execucaoId = Guid.NewGuid();
            var response = new PassagensAprovadasEdiResponse { ExecucaoId = execucaoId };

            Log.Info($"ID: {execucaoId} - Início - Execução Passagens Aprovadas Edi.");

            if (passagensAprovadasEdi != null && passagensAprovadasEdi.Any())
            {
                SalvarAprovadasStagingConectSys(passagensAprovadasEdi, response, execucaoId);
            }

            var jsonResponse = JsonConvert.SerializeObject(response);
            Log.Info($"ID: {execucaoId} - Fim - Execução Passagens Aprovadas Edi. Retorno: {jsonResponse}.");

            return response;

        }
        private void SalvarAprovadasStagingConectSys(List<PassagemAprovadaEDIDto> passagensAprovadas, PassagensAprovadasEdiResponse response, Guid execucaoId)
        {
            var sw = new Stopwatch();
            sw.Start();

            try
            {
                TransactionContextHelper.ExecuteTransaction((aprovadas) =>
                {
                    Log.Debug($"ID: {execucaoId} Início - Staging Passagens Aprovadas Edi - Conectsys.");
                    using (var datasourceConectSys = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSys))
                    {
                        try
                        {
                            var transacoes = passagensAprovadas.Any(c => c.TransacaoPassagemEDI != null) ? passagensAprovadas.Where(c => c.TransacaoPassagemEDI != null).Select(c => c.TransacaoPassagemEDI) : null;
                            var transacoesStaging = Mapper.Map<List<TransacaoPassagemLoteStaging>>((transacoes ?? new List<TransacaoPassagemEDIDto>()));
                            var transacoesProvisorias = passagensAprovadas.Any(c => c.TransacaoProvisoriaEDI != null) ? passagensAprovadas.Where(c => c.TransacaoProvisoriaEDI != null).Select(c => c.TransacaoProvisoriaEDI) : null;
                            var transacaosProvisoriaStaging = Mapper.Map<IEnumerable<TransacaoProvisoriaEDIDto>, IEnumerable<TransacaoPassagemLoteStaging>>(transacoesProvisorias);
                            if (transacaosProvisoriaStaging != null)
                                transacoesStaging.AddRange(transacaosProvisoriaStaging);

                            transacoesStaging.ForEach(x => x.ExecucaoId = execucaoId);

                            datasourceConectSys.Connection.BulkInsertTransacoes(transacoesStaging, "TransacaoPassagemEdiStaging");

                            var detalheTrfRecusados = passagensAprovadas.Any(c => c.DetalheTRFRecusaEvasao != null) ? passagensAprovadas.Select(c => c.DetalheTRFRecusaEvasao) : null;
                            var detalheTrfRecusadosStaging = Mapper.Map<List<DetalheTRFRecusadoLoteStaging>>((detalheTrfRecusados ?? new List<DetalheTRFRecusadoDto>()));
                            detalheTrfRecusadosStaging.ForEach(x => x.ExecucaoId = execucaoId);
                            datasourceConectSys.Connection.BulkInsert(detalheTrfRecusadosStaging, "DetalheTRFRecusadoLoteStaging");

                            var detalheTrfAprovadosManualmente = passagensAprovadas.Any(c => c.DetalheTRFAprovadoManualmente != null) ? passagensAprovadas.Select(c => c.DetalheTRFAprovadoManualmente) : null;
                            var detalheTrfAprovadosManualmenteStaging = Mapper.Map<List<DetalheTRFAprovadaManualmenteLoteStaging>>((detalheTrfAprovadosManualmente ?? new List<DetalheTRFAprovadoManualmenteDto>()));
                            detalheTrfAprovadosManualmenteStaging.ForEach(x => x.ExecucaoId = execucaoId);
                            datasourceConectSys.Connection.BulkInsert(detalheTrfAprovadosManualmenteStaging, "DetalheTRFAprovadaManualmenteLoteStaging");

                            var extratos = passagensAprovadas.Any(c => c.Extrato != null) ? passagensAprovadas.Select(c => c.Extrato) : null;
                            var extratosStaging = Mapper.Map<List<ExtratoLoteStaging>>((extratos ?? new List<ExtratoDto>()));
                            extratosStaging.ForEach(x => x.ExecucaoId = execucaoId);
                            datasourceConectSys.Connection.BulkInsert(extratosStaging, "ExtratoLoteStaging");

                            var eventos = passagensAprovadas.Any(c => c.EventoPrimeiraPassagemManual != null) ? passagensAprovadas.Select(c => c.EventoPrimeiraPassagemManual) : null;
                            var eventoStaging = Mapper.Map<List<EventoLoteStaging>>((eventos ?? new List<EventoDto>()));
                            eventoStaging.ForEach(x => x.ExecucaoId = execucaoId);
                            datasourceConectSys.Connection.BulkInsert(eventoStaging, "EventoLoteStaging");

                            var configuracoesAdesao = passagensAprovadas.Any(c => c.ConfiguracaoAdesao != null) ? passagensAprovadas.Select(c => c.ConfiguracaoAdesao) : null;
                            var configuracoesAdesaoStaging = Mapper.Map<List<ConfiguracaoAdesaoLoteStaging>>((configuracoesAdesao ?? new List<ConfiguracaoAdesaoDto>()));
                            configuracoesAdesaoStaging.ForEach(x => x.ExecucaoId = execucaoId);
                            datasourceConectSys.Connection.BulkInsert(configuracoesAdesaoStaging, "ConfiguracaoAdesaoLoteStaging");

                            var divergenciasCategoriaConfirmada = passagensAprovadas.Any(c => c.DivergenciaCategoriaConfirmada != null) ? passagensAprovadas.Select(c => c.DivergenciaCategoriaConfirmada) : null;
                            var divergenciasCategoriaConfirmadaStaging = Mapper.Map<List<DivergenciaCategoriaConfirmadaLoteStaging>>((divergenciasCategoriaConfirmada ?? new List<DivergenciaCategoriaConfirmadaDto>()));
                            divergenciasCategoriaConfirmadaStaging.ForEach(x => x.ExecucaoId = execucaoId);
                            datasourceConectSys.Connection.BulkInsert(divergenciasCategoriaConfirmadaStaging, "DivergenciaCategoriaConfirmadaLoteStaging");

                            var veiculos = passagensAprovadas.Any(c => c.Veiculo != null) ? passagensAprovadas.Select(c => c.Veiculo) : null;
                            var veiculosStaging = Mapper.Map<List<VeiculoLoteStaging>>((veiculos ?? new List<VeiculoDto>()));
                            veiculosStaging.ForEach(x => x.ExecucaoId = execucaoId);
                            datasourceConectSys.Connection.BulkInsert(veiculosStaging, "VeiculoLoteStaging");

                            var detalhesViagem = passagensAprovadas.Any(c => c.Viagens != null) ? passagensAprovadas.SelectMany(c => c.Viagens) : null;
                            var detalhesViagemStaging = Mapper.Map<List<DetalheViagemLoteStaging>>((detalhesViagem ?? new List<DetalheViagemDto>()));
                            detalhesViagemStaging.ForEach(x => x.ExecucaoId = execucaoId);
                            datasourceConectSys.Connection.BulkInsert(detalhesViagemStaging, "DetalheViagemLoteStaging");

                            response.QtdTransacaoPassagemEDIStaging = transacoesStaging.Count;
                            response.QtdDetalheTRFRecusaEvasaoStaging = detalheTrfRecusadosStaging.Count;
                            response.QtdTransacaoProvisoriaEDIStaging = transacoesStaging.Count(x => x.TransacaoProvisoria);
                            response.QtdDetalheTRFAprovadoManualmenteStaging = detalheTrfAprovadosManualmenteStaging.Count;
                            response.QtdExtratoStaging = extratosStaging.Count;
                            response.QtdEventoStaging = eventoStaging.Count;
                            response.QtdConfiguracaoAdesaoStaging = configuracoesAdesaoStaging.Count;
                            response.QtdDivergenciaCategoriaConfirmadaStaging = divergenciasCategoriaConfirmadaStaging.Count;
                            response.QtdVeiculoStaging = veiculosStaging.Count;
                            response.QtdDetalheViagemStaging = detalhesViagemStaging.Count;

                        }
                        catch (Exception ex)
                        {
                            Log.Fatal($"ID: {execucaoId} - Erro Fatal - Staging Passagens Aprovadas Edi - ConectSys.", ex);
                            response.SucessoStagingConectSys = false;
                        }
                    }
                    Log.Debug($"ID: {execucaoId} - Fim - Staging Passagens Aprovadas Edi - ConectSys.");

                }, passagensAprovadas);
            }
            catch (Exception ex)
            {
                Log.Fatal($"ID: {execucaoId} - Erro Fatal. Erro: {ex.Message}", ex);
            }

            sw.Stop();
            response.TempoExecucaoStagingConectSys = sw.Elapsed;

            Log.Debug($"Json Response BulkInsert {JsonConvert.SerializeObject(response)}");

        }

        #endregion

        #region [PASSAGENS REPROVADAS]

        /// <summary>
        /// Recebe coleção de passagens reprovadas para processamento no ConectSys.
        /// </summary>
        /// <param name="PassagensReprovadas">PassagemReprovadaEdiDto[]</param>
        /// <returns>PassagensReprovadasEdiResponse</returns>
        public PassagensReprovadasEdiResponse Execute(List<PassagemReprovadaEdiDto> PassagensReprovadas)
        {

            var response = new PassagensReprovadasEdiResponse();

            var execucaoId = Guid.NewGuid();
            response.ExecucaoId = execucaoId;

            Log.Info($"ID: {execucaoId} - Início - Execução Passagens Reprovadas Edi.");


            if (PassagensReprovadas != null && PassagensReprovadas.Any())
            {
                SalvarReprovadasStagingConectSys(PassagensReprovadas, response, execucaoId);
            }

            var jsonResponse = JsonConvert.SerializeObject(response);
            Log.Info($"ID: {execucaoId} - Fim - Execução Passagens Reprovadas Edi. Retorno: {jsonResponse}.");

            return response;
        }


        /// <summary>
        /// Salva as passagens reprovadas nas tabelas de Staging do ConectSys.
        /// </summary>
        /// <param name="passagensReprovadas">PassagemReprovadaEdiDto</param>
        /// <param name="response">PassagensReprovadasEdiResponse</param>
        /// <param name="execucaoId">Sessão de execução.</param>
        private void SalvarReprovadasStagingConectSys(List<PassagemReprovadaEdiDto> passagensReprovadas, PassagensReprovadasEdiResponse response, Guid execucaoId)
        {
            var sw = new Stopwatch();
            sw.Start();

            try
            {
                TransactionContextHelper.ExecuteTransaction((reprovadas) =>
                {
                    Log.Debug($"ID: {execucaoId} - Início - Staging Passagens Reprovadas Edi - ConectSys.");
                    using (var datasourceConectSys = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSys))
                    {
                        try
                        {
                            var detalheTrfRecusados = passagensReprovadas.Any(c => c.DetalheTRFRecusado != null) ? passagensReprovadas.Select(c => c.DetalheTRFRecusado) : null;
                            var detalheTRFRecusadosStaging = Mapper.Map<List<DetalheTRFRecusadoLoteStaging>>((detalheTrfRecusados ?? new List<DetalheTRFRecusadoDto>()));
                            detalheTRFRecusadosStaging.ForEach(x => x.ExecucaoId = execucaoId);
                            datasourceConectSys.Connection.BulkInsert(detalheTRFRecusadosStaging, "DetalheTRFRecusadoLoteStaging");

                            var veiculos = passagensReprovadas.Any(c => c.Veiculo != null) ? passagensReprovadas.Select(c => c.Veiculo) : null;
                            var veiculosLoteStaging = Mapper.Map<List<VeiculoLoteStaging>>((veiculos ?? new List<VeiculoDto>()));
                            veiculosLoteStaging.ForEach(x => x.ExecucaoId = execucaoId);
                            datasourceConectSys.Connection.BulkInsertTransacoes(veiculosLoteStaging, "VeiculoLoteStaging");

                            var transacoesRecusadasParceiro = passagensReprovadas.Any(c => c.TransacaoRecusadaParceiro != null) ? passagensReprovadas.Select(c => c.TransacaoRecusadaParceiro) : null;
                            var transacoesRecusadasParceiroStaging = Mapper.Map<List<TransacaoRecusadaParceiroLoteStaging>>((transacoesRecusadasParceiro ?? new List<TransacaoRecusadaParceiroEdiDto>()));
                            transacoesRecusadasParceiroStaging.ForEach(x => x.ExecucaoId = execucaoId);
                            datasourceConectSys.Connection.BulkInsertTransacoes(transacoesRecusadasParceiroStaging, "TransacaoRecusadaParceiroLoteStaging");

                            response.QtdDetalheTRFRecusadoStaging = detalheTRFRecusadosStaging.Count;
                            response.QtdVeiculoStaging = veiculosLoteStaging.Count;
                            response.QtdTransacaoRecusadaParceiroStaging = transacoesRecusadasParceiroStaging.Count;

                        }
                        catch (Exception ex)
                        {
                            Log.Fatal($"ID: {execucaoId} - Erro Fatal - Staging Passagens Reprovadas Edi - ConectSys.", ex);
                            response.SucessoStagingConectSys = false;
                        }

                    }
                    Log.Debug($"ID: {execucaoId} - Fim - Staging Passagens Reprovadas Edi - ConectSys.");

                }, passagensReprovadas);
            }
            catch (Exception ex)
            {
                Log.Fatal($"ID: {execucaoId} - Erro Fatal. Erro: {ex.Message}", ex);
            }

            sw.Stop();
            response.TempoExecucaoStagingConectSys = sw.Elapsed;

        }
        #endregion

    }
}
