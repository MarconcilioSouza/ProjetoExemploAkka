using ConectCar.Framework.Infrastructure.Cqrs.Ado.Queries;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Enums;
using ProcessadorPassagensActors.CommandQuery.Exceptions;
using ProcessadorPassagensActors.CommandQuery.Queries;
using ProcessadorPassagensActors.CommandQuery.Queries.Filter;
using ProcessadorPassagensActors.Infrastructure;
using System;
using ConectCar.Transacoes.Domain.Model;

namespace ProcessadorPassagensActors.CommandQuery.Bo
{
    public class CalcularRepasseParkBo : RepasseBase
    {
        private readonly PassagemAprovadaEstacionamento _passagemAprovada;
        private DateTime? DtReferencia { get; set; }

        public CalcularRepasseParkBo(DbConnectionDataSource dbSysReadOnly, DbConnectionDataSource dbSysFallBack, PassagemAprovadaEstacionamento passagemAprovadaEstacionamento)
        {
            _passagemAprovada = passagemAprovadaEstacionamento;
            DataSourceConectSysReadOnly = dbSysReadOnly;
            DataSourceFallBack = dbSysFallBack;

            var obterRepasse = new ObterRepasseVigentePorPlanoQuery();

            var filter = new ObterRepasseFilter(passagemAprovadaEstacionamento.Pista.CodigoPista, passagemAprovadaEstacionamento.DataPassagem);
            RepasseDto = obterRepasse.Execute(filter);
        }

        public void Calcular()
        {
            _passagemAprovada.TransacaoEstacionamento.RepasseId = RepasseDto.RepasseId;

            // Calcular tarifa de interconexão
            _passagemAprovada.TransacaoEstacionamento.TarifaDeInterconexao = RepasseDto.TarifaDeInterconexao.HasValue ? (_passagemAprovada.TransacaoEstacionamento.Valor * RepasseDto.TarifaDeInterconexao.Value) / 100 : 0;

            // calcular valor de rapasse
            _passagemAprovada.TransacaoEstacionamento.ValorRepasse =
                _passagemAprovada.TransacaoEstacionamento.Valor -
                _passagemAprovada.TransacaoEstacionamento.TarifaDeInterconexao;

            DtReferencia = DefinirDataReferencia(_passagemAprovada.TransacaoEstacionamento.DataHoraTransacao);
            _passagemAprovada.TransacaoEstacionamento.DataReferencia = DtReferencia ?? DateTime.Now;

            // calcular data de repasse.
            _passagemAprovada.TransacaoEstacionamento.DataRepasse = CalcularDataDeRepasse();
        }

        private DateTime? DefinirDataReferencia(DateTime dataHoraTransacao)
        {
            DateTime? dataReferencia = null;

            var obterhoraFechamentoQuery = new ObterHorarioFechamentoConveniadoDayChangeQuery();
            var horaFechamento = obterhoraFechamentoQuery.Execute(_passagemAprovada.Conveniado.Id.TryToLong());

            if (horaFechamento == null)
                dataReferencia = null;
            else
            {
                var hFechamento = horaFechamento ?? DateTime.Now;
                var dataReferenciaTransacao = (dataHoraTransacao <
                                               dataHoraTransacao.Date.Add(
                                                   hFechamento.TimeOfDay))
                    ? dataHoraTransacao
                    : dataHoraTransacao.AddDays(1);

                var dayChangePosMeioDia = hFechamento.TimeOfDay > TimeSpan.FromHours(12);

                if (dayChangePosMeioDia)
                    dataReferenciaTransacao = dataHoraTransacao.Date == dataReferenciaTransacao.Date ? dataHoraTransacao : dataReferenciaTransacao.Date;
                else
                    dataReferenciaTransacao = dataHoraTransacao.Date == dataReferenciaTransacao.Date ? dataHoraTransacao.AddDays(-1).Date.Add(new TimeSpan(23, 59, 59)) : dataHoraTransacao;

                var obterCountFechamentoDiasQuery = new ObterCountFechamentoPorConveniadoEDataQuery();
                var existeFechamento = obterCountFechamentoDiasQuery.Execute(new ObterCountFechamentoPorConveniadoEDataFilter
                {
                    ConveniadoId = _passagemAprovada.Conveniado.Id.TryToLong(),
                    DataReferenciaTransacao = dataReferenciaTransacao
                });


                var obterDataReferenciaQuery = new ObterDataReferenciaRetroativaPorConveniadoQuery();
                var dataReferenciaRetroativa = obterDataReferenciaQuery.Execute(_passagemAprovada.Conveniado.Id.TryToLong());

                dataReferencia = existeFechamento > 0
                    ? dataReferenciaRetroativa
                    : dataReferenciaTransacao;

            }



            return dataReferencia;
        }

        private DateTime CalcularDataDeRepasse()
        {
            switch (RepasseDto.TipoRepasse)
            {
                case TipoRepasse.DiaFixoDoMes:
                    return CalcularDiaFixoDeRepasse();
                case TipoRepasse.QuantidadeDeDiasAposTransacoes:
                    if (DtReferencia != null)
                        return BuscarQuantidadeDeDiasAposTransacoes();
                    return BuscarQuantidadeDeDiasAposTransacoes();
                case TipoRepasse.QuantidadeDeDiasUteisAposVencimento:
                    return BuscarQuantidadeDeDiasUteisAposVencimento(_passagemAprovada.Adesao.Cliente.Id.TryToInt(), _passagemAprovada.TransacaoEstacionamento.DataReferencia);
                case TipoRepasse.QuantidadeDeDiasCorridosAposVencimento:
                    return BuscarQuantidadeDeDiasCorridosAposVencimento(_passagemAprovada.Adesao.Cliente.Id.TryToInt());
            }
            throw new DomainException(TipoDeRepasseInvalido);
        }

    }
}