using Common.Logging;
using ConectCar.Framework.Backend.CommonQuery.Query;
using ConectCar.Framework.Domain.Model;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.Model;
using ProcessadorPassagensActors.CommandQuery.Exceptions;
using ProcessadorPassagensActors.CommandQuery.Queries;
using System;
using System.Linq;
using ConectCar.Framework.Infrastructure.Log;
using ProcessadorPassagensActors.CommandQuery.Queries.Filter;
using ProcessadorPassagensActors.Infrastructure;
using ConectCar.Framework.Infrastructure.Cqrs.Ado.Queries;
using ProcessadorPassagensActors.CommandQuery.Cache;

namespace ProcessadorPassagensActors.CommandQuery.Validators.Validator
{
    public class CategoriaVeiculoValidator : Loggable, IValidator
    {
        private readonly PassagemPendenteEDI _passagemPendenteEdi;

        public CategoriaVeiculoValidator(PassagemPendenteEDI passagemPendenteEdi)
        {
            _passagemPendenteEdi = passagemPendenteEdi;
        }

        public void Validate()
        {
            int quantidadeLimiteDivergencias;
            int quantidadeLimitePassagens;
            var categoriaIdentificada = _passagemPendenteEdi.CategoriaUtilizada;

            if (_passagemPendenteEdi.CategoriaUtilizada.Automovel && _passagemPendenteEdi.CategoriaUtilizada.Id != 1)
            {
                var obterCategoriaVeiculoPorCodigo = new ObterCategoriaVeiculo();

                //Codigo 1 = Automóvel, caminhonete e furgão (2 eixos simples).
                categoriaIdentificada = obterCategoriaVeiculoPorCodigo.Execute(1);
            }

            try
            {
                quantidadeLimitePassagens = ConfiguracaoSistemaCacheRepository.Obter(ConfiguracaoSistemaModel.QuantidadeDePassagensParaConfirmacaoDeCategoria).Valor.TryToInt();
                quantidadeLimiteDivergencias = ConfiguracaoSistemaCacheRepository.Obter(ConfiguracaoSistemaModel.QuantidadeLimiteDeDivergenciasParaReiniciarConfirmacaoDeCategoria).Valor.TryToInt();
            }
            catch (Exception ex)
            {
                Log.Info("Os parâmetros de limite do grupo de configuração de sistema CategoriaConfirmada são inválidos.", ex);
                return;
            }

            if (_passagemPendenteEdi.Adesao.Veiculo.CategoriaConfirmada)
            {
                //Se o veículo possui categoria confirmada e a categoria identificada pela transação é a mesma, então o processo deve continuar normalmente.
                if (_passagemPendenteEdi.Adesao.Veiculo.Categoria.Id == categoriaIdentificada.Id)
                {
                    _passagemPendenteEdi.Adesao.Veiculo.ContagemDivergenciaCategoriaConfirmada = null;
                }
                else
                {
                    //Se o veículo possui categoria confirmada, mas não é a mesma identificada pela transação.
                    var obterUltimaTransacaoPassagem = new ObterUltimaTransacaoPassagemTrnPorAdesaoIdQuery();
                    var ultimaTransacaoPassagem = obterUltimaTransacaoPassagem.Execute(_passagemPendenteEdi.Adesao.Id ?? 0);

                    if (ultimaTransacaoPassagem.CategoriaUtilizadaId == 0 && _passagemPendenteEdi.Adesao.AdesaoOrigemId != 0)
                        ultimaTransacaoPassagem = obterUltimaTransacaoPassagem.Execute(_passagemPendenteEdi.AdesaoOrigemId.TryToLong());

                    //Se a ultima transacao nao for encontrada eu forço uma categoria inexistente para indicar divergencia
                    int categoriaUltimaTransacao = ultimaTransacaoPassagem.CategoriaUtilizadaId != 0 ? ultimaTransacaoPassagem.CategoriaUtilizadaId : _passagemPendenteEdi.Adesao.Veiculo.Categoria.Id.TryToInt();

                    if (categoriaUltimaTransacao == 7 || categoriaUltimaTransacao == 8)
                        categoriaUltimaTransacao = 1;

                    var obterCategoriasRejeitadasQuery =
                        new ObterCategoriasRejeitadasQuery();

                    var contemCategoriaRegeitada = obterCategoriasRejeitadasQuery.Execute(
                        new ObterCategoriasRejeitadasFilter(_passagemPendenteEdi.Conveniado.Id ?? 0,
                            _passagemPendenteEdi.Adesao.Veiculo.Id ?? 0));

                    //Somente rejeito se a categoria estiver configurada como "rejeitável"
                    if (contemCategoriaRegeitada)
                    {
                        Log.Info($"Passagem recusada para o veículo {_passagemPendenteEdi.Placa} por divergência de categoria. Veículo tem categoria confirmada {_passagemPendenteEdi.Adesao.Veiculo.Categoria.Codigo} e conveniado recusa categorias divergentes para veículos dessa categoria.");
                        throw new EdiTransacaoException(CodigoRetornoTransacaoTRF.CATCobradaNaoCompativel, _passagemPendenteEdi);
                    }

                    //Se a categoria identificada for diferente da última transação, porém a última transação tem a mesma categoria que a categoria confirmada.
                    //Negaremos a criação da transação passagem
                    if (categoriaUltimaTransacao != categoriaIdentificada.Id &&
                            categoriaUltimaTransacao == _passagemPendenteEdi.Adesao.Veiculo.Categoria.Id &&
                            _passagemPendenteEdi.Adesao.Veiculo.ContagemDivergenciaCategoriaConfirmada == null)
                    {
                        _passagemPendenteEdi.Adesao.Veiculo.ContagemDivergenciaCategoriaConfirmada = 0;

                        if (_passagemPendenteEdi.Conveniado.AtivoProtocoloArtesp)
                            throw new EdiTransacaoException(CodigoRetornoTransacaoTRF.CATCobradaNaoCompativel, _passagemPendenteEdi);

                        throw new EdiTransacaoException(CodigoRetornoTransacaoTRF.CATCobradaNaoCompativel, _passagemPendenteEdi);
                    }

                    //Se a categoria identificada for diferente da última transação e a última transação já não for a mesma da categoria confirmada
                    //Contaremos a quantidade de divergências e reiniciaremos o processo de Categoria Confirmada
                    if (_passagemPendenteEdi.Adesao.Veiculo.ContagemDivergenciaCategoriaConfirmada != null)
                        _passagemPendenteEdi.Adesao.Veiculo.ContagemDivergenciaCategoriaConfirmada++;

                    if (_passagemPendenteEdi.Adesao.Veiculo.ContagemDivergenciaCategoriaConfirmada >= quantidadeLimiteDivergencias)
                    {
                        IniciarConfirmacaoCategoria(categoriaIdentificada);
                    }
                }
                //salva.
            }
            else
            {
                //Se a categoria ainda não está confirmada, verifica se é a primeira passagem e inicia a contagem de transações
                if (_passagemPendenteEdi.Adesao.Veiculo.ContagemConfirmacaoCategoria == null || _passagemPendenteEdi.Adesao.Veiculo.ContagemConfirmacaoCategoria == 0)
                {
                    IniciarConfirmacaoCategoria(categoriaIdentificada);
                }
                else
                {
                    //Se a categoria ainda não está confirmada
                    //Incrementa o contador de transações
                    _passagemPendenteEdi.Adesao.Veiculo.ContagemConfirmacaoCategoria++;

                    //Verifica se a categoria identificada é diferente da categoria em processo de confirmação
                    //Gera um alerta e reinicia a contagem de transações com a nova categoria identificada
                    if (_passagemPendenteEdi.Adesao.Veiculo.Categoria.Id != categoriaIdentificada.Id)
                    {
                        _passagemPendenteEdi.PossuiDivergenciaCategoriaVeiculo = true; // criar a divergencia no gerador passagem aprovada

                        IniciarConfirmacaoCategoria(categoriaIdentificada);
                    }

                    //Ao atingir o número definido de passagens atribui a categoria gravada o status de Categoria Confirmada
                    //Também atualiza o valor da categoria na ConfiguracaoAdesao para espelhar a informação para outros processos
                    if (_passagemPendenteEdi.Adesao.Veiculo.ContagemConfirmacaoCategoria >= quantidadeLimitePassagens)
                    {
                        SubstituirCategoriaVeiculo(_passagemPendenteEdi.Adesao.Veiculo.Categoria.Id ?? 0);

                        _passagemPendenteEdi.Adesao.Veiculo.DataConfirmacaoCategoria = DateTime.Now;
                        _passagemPendenteEdi.Adesao.Veiculo.CategoriaConfirmada = true;
                    }
                }
            }
        }

        private void SubstituirCategoriaVeiculo(long codigocategoria)
        {
            _passagemPendenteEdi.Adesao.ConfiguracaoAdesao.AdesaoProvisoria = false;

            var obterCategoriaQuery = new ObterCategoriaVeiculo();

            var categoriaveiculo = obterCategoriaQuery.Execute(codigocategoria.TryToInt());
            if (categoriaveiculo != null)
            {
                if (categoriaveiculo.Ativo)
                    _passagemPendenteEdi.Adesao.ConfiguracaoAdesao.Categoria.Id = categoriaveiculo.Id;
                else
                {
                    throw new DomainException("Categoria inválida");
                }
            }
            else
            {
                throw new DomainException("Categoria inválida");
            }

            if (categoriaveiculo.Ativo)
                _passagemPendenteEdi.Adesao.ConfiguracaoAdesao.Categoria.Id = categoriaveiculo.Id;
            else
                throw new DomainException("Categoria inválida");
        }

        private void IniciarConfirmacaoCategoria(CategoriaVeiculo categoriaIdentificada)
        {
            _passagemPendenteEdi.Adesao.Veiculo.DataConfirmacaoCategoria = null;
            _passagemPendenteEdi.Adesao.Veiculo.CategoriaConfirmada = false;
            _passagemPendenteEdi.Adesao.Veiculo.ContagemConfirmacaoCategoria = 1;
            _passagemPendenteEdi.Adesao.Veiculo.ContagemDivergenciaCategoriaConfirmada = null;
            _passagemPendenteEdi.Adesao.Veiculo.Categoria = categoriaIdentificada;
        }
    }
}
