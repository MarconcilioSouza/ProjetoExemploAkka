using System;
using ConectCar.Transacoes.Domain.Enum;
using ProcessadorPassagensActors.CommandQuery.Cache;
using ProcessadorPassagensActors.CommandQuery.Enums;
using ProcessadorPassagensActors.CommandQuery.Exceptions;
using ProcessadorPassagensActors.CommandQuery.Queries;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Queries.Filter;
using ConectCar.Framework.Infrastructure.Log;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Validators.Validator
{
    public class DivergenciaCategoriaValidator : Loggable
    {

        private ObterCategoriaVeiculo _categoriaVeiculo;
        private ObterTransacaoPassagemPorAdesaoIdQuery _transacaoPassagemPorAdesaoIdQuery;
        private ObterCategoriasRejeitadasQuery _categoriasRejeitadasQuery;
        private int _quantidadeLimitePassagens;
        private int _quantidadeLimiteDivergencias;

        public DivergenciaCategoriaValidator()
        {
            _categoriaVeiculo = new ObterCategoriaVeiculo();
            _transacaoPassagemPorAdesaoIdQuery = new ObterTransacaoPassagemPorAdesaoIdQuery();
            _categoriasRejeitadasQuery = new ObterCategoriasRejeitadasQuery();

            var qtdPassagensConfiguracao = ConfiguracaoSistemaCacheRepository.Obter(NomeConfiguracaoSistema
                    .QuantidadeDePassagensParaConfirmacaoDeCategoria.ToString());

            var qtdDivergenciasConfiguracao = ConfiguracaoSistemaCacheRepository.Obter(NomeConfiguracaoSistema
                    .QuantidadeLimiteDeDivergenciasParaReiniciarConfirmacaoDeCategoria.ToString());

            _quantidadeLimitePassagens = qtdPassagensConfiguracao.Valor.TryToInt();
            _quantidadeLimiteDivergencias = qtdDivergenciasConfiguracao.Valor.TryToInt();
        }

        public PassagemPendenteArtesp Validate(PassagemPendenteArtesp passagemPendenteArtesp)
        {
            if (passagemPendenteArtesp.Conveniado.HabilitarConfirmacaoCategoria)
            {

                var categoriaIdentificada = passagemPendenteArtesp.CategoriaUtilizada;

                if (passagemPendenteArtesp.CategoriaDetectada.Automovel && passagemPendenteArtesp.CategoriaDetectada.Codigo != 1)
                    categoriaIdentificada = DataBaseConnection.HandleExecution(_categoriaVeiculo.Execute, 1);

                if (passagemPendenteArtesp.Adesao.Veiculo.CategoriaConfirmada)
                {

                    if (passagemPendenteArtesp.Adesao.Veiculo.Categoria.Codigo == categoriaIdentificada.Codigo)
                    {
                        passagemPendenteArtesp.Adesao.Veiculo.ContagemDivergenciaCategoriaConfirmada = null;
                    }
                    else
                    {

                        long categoriaUltimaTransacao = 0;

                        var categoriaUtilizadaId = DataBaseConnection.HandleExecution(_transacaoPassagemPorAdesaoIdQuery.Execute, passagemPendenteArtesp.Adesao.Id ?? 0);

                        if (categoriaUtilizadaId.HasValue)
                        {
                            categoriaUltimaTransacao = categoriaUtilizadaId.Value;
                        }
                        else
                        {
                            // busca pela AdesaoOrigemId se existir
                            categoriaUtilizadaId = DataBaseConnection.HandleExecution(_transacaoPassagemPorAdesaoIdQuery.Execute, Convert.ToInt64(passagemPendenteArtesp.Adesao.AdesaoOrigemId));
                            if (categoriaUtilizadaId.HasValue)
                            {
                                categoriaUltimaTransacao = categoriaUtilizadaId.Value;
                            }
                            else
                            {
                                categoriaUltimaTransacao = passagemPendenteArtesp.Adesao.Veiculo.Categoria.Codigo;
                            }
                        }


                        if (categoriaUltimaTransacao == 7 || categoriaUltimaTransacao == 8)
                            categoriaUltimaTransacao = 1;


                        var filter = new ObterCategoriasRejeitadasFilter(passagemPendenteArtesp.Conveniado.Id ?? 0,
                                passagemPendenteArtesp.Adesao.Veiculo.Id ?? 0);

                        var contemcategoriasRejeitadas = DataBaseConnection.HandleExecution(_categoriasRejeitadasQuery.Execute, filter);

                        if (contemcategoriasRejeitadas)
                        {
                            Log.Error($"Passagem recusada para o veículo {passagemPendenteArtesp.Adesao.Veiculo.Placa} por divergência de categoria. Veículo tem categoria confirmada {passagemPendenteArtesp.Adesao.Veiculo.Categoria.Codigo.ToString()} e conveniado recusa categorias divergentes para veículos dessa categoria.");
                            throw new PassagemDivergenciaCategoriaException(MotivoNaoCompensado.CategoriaDivergente, passagemPendenteArtesp);
                        }


                        if (categoriaUltimaTransacao != categoriaIdentificada.Codigo
                                && categoriaUltimaTransacao == passagemPendenteArtesp.Adesao.Veiculo.Categoria.Codigo
                                && passagemPendenteArtesp.Adesao.Veiculo.ContagemDivergenciaCategoriaConfirmada == null)
                        {

                            passagemPendenteArtesp.Adesao.Veiculo.ContagemDivergenciaCategoriaConfirmada = 0;
                            throw new PassagemDivergenciaCategoriaException(MotivoNaoCompensado.CategoriaDivergente, passagemPendenteArtesp);

                        }

                        passagemPendenteArtesp.Adesao.Veiculo.ContagemDivergenciaCategoriaConfirmada++;
                        if (passagemPendenteArtesp.Adesao.Veiculo.ContagemDivergenciaCategoriaConfirmada >= _quantidadeLimiteDivergencias)
                        {
                            passagemPendenteArtesp.Adesao.Veiculo.DataConfirmacaoCategoria = null;
                            passagemPendenteArtesp.Adesao.Veiculo.CategoriaConfirmada = false;
                            passagemPendenteArtesp.Adesao.Veiculo.ContagemConfirmacaoCategoria = 1;
                            passagemPendenteArtesp.Adesao.Veiculo.ContagemDivergenciaCategoriaConfirmada = null;
                            passagemPendenteArtesp.Adesao.Veiculo.Categoria = categoriaIdentificada;
                        }
                    }
                }
                else
                {
                    if (passagemPendenteArtesp.Adesao.Veiculo.ContagemConfirmacaoCategoria == null || passagemPendenteArtesp.Adesao.Veiculo.ContagemConfirmacaoCategoria == 0)
                    {
                        passagemPendenteArtesp.Adesao.Veiculo.DataConfirmacaoCategoria = null;
                        passagemPendenteArtesp.Adesao.Veiculo.CategoriaConfirmada = false;
                        passagemPendenteArtesp.Adesao.Veiculo.ContagemConfirmacaoCategoria = 1;
                        passagemPendenteArtesp.Adesao.Veiculo.ContagemDivergenciaCategoriaConfirmada = null;
                        passagemPendenteArtesp.Adesao.Veiculo.Categoria = categoriaIdentificada;
                    }
                    else
                    {
                        passagemPendenteArtesp.Adesao.Veiculo.ContagemConfirmacaoCategoria++;

                        if (passagemPendenteArtesp.Adesao.Veiculo.Categoria.Codigo != categoriaIdentificada.Id)
                        {
                            passagemPendenteArtesp.PossuiDivergenciaCategoriaVeiculo = true;

                            passagemPendenteArtesp.Adesao.Veiculo.DataConfirmacaoCategoria = null;
                            passagemPendenteArtesp.Adesao.Veiculo.CategoriaConfirmada = false;
                            passagemPendenteArtesp.Adesao.Veiculo.ContagemConfirmacaoCategoria = 1;
                            passagemPendenteArtesp.Adesao.Veiculo.ContagemDivergenciaCategoriaConfirmada = null;
                            passagemPendenteArtesp.Adesao.Veiculo.Categoria = categoriaIdentificada;
                        }

                        if (passagemPendenteArtesp.Adesao.Veiculo.ContagemConfirmacaoCategoria >= _quantidadeLimitePassagens)
                        {
                            passagemPendenteArtesp.Adesao.ConfiguracaoAdesao.AdesaoProvisoria = false;


                            var categoria = DataBaseConnection.HandleExecution(_categoriaVeiculo.Execute, passagemPendenteArtesp.Adesao.Veiculo.Categoria.Id.TryToInt());

                            if (categoria != null && categoria.Ativo)
                            {
                                passagemPendenteArtesp.Adesao.Veiculo.Categoria = categoria;
                            }
                            else
                            {
                                throw new DomainException("Categoria Inválida.");
                            }

                            passagemPendenteArtesp.Adesao.Veiculo.DataConfirmacaoCategoria = DateTime.Now;
                            passagemPendenteArtesp.Adesao.Veiculo.CategoriaConfirmada = true;
                        }
                    }
                }
            }

            return passagemPendenteArtesp;
        }
    }
}
