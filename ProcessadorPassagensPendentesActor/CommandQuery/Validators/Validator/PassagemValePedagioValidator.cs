using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.Model;
using ProcessadorPassagensActors.CommandQuery.Queries;
using ProcessadorPassagensActors.CommandQuery.Queries.Filter;
using System.Collections.Generic;
using System.Linq;
using System;
using ProcessadorPassagensActors.CommandQuery.Exceptions;
using ProcessadorPassagensActors.CommandQuery.Dtos;
using ConectCar.Transacoes.Domain.ValueObject;
using ConectCar.Framework.Infrastructure.Log;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Validators.Validator
{
    public class PassagemValePedagioValidator : Loggable
    {
        private readonly ValePedagioArtespDto _retorno;
        private readonly ObterPassagemAnteriorQuery _passagemAnteriorQuery;
        private readonly ObterViagemAgendadaOriginalQuery _viagemAgendadaOriginalQuery;
        private readonly ObterCountDetalheViagemCanceladaPorViagemId _countDetalheViagemCanceladaPorViagemId;
        private readonly ObterDetalhesViagemPorViagemIdQuery _detalhesViagemPorViagemIdQuery;
        private readonly ObterViagemAgendadaPorPlacaPracaDataPassagemQuery _viagemAgendadaPorPlacaPracaDataPassagemQuery;
        private readonly ListarViagensASeremCanceladasQuery _listarViagensASeremCanceladasQuery;
        private readonly ObterPassagemImediatamenteAnteriorQuery _passagemImediatamenteAnteriorQuery;
        private readonly ObterNumeroVezesRecusadoParamValePedagioFinanceiroQuery _numeroVezesRecusadoParamValePedagioFinanceiroQuery;

        public PassagemValePedagioValidator()
        {
            _passagemAnteriorQuery = new ObterPassagemAnteriorQuery();
            _viagemAgendadaOriginalQuery = new ObterViagemAgendadaOriginalQuery();
            _countDetalheViagemCanceladaPorViagemId = new ObterCountDetalheViagemCanceladaPorViagemId();
            _detalhesViagemPorViagemIdQuery = new ObterDetalhesViagemPorViagemIdQuery();
            _viagemAgendadaPorPlacaPracaDataPassagemQuery = new ObterViagemAgendadaPorPlacaPracaDataPassagemQuery();
            _listarViagensASeremCanceladasQuery = new ListarViagensASeremCanceladasQuery();
            _passagemImediatamenteAnteriorQuery = new ObterPassagemImediatamenteAnteriorQuery();
            _numeroVezesRecusadoParamValePedagioFinanceiroQuery = new ObterNumeroVezesRecusadoParamValePedagioFinanceiroQuery();

            _retorno = new ValePedagioArtespDto
            {
                ViagensParaRetorno = new List<DetalheViagem>()
            };
        }

        public ValePedagioArtespDto Validate(PassagemPendenteArtesp passagemPendenteArtesp)
        {
            var passagemOriginal = DataBaseConnection.HandleExecution(_passagemAnteriorQuery.Execute, passagemPendenteArtesp);

            if (passagemOriginal != null && passagemPendenteArtesp.NumeroReenvio > passagemOriginal.NumeroReenvio)
            {
                var viagensCopiadas = RecriarEstruturaAgendamento(passagemPendenteArtesp);
                _retorno.ViagensParaRetorno.AddRange(viagensCopiadas);

                CancelarAgendamentos(viagensCopiadas);
            }

            if (passagemPendenteArtesp.MotivoSemValor != MotivoSemValor.CobrancaIndevida)
            {
                _retorno.MotivoNaoCompensado = UtilizarViagensAgendadas(passagemPendenteArtesp, out var viagemNaoCompensadaId);
                if (_retorno.MotivoNaoCompensado == MotivoNaoCompensado.ValorInvalido && viagemNaoCompensadaId.HasValue)
                    _retorno.ViagemNaoCompensadaId = viagemNaoCompensadaId.TryToInt();
            }

            return _retorno;
        }

        private MotivoNaoCompensado UtilizarViagensAgendadas(PassagemPendenteArtesp passagemPendenteArtesp, out int? viagemNaoCompensadaId)
        {
            long? viagemId = null;
            viagemNaoCompensadaId = null;

            if (_retorno.ViagensParaRetorno != null && _retorno.ViagensParaRetorno.Any())
            {
                viagemId = _retorno.ViagensParaRetorno.FirstOrDefault().Viagem.Id;
            }


            var listaViagens = ObterViagensAgendadas(passagemPendenteArtesp, viagemId);

            if (_retorno.ViagensParaRetorno != null && _retorno.ViagensParaRetorno.Any())
                listaViagens.AddRange(_retorno.ViagensParaRetorno.Where(c => c.StatusDetalheViagem == StatusDetalheViagem.Criada && c.PracaId == passagemPendenteArtesp.Praca.Id));
            var motivoNaoCompensado = MotivoNaoCompensado.SemMotivoNaoCompensado;



            DetalheViagem viagemAgendada = null;

            if (listaViagens != null && listaViagens.Count > 0)
            {
                if (passagemPendenteArtesp.NumeroReenvio > 0)
                {
                    var qtdRecusasValePedagio = DataBaseConnection.HandleExecution(_numeroVezesRecusadoParamValePedagioFinanceiroQuery.Execute);

                    var passagemOriginal = DataBaseConnection.HandleExecution(_passagemImediatamenteAnteriorQuery.Execute, passagemPendenteArtesp);

                    if (passagemOriginal != null &&
                        passagemPendenteArtesp.NumeroReenvio > passagemOriginal.NumeroReenvio)
                    {
                        viagemAgendada =
                            listaViagens.FirstOrDefault(x => x.ValorPassagem == passagemPendenteArtesp.Valor);

                        if (viagemAgendada == null && passagemPendenteArtesp.NumeroReenvio > qtdRecusasValePedagio)
                        {
                            viagemAgendada = listaViagens.FirstOrDefault();
                        }
                        else if (viagemAgendada == null)
                        {
                            viagemNaoCompensadaId = listaViagens.FirstOrDefault().Viagem.Id.TryToInt();
                            return MotivoNaoCompensado.ValorInvalido;
                        }

                    }
                    else
                    {
                        viagemNaoCompensadaId = listaViagens.FirstOrDefault().Viagem.Id.TryToInt();
                        return MotivoNaoCompensado.ValorInvalido;
                    }
                }
                else
                {
                    viagemAgendada = listaViagens.FirstOrDefault(x => x.ValorPassagem == passagemPendenteArtesp.Valor);

                    if (viagemAgendada == null)
                    {
                        throw new TransacaoParceiroException(MotivoNaoCompensado.ValorInvalido,
                            listaViagens.FirstOrDefault().Viagem.Id.TryToInt(),
                            passagemPendenteArtesp);
                    }
                }
            }


            try
            {
                if (viagemAgendada == null)
                {
                    if (_retorno.ViagensParaRetorno.Any(c => c.StatusDetalheViagem == StatusDetalheViagem.Criada))
                        return motivoNaoCompensado;

                    return motivoNaoCompensado;
                }

                if (viagemAgendada.StatusDetalheViagemId == (int)StatusDetalheViagem.Utilizada)
                    throw new DomainException("Detalhe de Viagem já possuí transação.");

                viagemAgendada.StatusDetalheViagemId = (int)(StatusDetalheViagem.Utilizada);

                if (!_retorno.ViagensParaRetorno.Any(c => c == viagemAgendada))
                {
                    _retorno.ViagensParaRetorno.Add(viagemAgendada); // save    
                }

                CancelarDetalhesDuplicados(viagemAgendada);

                passagemPendenteArtesp.ValePedagio = true;

                return motivoNaoCompensado;
            }
            catch (Exception ex)
            {
                if (listaViagens != null)
                {
                    var ret = listaViagens.FirstOrDefault();
                    if (ret != null)
                        Log.Debug(string.Format("Erro ao atribuir transação à viagem id:{0}", ret.Id), ex);

                }
                if (listaViagens != null)
                {
                    var ret = listaViagens.FirstOrDefault();
                    if (ret != null)
                        throw new DomainException(string.Format("Erro ao atribuir transação à viagem id:{0}", ret.Id));
                }

                throw;
            }
        }

        public void CancelarDetalhesDuplicados(DetalheViagem viagemAgendada)
        {

            var detalhesACancelarBancoDados = DataBaseConnection.HandleExecution(_listarViagensASeremCanceladasQuery.Execute, viagemAgendada).ToList();

            var detalhesParaCancelar = new List<DetalheViagem>();
            detalhesParaCancelar.AddRange(_retorno.ViagensParaRetorno.Where(c => c.StatusDetalheViagem == StatusDetalheViagem.Criada));

            foreach (var dv in detalhesACancelarBancoDados)
            {
                if (!_retorno.ViagensParaRetorno.Any(c => c.Id == dv.Id))
                {
                    detalhesParaCancelar.Add(dv);
                }
            }

            foreach (var detalheViagem in detalhesParaCancelar)
            {
                if (detalheViagem.StatusDetalheViagemId != (int)StatusDetalheViagem.CanceladaDuplicidade)
                {

                    detalheViagem.StatusDetalheViagemId = (int)StatusDetalheViagem.CanceladaDuplicidade;
                    detalheViagem.DataCancelamento = DateTime.Now;

                    if (detalheViagem.PracaId != viagemAgendada.PracaId)
                    {
                        if (!_retorno.ViagensParaRetorno.Any(c => c == detalheViagem))
                            _retorno.ViagensParaRetorno.Add(detalheViagem);
                    }
                }
            }
        }
        private IList<DetalheViagem> RecriarEstruturaAgendamento(PassagemPendenteArtesp passagemPendenteArtesp)
        {
            var retorno = new List<DetalheViagem>();


            var filtroParaPlacaDataPassagemETransacaoIdOriginal = new PlacaDataPassagemETransacaoIdOriginalFilter()
            {
                Placa = passagemPendenteArtesp.Placa,
                DataPassagem = passagemPendenteArtesp.DataPassagem,
                TransacaoIdOriginal = passagemPendenteArtesp.TransacaoPassagemIdAnterior
            };

            var viagens = DataBaseConnection.HandleExecution(_viagemAgendadaOriginalQuery.Execute, filtroParaPlacaDataPassagemETransacaoIdOriginal);

            foreach (var agendamento in viagens)
            {
                // update
                retorno.Add(new DetalheViagem
                {
                    Id = agendamento.Id,
                    CodigoPracaRoadCard = agendamento.CodigoPracaRoadCard,
                    DataCancelamento = agendamento.DataCancelamento,
                    PracaId = agendamento.PracaId,
                    Sequencia = agendamento.Sequencia,
                    StatusDetalheViagemId = (int)StatusDetalheViagem.CanceladaPorReenvio, // 5
                    ValorPassagem = agendamento.ValorPassagem,
                    Viagem = new Viagem()
                    {
                        Id = agendamento.ViagemId,
                        CnpjEmbarcador = agendamento.CnpjEmbarcador,
                        CodigoViagemParceiro = agendamento.CodigoViagemParceiro,
                        Embarcador = agendamento.Embarcador
                    }
                });

                var detalheCopia = new DetalheViagem
                {
                    Id = null,
                    PracaId = agendamento.PracaId,
                    CodigoPracaRoadCard = agendamento.CodigoPracaRoadCard,
                    Sequencia = agendamento.Sequencia,
                    ValorPassagem = agendamento.ValorPassagem,
                    StatusDetalheViagemId = (int)StatusDetalheViagem.Criada, // 1
                    Viagem = new Viagem()
                    {
                        Id = agendamento.ViagemId,
                        CnpjEmbarcador = agendamento.CnpjEmbarcador,
                        CodigoViagemParceiro = agendamento.CodigoViagemParceiro,
                        Embarcador = agendamento.Embarcador
                    }
                };

                retorno.Add(detalheCopia); // insert
            }

            return retorno.ToList();
        }

        private void CancelarAgendamentos(IList<DetalheViagem> viagensCopiadas)
        {
            if (viagensCopiadas != null && viagensCopiadas.Any(c => (c.Viagem.Id ?? 0) > 0))
            {
                var viagemIdOriginal = viagensCopiadas.First(c => (c.Viagem.Id ?? 0) > 0).Viagem.Id;

                var existeDetalheCancelado = DataBaseConnection.HandleExecution(_countDetalheViagemCanceladaPorViagemId.Execute, viagemIdOriginal.TryToInt());

                if (existeDetalheCancelado)
                {
                    var detalhes = DataBaseConnection.HandleExecution(_detalhesViagemPorViagemIdQuery.Execute, viagemIdOriginal ?? 0); // obtenho todas os detalhes viagem.
                    var detalhesParaCancelar = detalhes.Where(x => x.StatusDetalheViagemId == (int)StatusDetalheViagem.Criada).ToList();

                    // adiciono os recentemente criados
                    detalhesParaCancelar.AddRange(viagensCopiadas.Where(x => x.StatusDetalheViagemId == (int)StatusDetalheViagem.Criada));

                    foreach (var detalheViagem in detalhesParaCancelar)
                    {
                        if (detalheViagem.StatusDetalheViagemId == (int)StatusDetalheViagem.Criada)
                        {
                            detalheViagem.StatusDetalheViagemId = (int)StatusDetalheViagem.Cancelada;
                            detalheViagem.DataCancelamento = DateTime.Now;

                            if (_retorno.ViagensParaRetorno.All(c => c != detalheViagem))
                                _retorno.ViagensParaRetorno.Add(detalheViagem);
                        }
                    }
                }
            }
        }
        public List<DetalheViagem> ObterViagensAgendadas(PassagemPendenteArtesp passagemPendenteArtesp, long? viagemId)
        {
            var filter = new ObterViagemAgendadaPorPlacaPracaDataPassagemFilter
            {
                Placa = passagemPendenteArtesp.Placa,
                PracaId = passagemPendenteArtesp.Praca.Id,
                DataPassagem = passagemPendenteArtesp.DataPassagem,
                ViagemId = viagemId
            };

            var viagens = DataBaseConnection.HandleExecution(_viagemAgendadaPorPlacaPracaDataPassagemQuery.Execute, filter);
            return viagens.ToList();
        }
    }
}
