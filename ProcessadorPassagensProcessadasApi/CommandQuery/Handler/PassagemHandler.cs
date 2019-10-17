using AutoMapper;
using ConectCar.Framework.Infrastructure.Cqrs.Commands;
using ConectCar.Framework.Infrastructure.Cqrs.Handlers;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Framework.Infrastructure.Transactions;
using ConectCar.Transacoes.Domain.Dto;
using Newtonsoft.Json;
using ProcessadorPassagensProcessadasApi.CommandQuery.Commands;
using ProcessadorPassagensProcessadasApi.CommandQuery.Commands.Args;
using ProcessadorPassagensProcessadasApi.CommandQuery.Commands.Filter;
using ProcessadorPassagensProcessadasApi.CommandQuery.Dtos;
using ProcessadorPassagensProcessadasApi.CommandQuery.Extension;
using ProcessadorPassagensProcessadasApi.CommandQuery.Response;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Handler
{
    public sealed class PassagemHandler : DataSourceHandlerBase, IAdoDataSourceProvider,
        ICommand<List<PassagemAprovadaArtespDto>, PassagensAprovadasArtespResponse>,
        ICommand<List<PassagemReprovadaArtespDto>, PassagensReprovadasArtespResponse>,
        ICommand<List<PassagemInvalidaArtespDto>>

    {
        public PassagemHandler()
        {

        }

        public DbConnectionDataSourceProvider AdoDataSourceProvider
        {
            get
            {
                return GetAdoProvider();
            }
        }

        protected override void Init()
        {
            AddProvider(new DbConnectionDataSourceProvider());
        }

        #region [PASSAGENS APROVADAS]

        /// <summary>
        /// Recebe coleção de passagens aprovadas para processamento no ConectSys e Mensageria.
        /// </summary>
        /// <param name="passagensAprovadas">PassagemAprovadaArtespDto[]</param>
        /// <returns>PassagensAprovadasArtespResponse</returns>
        public PassagensAprovadasArtespResponse Execute(List<PassagemAprovadaArtespDto> passagensAprovadas)
        {
            var response = new PassagensAprovadasArtespResponse();
            var execucaoId = Guid.NewGuid();
            response.ExecucaoId = execucaoId;

            Log.Info($"ID: {execucaoId} - Início - Execução Passagens Aprovadas Artesp.");

            if (passagensAprovadas != null && passagensAprovadas.Any())
            {
                SalvarAprovadasStagingConectSys(passagensAprovadas, response, execucaoId);

                SalvarProcessadasAprovadasStagingMensageria(passagensAprovadas, response, execucaoId);

                SalvarAprovadasConectSys(passagensAprovadas, response, execucaoId);

                SalvarProcessadasAprovadasMensageria(passagensAprovadas, response, execucaoId);
            }

            var jsonResponse = JsonConvert.SerializeObject(response);
            Log.Info($"ID: {execucaoId} - Fim - Execução Passagens Aprovadas Artesp. Retorno: {jsonResponse}.");

            return response;
        }

        /// <summary>
        /// Salva as passagens aprovadas nas tabelas de Staging do ConectSys.
        /// </summary>
        /// <param name="passagensAprovadas">PassagemAprovadaArtespDto</param>
        /// <param name="response">PassagensAprovadasArtespResponse</param>
        /// <param name="execucaoId">Sessão de execução.</param>
        private void SalvarAprovadasStagingConectSys(List<PassagemAprovadaArtespDto> passagensAprovadas, PassagensAprovadasArtespResponse response, Guid execucaoId)
        {
            var sw = new Stopwatch();
            sw.Start();

            try
            {
                TransactionContextHelper.ExecuteTransaction((aprovadas) =>
                {
                    Log.Debug($"ID: {execucaoId} - Início - Staging Passagens Aprovadas Artesp - ConectSys.");
                    using (var datasourceConectSys = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSys))
                    {
                        try
                        {
                            var aceites = passagensAprovadas.Any(c => c.AceiteManualReenvioPassagem != null) ? passagensAprovadas.Select(c => c.AceiteManualReenvioPassagem) : null;
                            var aceiteManualReenvioPassagemLoteStaging = Mapper.Map<List<AceiteManualReenvioPassagemLoteStaging>>((aceites ?? new List<AceiteManualReenvioPassagemDto>()));
                            aceiteManualReenvioPassagemLoteStaging.ForEach(x => x.ExecucaoId = execucaoId);
                            datasourceConectSys.Connection.BulkInsertTransacoes(aceiteManualReenvioPassagemLoteStaging, "AceiteManualReenvioPassagemLoteStaging");

                            var configuracoesAdesao = passagensAprovadas.Any(c => c.ConfiguracaoAdesao != null) ? passagensAprovadas.Select(c => c.ConfiguracaoAdesao) : null;
                            var configuracaoAdesaoLoteStaging = Mapper.Map<List<ConfiguracaoAdesaoLoteStaging>>((configuracoesAdesao ?? new List<ConfiguracaoAdesaoDto>()));
                            configuracaoAdesaoLoteStaging.ForEach(x => x.ExecucaoId = execucaoId);
                            datasourceConectSys.Connection.BulkInsertTransacoes(configuracaoAdesaoLoteStaging, "ConfiguracaoAdesaoLoteStaging");

                            var viagens = passagensAprovadas.Any(c => c.Viagens != null) ? passagensAprovadas.SelectMany(c => c.Viagens) : null;
                            var detalheViagemLoteStaging = Mapper.Map<List<DetalheViagemLoteStaging>>((viagens ?? new List<DetalheViagemDto>()));
                            detalheViagemLoteStaging.ForEach(x => x.ExecucaoId = execucaoId);
                            datasourceConectSys.Connection.BulkInsertTransacoes(detalheViagemLoteStaging, "DetalheViagemLoteStaging");

                            var divergencias = passagensAprovadas.Any(c => c.DivergenciaCategoriaConfirmada != null) ? passagensAprovadas.Select(c => c.DivergenciaCategoriaConfirmada) : null;
                            var divergenciaCategoriaConfirmadaLoteStaging = Mapper.Map<List<DivergenciaCategoriaConfirmadaLoteStaging>>((divergencias ?? new List<DivergenciaCategoriaConfirmadaDto>()));
                            divergenciaCategoriaConfirmadaLoteStaging.ForEach(x => x.ExecucaoId = execucaoId);
                            datasourceConectSys.Connection.BulkInsertTransacoes(divergenciaCategoriaConfirmadaLoteStaging, "DivergenciaCategoriaConfirmadaLoteStaging");

                            var estornos = passagensAprovadas.Any(c => c.EstornoPassagem != null) ? passagensAprovadas.Select(c => c.EstornoPassagem) : null;
                            var estornoPassagemLoteStaging = Mapper.Map<List<EstornoPassagemLoteStaging>>((estornos ?? new List<EstornoPassagemDto>()));
                            estornoPassagemLoteStaging.ForEach(x => x.ExecucaoId = execucaoId);
                            datasourceConectSys.Connection.BulkInsertTransacoes(estornoPassagemLoteStaging, "EstornoPassagemLoteStaging");

                            var eventos = passagensAprovadas.Any(c => c.EventoPrimeiraPassagemManual != null) ? passagensAprovadas.Select(c => c.EventoPrimeiraPassagemManual) : null;
                            var eventoLoteStaging = Mapper.Map<List<EventoLoteStaging>>((eventos ?? new List<EventoDto>()));
                            eventoLoteStaging.ForEach(x => x.ExecucaoId = execucaoId);
                            datasourceConectSys.Connection.BulkInsertTransacoes(eventoLoteStaging, "EventoLoteStaging");

                            var extratos = passagensAprovadas.Any(c => c.Extrato != null) ? passagensAprovadas.Select(c => c.Extrato) : null;
                            var extratoLoteStaging = Mapper.Map<List<ExtratoLoteStaging>>((extratos ?? new List<ExtratoDto>()));
                            extratoLoteStaging.ForEach(x => x.ExecucaoId = execucaoId);
                            datasourceConectSys.Connection.BulkInsertTransacoes(extratoLoteStaging, "ExtratoLoteStaging");

                            var extratosEstorno = passagensAprovadas.Any(c => c.ExtratoEstorno != null) ? passagensAprovadas.Select(c => c.ExtratoEstorno) : null;
                            var extratoEstornoLoteStaging = Mapper.Map<List<ExtratoLoteStaging>>((extratosEstorno ?? new List<ExtratoDto>()));
                            extratoEstornoLoteStaging.ForEach(x => x.ExecucaoId = execucaoId);
                            datasourceConectSys.Connection.BulkInsertTransacoes(extratoEstornoLoteStaging, "ExtratoLoteStaging");

                            var solicitacoes = passagensAprovadas.Any(c => c.SolicitacaoImagem != null) ? passagensAprovadas.Select(c => c.SolicitacaoImagem) : null;
                            var solicitacaoImagemLoteStaging = Mapper.Map<List<SolicitacaoImagemLoteStaging>>((solicitacoes ?? new List<SolicitacaoImagemDto>()));
                            solicitacaoImagemLoteStaging.ForEach(x => x.ExecucaoId = execucaoId);
                            datasourceConectSys.Connection.BulkInsertTransacoes(solicitacaoImagemLoteStaging, "SolicitacaoImagemLoteStaging");

                            var transacoes = passagensAprovadas.Any(c => c.TransacaoPassagemArtesp != null) ? passagensAprovadas.Select(c => c.TransacaoPassagemArtesp) : null;
                            var transacaoPassagemLoteStaging = Mapper.Map<List<TransacaoPassagemLoteStaging>>((transacoes ?? new List<TransacaoPassagemArtespDto>()));
                            transacaoPassagemLoteStaging.ForEach(x => x.ExecucaoId = execucaoId);
                            datasourceConectSys.Connection.BulkInsertTransacoes(transacaoPassagemLoteStaging, "TransacaoPassagemLoteStaging");

                            var veiculos = passagensAprovadas.Any(c => c.Veiculo != null) ? passagensAprovadas.Select(c => c.Veiculo) : null;
                            var veiculosLoteStaging = Mapper.Map<List<VeiculoLoteStaging>>((veiculos ?? new List<VeiculoDto>()));
                            veiculosLoteStaging.ForEach(x => x.ExecucaoId = execucaoId);
                            datasourceConectSys.Connection.BulkInsertTransacoes(veiculosLoteStaging, "VeiculoLoteStaging");


                            var passagens = passagensAprovadas.Any(c => c.Passagem != null) ? passagensAprovadas.Select(c => c.Passagem) : null;
                            var passagensLoteStaging = Mapper.Map<List<PassagemLoteStaging>>((passagens ?? new List<PassagemDto>()));
                            passagensLoteStaging.ForEach(x => x.ExecucaoId = execucaoId);
                            datasourceConectSys.Connection.BulkInsertTransacoes(passagensLoteStaging, "PassagemLoteStaging");


                            response.QtdAceiteManualReenvioStaging = aceiteManualReenvioPassagemLoteStaging.Count;
                            response.QtdConfiguracaoAdesaoStaging = configuracaoAdesaoLoteStaging.Count;
                            response.QtdViagemValePedagioStaging = detalheViagemLoteStaging.Count;
                            response.QtdDivergenciaCategoriaConfirmadaStaging = divergenciaCategoriaConfirmadaLoteStaging.Count;
                            response.QtdEstornoStaging = estornoPassagemLoteStaging.Count;
                            response.QtdEventoStaging = eventoLoteStaging.Count;
                            response.QtdExtratoStaging = extratoLoteStaging.Count;
                            response.QtdExtratoEstornoStaging = extratoEstornoLoteStaging.Count;
                            response.QtdSolicitacaoImagemStaging = solicitacaoImagemLoteStaging.Count;
                            response.QtdTransacaoPassagemStaging = transacaoPassagemLoteStaging.Count;
                            response.QtdVeiculosStaging = veiculosLoteStaging.Count;
                            response.QtdPassagensStaging = passagensLoteStaging.Count;
                            response.SucessoStagingConectSys = true;

                        }
                        catch (Exception ex)
                        {
                            Log.Fatal($"ID: {execucaoId} - Erro Fatal - Staging Passagens Aprovadas Artesp - ConectSys.", ex);
                            response.SucessoStagingConectSys = false;
                        }

                    }
                    Log.Debug($"ID: {execucaoId} - Fim - Staging Passagens aprovadas Artesp - ConectSys.");

                }, passagensAprovadas);
            }
            catch (Exception ex)
            {
                Log.Fatal($"ID: {execucaoId} - Erro Fatal. Erro: {ex.Message}", ex);
            }

            sw.Stop();
            response.TempoExecucaoStagingConectSys = sw.Elapsed;
        }

        /// <summary>
        /// Salva as passagens processadas que foram aprovadas nas tabelas de Staging do Mensageria.
        /// </summary>
        /// <param name="passagensAprovadas">PassagemAprovadaArtespDto[]</param>
        /// <param name="response">PassagensAprovadasArtespResponse</param>
        /// <param name="execucaoId">Sessão de execução.</param>
        private void SalvarProcessadasAprovadasStagingMensageria(List<PassagemAprovadaArtespDto> passagensAprovadas, PassagensAprovadasArtespResponse response, Guid execucaoId)
        {
            var sw = new Stopwatch();
            sw.Start();

            try
            {
                TransactionContextHelper.ExecuteTransaction((aprovadas) =>
                {
                    Log.Debug($"ID: {execucaoId} - Início - Staging Passagens Processadas - Mensageria.");
                    using (var datasourceConectSys = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.Mensageria))
                    {
                        try
                        {
                            var passagensProcessadas = passagensAprovadas.Any(c => c.PassagemProcessada != null) ? passagensAprovadas.Select(c => c.PassagemProcessada) : null;
                            var passagensProcessadasLoteStaging = Mapper.Map<List<PassagemProcessadaLoteStaging>>((passagensProcessadas ?? new List<PassagemProcessadaArtespDto>()));
                            passagensProcessadasLoteStaging.ForEach(x => { x.ExecucaoId = execucaoId; x.Aprovada = true; });
                            datasourceConectSys.Connection.BulkInsertTransacoes(passagensProcessadasLoteStaging, "PassagemProcessadaLoteStaging");


                            response.QtdPassagensProcessadasStaging = passagensProcessadasLoteStaging.Count;
                            response.SucessoStagingMensageria = true;

                        }
                        catch (Exception ex)
                        {
                            Log.Error($"ID: {execucaoId} - Erro Fatal - Staging Passagens Processadas - Mensageria.", ex);
                            response.SucessoStagingMensageria = false;
                        }

                    }
                    Log.Debug($"ID: {execucaoId} - Fim - Staging Passagens Processadas - Mensageria.");

                }, passagensAprovadas);
            }
            catch (Exception ex)
            {
                Log.Fatal($"ID: {execucaoId} - Erro Fatal. Erro: {ex.Message}", ex);
            }

            sw.Stop();
            response.TempoExecucaoStagingMensageria = sw.Elapsed;

        }

        /// <summary>
        /// Executa a procedure "SP_SalvarPassagensAprovadas" na base do ConectSys.
        /// </summary>
        /// <param name="passagensAprovadas">PassagemAprovadaArtespDto[]</param>
        /// <param name="response">PassagensAprovadasArtespResponse</param>
        /// <param name="execucaoId">Sessão de execução.</param>
        private void SalvarAprovadasConectSys(List<PassagemAprovadaArtespDto> passagensAprovadas, PassagensAprovadasArtespResponse response, Guid execucaoId)
        {
            var sw = new Stopwatch();
            sw.Start();

            try
            {
                TransactionContextHelper.ExecuteTransaction((qtdRegistros) =>
                {
                    Log.Debug($"ID: {execucaoId} - Início - Execução procedure SP_SalvarPassagensAprovadas - ConectSys.");
                    using (var datasource = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSys))
                    {
                        try
                        {
                            var command = new SalvarPassagensAprovadasSysCommand(datasource);
                            var filter = new PassagemAprovadaSysFilter
                            {
                                ExecucaoId = execucaoId,
                                Passagens = passagensAprovadas.Any(c => c.Passagem != null) ? passagensAprovadas.Select(c => c.Passagem) : null,
                                TransacoesPassagens = passagensAprovadas.Any(c => c.TransacaoPassagemArtesp != null) ? passagensAprovadas.Select(c => c.TransacaoPassagemArtesp) : null,
                                Extratos = passagensAprovadas.Any(c => c.Extrato != null) ? passagensAprovadas.Select(c => c.Extrato) : null,
                                EstornosPassagem = passagensAprovadas.Any(c => c.EstornoPassagem != null) ? passagensAprovadas.Select(c => c.EstornoPassagem) : null,
                                ExtratosEstornos = passagensAprovadas.Any(c => c.ExtratoEstorno != null) ? passagensAprovadas.Select(c => c.ExtratoEstorno) : null,
                                Veiculos = passagensAprovadas.Any(c => c.Veiculo != null) ? passagensAprovadas.Select(c => c.Veiculo) : null,
                                Eventos = passagensAprovadas.Any(c => c.EventoPrimeiraPassagemManual != null) ? passagensAprovadas.Select(c => c.EventoPrimeiraPassagemManual) : null,
                                DetalhesViagem = passagensAprovadas.Any(c => c.Viagens != null) ? passagensAprovadas.SelectMany(c => c.Viagens) : null,
                                SolicitacoesImagem = passagensAprovadas.Any(c => c.SolicitacaoImagem != null) ? passagensAprovadas.Select(c => c.SolicitacaoImagem) : null,
                                AceitesManuaisReenvioPassagem = passagensAprovadas.Any(c => c.AceiteManualReenvioPassagem != null) ? passagensAprovadas.Select(c => c.AceiteManualReenvioPassagem) : null,
                                ConfiguracoesAdesao = passagensAprovadas.Any(c => c.ConfiguracaoAdesao != null) ? passagensAprovadas.Select(c => c.ConfiguracaoAdesao) : null,
                                DivergenciasCategoriaConfirmada = passagensAprovadas.Any(c => c.DivergenciaCategoriaConfirmada != null) ? passagensAprovadas.Select(c => c.DivergenciaCategoriaConfirmada) : null
                            };

                            var dto = command.Execute(filter);

                            if (dto.Status)
                            {
                                response.SucessoConectSys = true;
                                response.ErroConectSys = String.Empty;
                            }
                            else
                            {
                                Log.Error($"ID: {execucaoId} - Erro - Execução procedure SP_SalvarPassagensAprovadas - ConectSys. Retorno: {dto}.");

                                response.SucessoConectSys = false;
                                response.ErroConectSys = dto.ToString();

                                AtualizarFalhaPassagensAprovadas(datasource, execucaoId, dto.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Fatal($"ID: {execucaoId} - Erro Fatal - Execução procedure SP_SalvarPassagensAprovadas - ConectSys.", ex);
                            response.SucessoConectSys = false;

                            AtualizarFalhaPassagensAprovadas(datasource, execucaoId, ex.Message);
                        }
                    }
                    Log.Debug($"ID: {execucaoId} - Fim - Execução procedure SP_SalvarPassagensAprovadas - ConectSys.");
                }, 0);
            }
            catch (Exception ex)
            {
                Log.Fatal($"ID: {execucaoId} - Erro Fatal. Erro: {ex.Message}", ex);
            }

            sw.Stop();
            response.TempoExecucaoConectSys = sw.Elapsed;

        }

        /// <summary>
        /// Executa a procedure "SP_SalvarPassagensProcessadas" na base do Mensageria.
        /// </summary>
        /// <param name="passagensAprovadas">PassagemAprovadaArtespDto[]</param>
        /// <param name="response">PassagensAprovadasArtespResponse</param>
        /// <param name="execucaoId">Sessão de execução.</param>
        private void SalvarProcessadasAprovadasMensageria(List<PassagemAprovadaArtespDto> passagensAprovadas, PassagensAprovadasArtespResponse response, Guid execucaoId)
        {
            var sw = new Stopwatch();
            sw.Start();

            try
            {
                TransactionContextHelper.ExecuteTransaction((qtdRegistros) =>
                {
                    Log.Debug($"ID: {execucaoId} - Início - Execução procedure SP_SalvarPassagensProcessadas - Mensageria.");
                    using (var datasourceMensageria = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.Mensageria))
                    {

                        try
                        {
                            var command = new SalvarPassagensProcessadasCommand(datasourceMensageria);
                            var passagensProcessadas = passagensAprovadas.Any(c => c.PassagemProcessada != null) ? passagensAprovadas.Select(c => c.PassagemProcessada).ToList() : null;
                            var filter = new PassagemProcessadaFilter
                            {
                                ExecucaoId = execucaoId,
                                PassagensProcessadas = passagensProcessadas
                            };

                            var dto = command.Execute(filter);
                            if (dto.Status)
                            {
                                response.SucessoMensageria = true;
                                response.ErroMensageria = String.Empty;
                            }
                            else
                            {
                                Log.Error($"ID: {execucaoId} - Erro - Execução procedure SP_SalvarPassagensProcessadas - Mensageria. Retorno: {dto}.");

                                response.SucessoMensageria = false;
                                response.ErroMensageria = dto.ToString();

                                AtualizarFalhaPassagensProcessadas(datasourceMensageria, execucaoId, dto.ToString());

                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Fatal($"ID: {execucaoId} - Erro Fatal - Execução procedure SP_SalvarPassagensProcessadas - Mensageria.", ex);
                            response.SucessoMensageria = false;

                            AtualizarFalhaPassagensProcessadas(datasourceMensageria, execucaoId, ex.Message);
                        }
                    }
                    Log.Debug($"ID: {execucaoId} - Fim - Execução procedure SP_SalvarPassagensProcessadas - Mensageria.");
                }, 0);
            }
            catch (Exception ex)
            {
                Log.Fatal($"ID: {execucaoId} - Erro Fatal. Erro: {ex.Message}", ex);
            }

            sw.Stop();
            response.TempoExecucaoMensageria = sw.Elapsed;

        }

        /// <summary>
        /// Atualiza o status de falha das passagens aprovadas.
        /// </summary>
        /// <param name="dataSource">DbConnectionDataSource</param>
        /// <param name="execucaoId">Sessão de execução.</param>
        /// <param name="motivo">Motivo do erro.</param>
        private void AtualizarFalhaPassagensAprovadas(DbConnectionDataSource dataSource, Guid execucaoId, string motivo)
        {
            Log.Debug($"ID: {execucaoId} - Início - Atualizar Falha Passagens Aprovadas Artesp - ConectSys.");

            try
            {
                var command = new AtualizarFalhaPassagensAprovadasCommand(dataSource);
                var args = new AtualizarFalhaPassagensAprovadasArgs
                {
                    ExecucaoId = execucaoId,
                    Motivo = motivo
                };
                command.Execute(args);
            }
            catch (Exception ex)
            {
                Log.Fatal($"ID: {execucaoId} - Erro Fatal - Atualizar Falha Passagens Aprovadas Artesp. Erro: {ex.Message}", ex);
            }

            Log.Debug($"ID: {execucaoId} - Fim - Atualizar Falha Passagens Aprovadas Artesp - ConectSys.");
        }

        #endregion

        #region [PASSAGENS REPROVADAS]

        /// <summary>
        /// Recebe coleção de passagens reprovadas para processamento no ConectSys e no Mensageria.
        /// </summary>
        /// <param name="PassagensReprovadas">PassagemReprovadaArtespDto[]</param>
        /// <returns>PassagensReprovadasArtespResponse</returns>
        public PassagensReprovadasArtespResponse Execute(List<PassagemReprovadaArtespDto> PassagensReprovadas)
        {

            var response = new PassagensReprovadasArtespResponse();

            var execucaoId = Guid.NewGuid();
            response.ExecucaoId = execucaoId;

            Log.Info($"ID: {execucaoId} - Início - Execução Passagens Reprovadas Artesp.");


            if (PassagensReprovadas != null && PassagensReprovadas.Any())
            {
                SalvarReprovadasStagingConectSys(PassagensReprovadas, response, execucaoId);

                SalvarProcessadasReprovadasStagingMensageria(PassagensReprovadas, response, execucaoId);

                SalvarReprovadasConectSys(PassagensReprovadas, response, execucaoId);

                SalvarProcessadasReprovadasMensageria(PassagensReprovadas, response, execucaoId);
            }


            var jsonResponse = JsonConvert.SerializeObject(response);
            Log.Info($"ID: {execucaoId} - Fim - Execução Passagens Reprovadas Artesp. Retorno: {jsonResponse}.");

            return response;
        }

        /// <summary>
        /// Salva as passagens reprovadas nas tabelas de Staging do ConectSys.
        /// </summary>
        /// <param name="passagensReprovadas">PassagemReprovadaArtespDto</param>
        /// <param name="response">PassagensReprovadasArtespResponse</param>
        /// <param name="execucaoId">Sessão de execução.</param>
        private void SalvarReprovadasStagingConectSys(List<PassagemReprovadaArtespDto> passagensReprovadas, PassagensReprovadasArtespResponse response, Guid execucaoId)
        {
            var sw = new Stopwatch();
            sw.Start();

            try
            {
                TransactionContextHelper.ExecuteTransaction((reprovadas) =>
                {
                    Log.Debug($"ID: {execucaoId} - Início - Staging Passagens Reprovadas Artesp - ConectSys.");
                    using (var datasourceConectSys = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSys))
                    {
                        try
                        {
                            var veiculos = passagensReprovadas.Any(c => c.Veiculo != null) ? passagensReprovadas.Select(c => c.Veiculo) : null;
                            var veiculosLoteStaging = Mapper.Map<List<VeiculoLoteStaging>>((veiculos ?? new List<VeiculoDto>()));
                            veiculosLoteStaging.ForEach(x => x.ExecucaoId = execucaoId);
                            datasourceConectSys.Connection.BulkInsertTransacoes(veiculosLoteStaging, "VeiculoLoteStaging");


                            var passagens = passagensReprovadas.Any(c => c.Passagem != null) ? passagensReprovadas.Select(c => c.Passagem) : null;
                            var passagensLoteStaging = Mapper.Map<List<PassagemLoteStaging>>((passagens ?? new List<PassagemDto>()));
                            passagensLoteStaging.ForEach(x => x.ExecucaoId = execucaoId);
                            datasourceConectSys.Connection.BulkInsertTransacoes(passagensLoteStaging, "PassagemLoteStaging");

                            var transacoesRecusadas = passagensReprovadas.Any(c => c.TransacaoRecusada != null) ? passagensReprovadas.Select(c => c.TransacaoRecusada) : null;
                            var transacoesRecusadasLoteStaging = Mapper.Map<List<TransacaoRecusadaLoteStaging>>((transacoesRecusadas ?? new List<TransacaoRecusadaDto>()));
                            transacoesRecusadasLoteStaging.ForEach(x => x.ExecucaoId = execucaoId);
                            datasourceConectSys.Connection.BulkInsertTransacoes(transacoesRecusadasLoteStaging, "TransacaoRecusadaLoteStaging");

                            var transacoesRecusadasParceiro = passagensReprovadas.Any(c => c.TransacaoRecusadaParceiro != null) ? passagensReprovadas.Select(c => c.TransacaoRecusadaParceiro) : null;
                            var transacoesRecusadasParceiroLoteStaging = Mapper.Map<List<TransacaoRecusadaParceiroLoteStaging>>((transacoesRecusadasParceiro ?? new List<TransacaoRecusadaParceiroDto>()));
                            transacoesRecusadasParceiroLoteStaging.ForEach(x => x.ExecucaoId = execucaoId);
                            datasourceConectSys.Connection.BulkInsertTransacoes(transacoesRecusadasParceiroLoteStaging, "TransacaoRecusadaParceiroLoteStaging");


                            response.QtdVeiculosStaging = veiculosLoteStaging.Count;
                            response.QtdPassagensStaging = passagensLoteStaging.Count;
                            response.QtdTransacoesRecusadasParceiroStaging = transacoesRecusadasParceiroLoteStaging.Count;
                            response.QtdTransacoesRecusadasStaging = transacoesRecusadasLoteStaging.Count;
                            response.SucessoStagingConectSys = true;

                        }
                        catch (Exception ex)
                        {
                            Log.Fatal($"ID: {execucaoId} - Erro Fatal - Staging Passagens Reprovadas Artesp - ConectSys.", ex);
                            response.SucessoStagingConectSys = false;
                        }

                    }
                    Log.Debug($"ID: {execucaoId} - Fim - Staging Passagens Reprovadas Artesp - ConectSys.");

                }, passagensReprovadas);
            }
            catch (Exception ex)
            {
                Log.Fatal($"ID: {execucaoId} - Erro Fatal. Erro: {ex.Message}", ex);
            }

            sw.Stop();
            response.TempoExecucaoStagingConectSys = sw.Elapsed;

        }

        /// <summary>
        /// Salva as passagens processadas que foram reprovadas nas tabelas de Staging do Mensageria.
        /// </summary>
        /// <param name="passagensReprovadas">PassagemReprovadaArtespDto[]</param>
        /// <param name="response">PassagensReprovadasArtespResponse</param>
        /// <param name="execucaoId">Sessão de execução.</param>
        private void SalvarProcessadasReprovadasStagingMensageria(List<PassagemReprovadaArtespDto> passagensReprovadas, PassagensReprovadasArtespResponse response, Guid execucaoId)
        {
            var sw = new Stopwatch();
            sw.Start();

            try
            {
                TransactionContextHelper.ExecuteTransaction((reprovadas) =>
                {
                    Log.Debug($"ID: {execucaoId} - Início - Staging Passagens Processadas - Mensageria.");
                    using (var datasourceConectSys = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.Mensageria))
                    {
                        try
                        {
                            var passagensProcessadas = passagensReprovadas.Any(c => c.PassagemProcessada != null) ? passagensReprovadas.Select(c => c.PassagemProcessada) : null;
                            var passagensProcessadasLoteStaging = Mapper.Map<List<PassagemProcessadaLoteStaging>>((passagensProcessadas ?? new List<PassagemProcessadaArtespDto>()));
                            passagensProcessadasLoteStaging.ForEach(x => { x.ExecucaoId = execucaoId; x.Aprovada = false; });
                            datasourceConectSys.Connection.BulkInsertTransacoes(passagensProcessadasLoteStaging, "PassagemProcessadaLoteStaging");


                            response.QtdPassagensProcessadasStaging = passagensProcessadasLoteStaging.Count;
                            response.SucessoStagingMensageria = true;

                        }
                        catch (Exception ex)
                        {
                            Log.Error($"ID: {execucaoId} - Erro Fatal - Staging Passagens Processadas - Mensageria.", ex);
                            response.SucessoStagingMensageria = false;
                        }

                    }
                    Log.Debug($"ID: {execucaoId} - Fim - Staging Passagens Processadas - Mensageria.");

                }, passagensReprovadas);
            }
            catch (Exception ex)
            {
                Log.Fatal($"ID: {execucaoId} - Erro Fatal. Erro: {ex.Message}", ex);
            }

            sw.Stop();
            response.TempoExecucaoStagingMensageria = sw.Elapsed;

        }

        /// <summary>
        /// Executa a procedure "SP_SalvarPassagensReprovadas" na base do ConectSys.
        /// </summary>
        /// <param name="passagensReprovadas">PassagemReprovadaArtespDto[]</param>
        /// <param name="response">PassagensReprovadasArtespResponse</param>
        /// <param name="execucaoId">Sessão de execução.</param>
        private void SalvarReprovadasConectSys(List<PassagemReprovadaArtespDto> passagensReprovadas, PassagensReprovadasArtespResponse response, Guid execucaoId)
        {
            var sw = new Stopwatch();
            sw.Start();

            try
            {
                TransactionContextHelper.ExecuteTransaction((qtdRegistros) =>
                {
                    Log.Debug($"ID: {execucaoId} - Início - Execução procedure SP_SalvarPassagensReprovadas - ConectSys.");
                    using (var datasourceConectSys = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSys))
                    {
                        try
                        {

                            var command = new SalvarPassagensReprovadasSysCommand(datasourceConectSys);
                            var filter = new PassagemReprovadaSysFilter
                            {
                                ExecucaoId = execucaoId,
                                Passagens = passagensReprovadas.Any(c => c.Passagem != null) ? passagensReprovadas.Select(c => c.Passagem) : null,
                                TransacoesRecusada = passagensReprovadas.Any(c => c.TransacaoRecusada != null) ? passagensReprovadas.Select(c => c.TransacaoRecusada) : null,
                                TransacoesRecusadaParceiro = passagensReprovadas.Any(c => c.TransacaoRecusadaParceiro != null) ? passagensReprovadas.Select(c => c.TransacaoRecusadaParceiro) : null,
                                Veiculos = passagensReprovadas.Any(c => c.Veiculo != null) ? passagensReprovadas.Select(c => c.Veiculo) : null
                            };

                            var dto = command.Execute(filter);
                            if (dto.Status)
                            {
                                response.SucessoConectSys = true;
                                response.ErroConectSys = String.Empty;
                            }
                            else
                            {
                                Log.Error($"ID: {execucaoId} - Erro - Execução procedure SP_SalvarPassagensReprovadas - ConectSys. Retorno: {dto}.");

                                response.SucessoConectSys = false;
                                response.ErroConectSys = dto.ToString();

                                AtualizarFalhaPassagensReprovadas(datasourceConectSys, execucaoId, dto.ToString());
                            }


                        }
                        catch (Exception ex)
                        {
                            Log.Fatal($"ID: {execucaoId} - Erro Fatal - Execução procedure SP_SalvarPassagensReprovadas - ConectSys.", ex);
                            response.SucessoConectSys = false;

                            AtualizarFalhaPassagensReprovadas(datasourceConectSys, execucaoId, ex.Message);
                        }

                    }

                    Log.Debug($"ID: {execucaoId} - Fim - Execução procedure SP_SalvarPassagensReprovadas - ConectSys.");
                }, 0);
            }
            catch (Exception ex)
            {
                Log.Fatal($"ID: {execucaoId} - Erro Fatal. Erro: {ex.Message}", ex);
            }

            sw.Stop();
            response.TempoExecucaoConectSys = sw.Elapsed;
        }

        /// <summary>
        /// Executa a procedure "SP_SalvarPassagensProcessadas" na base do Mensageria.
        /// </summary>
        /// <param name="passagensReprovadas">PassagemReprovadaArtespDto[]</param>
        /// <param name="response">PassagensReprovadasArtespResponse</param>
        /// <param name="execucaoId">Sessão de execução.</param>
        private void SalvarProcessadasReprovadasMensageria(List<PassagemReprovadaArtespDto> passagensReprovadas, PassagensReprovadasArtespResponse response, Guid execucaoId)
        {
            var sw = new Stopwatch();
            sw.Start();

            try
            {
                TransactionContextHelper.ExecuteTransaction((qtdRegistros) =>
                {
                    Log.Debug($"ID: {execucaoId} - Início - Execução procedure SP_SalvarPassagensProcessadas - Mensageria.");
                    using (var datasourceMensageria = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.Mensageria))
                    {

                        try
                        {
                            var command = new SalvarPassagensProcessadasCommand(datasourceMensageria);
                            var passagensProcessadas = passagensReprovadas.Any(c => c.PassagemProcessada != null) ? passagensReprovadas.Select(c => c.PassagemProcessada).ToList() : null;
                            var filter = new PassagemProcessadaFilter
                            {
                                ExecucaoId = execucaoId,
                                PassagensProcessadas = passagensProcessadas
                            };

                            var dto = command.Execute(filter);
                            if (dto.Status)
                            {
                                response.SucessoMensageria = true;
                                response.ErroMensageria = String.Empty;
                            }
                            else
                            {
                                Log.Error($"ID: {execucaoId} - Erro - Execução procedure SP_SalvarPassagensProcessadas - Mensageria. Retorno: {dto}.");

                                response.SucessoMensageria = false;
                                response.ErroMensageria = dto.ToString();

                                AtualizarFalhaPassagensProcessadas(datasourceMensageria, execucaoId, dto.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error($"ID: {execucaoId} - Erro Fatal - Execução procedure SP_SalvarPassagensProcessadas - Mensageria.", ex);
                            response.SucessoMensageria = false;

                            AtualizarFalhaPassagensProcessadas(datasourceMensageria, execucaoId, ex.Message);
                        }
                    }
                    Log.Debug($"ID: {execucaoId} - Fim - Execução procedure SP_SalvarPassagensProcessadas - Mensageria.");
                }, 0);
            }
            catch (Exception ex)
            {
                Log.Fatal($"ID: {execucaoId} - Erro Fatal. Erro: {ex.Message}", ex);
            }

            sw.Stop();
            response.TempoExecucaoMensageria = sw.Elapsed;

        }

        /// <summary>
        /// Atualiza o status de falha das passagens reprovadas.
        /// </summary>
        /// <param name="dataSource">DbConnectionDataSource</param>
        /// <param name="execucaoId">Sessão de execução.</param>
        /// <param name="motivo">Motivo do erro.</param>
        private void AtualizarFalhaPassagensReprovadas(DbConnectionDataSource dataSource, Guid execucaoId, string motivo)
        {
            Log.Debug($"ID: {execucaoId} - Início - Atualizar Falha Passagens Reprovadas Artesp - ConectSys.");

            try
            {
                var command = new AtualizarFalhaPassagensReprovadasCommand(dataSource);
                var args = new AtualizarFalhaPassagensReprovadasArgs
                {
                    ExecucaoId = execucaoId,
                    Motivo = motivo
                };
                command.Execute(args);
            }
            catch (Exception ex)
            {
                Log.Fatal($"ID: {execucaoId} - Erro Fatal - Atualizar Falha Passagens Reprovadas Artesp. Erro: {ex.Message}", ex);
            }

            Log.Debug($"ID: {execucaoId} - Fim - Atualizar Falha Passagens Reprovadas Artesp - ConectSys.");
        }

        #endregion

        /// <summary>
        /// Atualiza o status de falha das passagens processadas.
        /// </summary>
        /// <param name="dataSource">DbConnectionDataSource</param>
        /// <param name="execucaoId">Sessão de execução.</param>
        /// <param name="motivo">Motivo do erro.</param>
        private void AtualizarFalhaPassagensProcessadas(DbConnectionDataSource dataSource, Guid execucaoId, string motivo)
        {
            Log.Debug($"ID: {execucaoId} - Início - Atualizar Falha Passagens Processadas Artesp - Mensageria.");

            try
            {
                var command = new AtualizarFalhaPassagensProcessadasCommand(dataSource);
                var args = new AtualizarFalhaPassagensProcessadasArgs
                {
                    ExecucaoId = execucaoId,
                    Motivo = motivo
                };
                command.Execute(args);
            }
            catch (Exception ex)
            {
                Log.Fatal($"ID: {execucaoId} - Erro Fatal - Atualizar Falha Passagens Processadas Artesp. Erro: {ex.Message}", ex);
            }

            Log.Debug($"ID: {execucaoId} - Fim - Atualizar Falha Passagens Processadas Artesp - Mensageria.");
        }

        // inválidas
        public void Execute(List<PassagemInvalidaArtespDto> PassagensInvalidas)
        {
            // Only Mensageria
            if (PassagensInvalidas != null && PassagensInvalidas.Any())
            {
                using (var datasource = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.Mensageria))
                {
                    var command = new SalvarPassagensInvalidasMensageriaCommand(datasource);
                    PassagensInvalidas = PassagensInvalidas ?? new List<PassagemInvalidaArtespDto>();
                    command.Execute(PassagensInvalidas);
                }
            }
        }
    }
}
