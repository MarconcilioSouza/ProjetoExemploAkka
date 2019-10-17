using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConectCar.Cadastros.Conveniados.Backend.CommonQuery.Query;
using ConectCar.Cadastros.Domain.Model;
using ConectCar.Framework.Infrastructure.Cqrs.Ado.Queries;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.Model;
using ProcessadorPassagensActors.CommandQuery.Dtos;
using ProcessadorPassagensActors.CommandQuery.Enums;
using ProcessadorPassagensActors.CommandQuery.Exceptions;
using ProcessadorPassagensActors.CommandQuery.Queries;
using ProcessadorPassagensActors.CommandQuery.Queries.Filter;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Validators.Validator
{
    public class SlaListaNelaEdiValidator : IValidator
    {
        private readonly DbConnectionDataSource _dataSourceConectSysReadOnly;
        private readonly DbConnectionDataSource _dataSourceFallBack;
        private readonly PassagemPendenteEDI _passagemPendenteEdi;
        private readonly GenericValidator<PassagemPendenteEDI> _validatorRuleSet;
        private HistoricoListaNelaDto _historicoListaNela;
        private readonly ConcessionariaModel _conveniado;
        private int _tempoAtualizacaoPista;

        public SlaListaNelaEdiValidator(DbConnectionDataSource dbSysReadOnly, DbConnectionDataSource dbSysFallBack, PassagemPendenteEDI passagemPendenteEdi)
        {
            _dataSourceConectSysReadOnly = dbSysReadOnly;
            _dataSourceFallBack = dbSysFallBack;
            _passagemPendenteEdi = passagemPendenteEdi;
            _validatorRuleSet = new GenericValidator<PassagemPendenteEDI>();

            var obterConveniado = new ObterConveniadoQuery(_dataSourceConectSysReadOnly,
                _dataSourceFallBack);

            _conveniado = obterConveniado.Execute(_passagemPendenteEdi.Conveniado.CodigoProtocolo);
        }

        public void Validate()
        {
            var possuiTransacaoAprovadaManualmente = _validatorRuleSet.Validate(_passagemPendenteEdi, PassagemPendenteEdiValidatorEnum.PossuiTransacaoAprovadaManualmente.ToString());
            var validarSaldoSuficiente = new PossuiSaldoSuficienteValidator(_passagemPendenteEdi);

            var possuiSaldoSuficiente = validarSaldoSuficiente.Validate();


            if (!possuiTransacaoAprovadaManualmente || possuiSaldoSuficiente)
            {
                var obterhistoricoListaNela = new ObterHistoricoListaNelaQuery();
                _historicoListaNela = obterhistoricoListaNela.Execute(new ObterHistoricoListaNelaFilter
                {
                    TagId = _passagemPendenteEdi.Tag.Id ?? 0,
                    DataDePassagem = _passagemPendenteEdi.DataPassagem
                });

                if (_validatorRuleSet.Validate(_passagemPendenteEdi, PassagemPendenteEdiValidatorEnum.PossuiAdesaoAtiva.ToString()))
                {
                    if (_historicoListaNela != null)
                    {
                        _tempoAtualizacaoPista = (_passagemPendenteEdi.Praca.TempoAtualizacaoPista > 0
                            ? _passagemPendenteEdi.Praca.TempoAtualizacaoPista
                            : _conveniado?.TempoDeAtualizacaoDePista) ?? 0;

                        if (
                            _passagemPendenteEdi.Adesao.Tag.StatusTag == StatusTag.Bloqueada && 
                            (!_passagemPendenteEdi.Adesao.Cliente.PessoaFisica || (_passagemPendenteEdi.Adesao.Cliente.PessoaFisica && _passagemPendenteEdi.Adesao.AdesaoValePedagio))
                            && (_historicoListaNela.DataEntrada.AddMinutes(_tempoAtualizacaoPista) <= _passagemPendenteEdi.DataPassagem))
                        {
                            throw new EdiTransacaoException(CodigoRetornoTransacaoTRF.PassagemForaDoPeriodo, _passagemPendenteEdi);
                        }

                        VerificarSlaListaNela(possuiSaldoSuficiente);
                    }
                }
            }
        }


        private void VerificarSlaListaNela(bool possuiSaldoSuficiente)
        {
            if (_historicoListaNela != null)
            {
                if (possuiSaldoSuficiente)
                    return;

                VerificarSeEstavaNaListaNelaComSla();
            }

            ValidarTempoDeAtualizacaoDeTransacaoDaPraca(possuiSaldoSuficiente);
        }

        private void VerificarSeEstavaNaListaNelaComSla()
        {
            if (_historicoListaNela.DataEntrada.AddMinutes(_tempoAtualizacaoPista) <= _passagemPendenteEdi.DataPassagem
                && (!_historicoListaNela.DataSaida.HasValue || _historicoListaNela.DataSaida > _passagemPendenteEdi.DataPassagem))
                throw new EdiTransacaoException(CodigoRetornoTransacaoTRF.PassagemValidaListaNela, _passagemPendenteEdi);

        }


        private void ValidarTempoDeAtualizacaoDeTransacaoDaPraca(bool possuiSaldoSuficiente)
        {
            if (_passagemPendenteEdi.StatusCobranca != StatusCobranca.Confirmacao)
            {
                var verificarSePossuiIncidenteQuery = new ObterSePossuiIncidentesQuery();

                var possuiIncidente = verificarSePossuiIncidenteQuery.Execute(new ObterSePossuiIncidentesFilter
                {
                    DataPassagem = _passagemPendenteEdi.DataPassagem,
                    PistaId = _passagemPendenteEdi.Pista.Id.TryToInt(),
                    PracaId = _passagemPendenteEdi.Praca.Id.TryToInt()
                });


                if (possuiIncidente)
                    return;

                var intervaloPassagem = _passagemPendenteEdi.DataCriacao.Subtract(_passagemPendenteEdi.DataPassagem).TotalMinutes;

                if (_passagemPendenteEdi.Praca.TempoAtualizacaoTransacao.HasValue)
                {
                    if (intervaloPassagem > _passagemPendenteEdi.Praca.TempoAtualizacaoTransacao.Value)
                    {
                        if (!possuiSaldoSuficiente)
                            throw new EdiTransacaoException(CodigoRetornoTransacaoTRF.PassagemForaDoPeriodo, _passagemPendenteEdi);
                    }
                }
                else
                {
                    if (intervaloPassagem > _conveniado.TempoDeAtualizacaoDasTransacoes)
                    {
                        if (!possuiSaldoSuficiente)
                            throw new EdiTransacaoException(CodigoRetornoTransacaoTRF.PassagemForaDoPeriodo, _passagemPendenteEdi);
                    }
                }
            }
        }
    }
}
