using ConectCar.Framework.Infrastructure.Cqrs.Ado.Queries;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Dtos;
using ProcessadorPassagensActors.CommandQuery.Enums;
using ProcessadorPassagensActors.CommandQuery.Exceptions;
using ProcessadorPassagensActors.CommandQuery.Queries;
using ProcessadorPassagensActors.CommandQuery.Queries.Filter;
using ProcessadorPassagensActors.Infrastructure;
using System;

namespace ProcessadorPassagensActors.CommandQuery.Bo
{
    public class CalcularRepasseEdiBo : RepasseBase
    {
        private readonly PassagemAprovadaEDI _passagemAprovadaEdi;

        private readonly TransacaoPassagem _transacaoPassagem;

        public CalcularRepasseEdiBo(PassagemAprovadaEDI passagemAprovadaEdi, DbConnectionDataSource dbSysReadOnly, DbConnectionDataSource dbSysFallBack)
        {
            _passagemAprovadaEdi = passagemAprovadaEdi;
            DataSourceConectSysReadOnly = dbSysReadOnly;
            DataSourceFallBack = dbSysFallBack;

            var queryObterRepasse = new ObterRepasseQuery();

            _transacaoPassagem = passagemAprovadaEdi.StatusCobranca == StatusCobranca.Provisoria
                ? passagemAprovadaEdi.TransacaoProvisoria
                : passagemAprovadaEdi.Transacao;

            RepasseDto = queryObterRepasse.Execute(new ObterRepasseFilter(
                dataPassagem: _passagemAprovadaEdi.DataPassagem,
                conveniadoId: _passagemAprovadaEdi.Conveniado.Id ?? 0,
                pracaId: _passagemAprovadaEdi.Praca.Id ?? 0,
                codigoPista: _passagemAprovadaEdi.Pista.CodigoPista,
                planoId: _passagemAprovadaEdi.Adesao.PlanoId
            ));
        }

        public void Calcular()
        {
            if (RepasseDto == null)
                throw new EdiDomainException("ObterRepasse não configurado.", _passagemAprovadaEdi);

            _transacaoPassagem.DataRepasse = CalcularDataDeRepasse();
            _transacaoPassagem.RepasseId = RepasseDto.RepasseId;

            var possuiTransacaoProvisoriaQuery = new ObterCountPossuiTransacaoProvisoriaQuery();

            var obterConveniado = new ObterConveniadoQuery(DataSourceConectSysReadOnly, DataSourceFallBack);
            var conveniado = obterConveniado.Execute(_passagemAprovadaEdi.Conveniado.Id.TryToInt());

            if (possuiTransacaoProvisoriaQuery.Execute(_passagemAprovadaEdi))
            {
                var obterTransacaoProvisoria = new ObterTransacaoProvisoriaQuery();
                var transacaoProvisoria = obterTransacaoProvisoria.Execute(_passagemAprovadaEdi);

                if (_passagemAprovadaEdi.StatusCobranca == StatusCobranca.Confirmacao)
                {
                    if (transacaoProvisoria.Valor > _passagemAprovadaEdi.Valor)
                    {
                        _transacaoPassagem.Credito = true;
                        _transacaoPassagem.Valor = Math.Round(transacaoProvisoria.Valor - _transacaoPassagem.Valor, 2);
                    }
                    else
                    {
                        _transacaoPassagem.Credito = false;
                        _transacaoPassagem.Valor = Math.Round(_transacaoPassagem.Valor - transacaoProvisoria.Valor, 2);
                    }
                }

                var valorCorrigido = (_transacaoPassagem.Credito ? -1 * _transacaoPassagem.Valor : _transacaoPassagem.Valor) + (transacaoProvisoria.Credito ? -1 * transacaoProvisoria.Valor : transacaoProvisoria.Valor);

                _transacaoPassagem.TarifaDeInterconexao = valorCorrigido * RepasseDto.TarifaDeInterconexao / 100 ?? 0;
                _transacaoPassagem.ValorRepasse = valorCorrigido - _transacaoPassagem.TarifaDeInterconexao;

                if (_transacaoPassagem.TipoOperacao == TipoOperacaoMovimentoFinanceiro.PassagemValePedagio && conveniado.ConcessionariaParticipanteValePedagio)
                {
                    var tarifaValePedagio = conveniado.TaxaInterconexaoValePedagio;
                    var tarifaInterconexaoParceiro = RepasseDto.TarifaDeInterconexaoParceiro;
                    var tarifa = tarifaValePedagio + tarifaInterconexaoParceiro;

                    _transacaoPassagem.ValorRepasse = valorCorrigido - (valorCorrigido * (tarifa / 100));
                }
            }
            else
            {
                _transacaoPassagem.TarifaDeInterconexao = RepasseDto.TarifaDeInterconexao.HasValue ? (_transacaoPassagem.Valor * RepasseDto.TarifaDeInterconexao.Value) / 100 : 0;
                _transacaoPassagem.ValorRepasse = _transacaoPassagem.Valor - _transacaoPassagem.TarifaDeInterconexao;
                if (_transacaoPassagem.TipoOperacao == TipoOperacaoMovimentoFinanceiro.PassagemValePedagio && conveniado.ConcessionariaParticipanteValePedagio)
                {
                    var tarifaValePedagio = conveniado.TaxaInterconexaoValePedagio;
                    var tarifaInterconexaoParceiro = RepasseDto.TarifaDeInterconexaoParceiro;
                    var tarifa = tarifaValePedagio + tarifaInterconexaoParceiro;

                    _transacaoPassagem.ValorRepasse = _transacaoPassagem.Valor - (_transacaoPassagem.Valor * (tarifa / 100));
                }
            }
        }

        public virtual DateTime CalcularDataDeRepasse()
        {
            switch (RepasseDto.TipoRepasse)
            {
                case TipoRepasse.DiaFixoDoMes:
                    return CalcularDiaFixoDeRepasse();

                case TipoRepasse.QuantidadeDeDiasAposTransacoes:
                    return BuscarQuantidadeDeDiasAposTransacoes();
            }
            throw new DomainException(TipoDeRepasseInvalido);
        }
    }
}