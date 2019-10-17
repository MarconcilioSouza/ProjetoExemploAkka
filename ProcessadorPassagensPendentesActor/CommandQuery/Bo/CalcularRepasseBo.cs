using System;
using System.Linq;
using ConectCar.Framework.Backend.CommonQuery.Query;
using ConectCar.Framework.Domain.Model;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ProcessadorPassagensActors.CommandQuery.Dtos;
using ProcessadorPassagensActors.CommandQuery.Queries;
using ProcessadorPassagensActors.Infrastructure;
using ConectCar.Framework.Infrastructure.Cqrs.Ado.Queries;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Util;
using ProcessadorPassagensActors.CommandQuery.Cache;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.CommandQuery.Queries.Filter;

namespace ProcessadorPassagensActors.CommandQuery.Bo
{
    public class CalcularRepasseBo
    {
        private ObterRepasseQuery _repasseQuery;
        private ObterDataVencimentoUltimaFaturaQuery _dataVencimentoUltimaFaturaQuery;
        private ObterDiaVencimentoFaturaQuery _diaVencimentoFaturaQuery;
        private ObterDataCadastroConfiguracaoPlanoQuery _dataCadastroConfiguracaoPlanoQuery;
        private int _quantidadeDeDiasParaCorte;
        private int _quantidadeMinimaDeDiasDaFatura;
        private int _quantidadeMaximaDeDiasDaFatura;
        private RepasseDto RepasseDto { get; set; }

        private const int Janeiro = 1;
        private const string PeriodoInvalido = "Período inválido para o dia de vencimento selecionado.";

        public CalcularRepasseBo()
        {
            _repasseQuery = new ObterRepasseQuery();
            _dataVencimentoUltimaFaturaQuery = new ObterDataVencimentoUltimaFaturaQuery();
            _diaVencimentoFaturaQuery = new ObterDiaVencimentoFaturaQuery();
            _dataCadastroConfiguracaoPlanoQuery = new ObterDataCadastroConfiguracaoPlanoQuery();

            var configuracaoQuantidadeDiasParaCorteFaturaPosPagoEmpresarialAntesVencimento = ConfiguracaoSistemaCacheRepository.Obter(ConfiguracaoSistemaModel.QuantidadeDiasParaCorteFaturaPosPagoEmpresarialAntesVencimento);
            var configuracaoQuantidadeMinimaDiasFaturaPosPagoEmpresarial = ConfiguracaoSistemaCacheRepository.Obter(ConfiguracaoSistemaModel.QuantidadeMinimaDiasFaturaPosPagoEmpresarial);
            var configuracaoQuantidadeMaximaDiasFaturaPosPagoEmpresarial = ConfiguracaoSistemaCacheRepository.Obter(ConfiguracaoSistemaModel.QuantidadeMaximaDiasFaturaPosPagoEmpresarial);

            _quantidadeDeDiasParaCorte = configuracaoQuantidadeDiasParaCorteFaturaPosPagoEmpresarialAntesVencimento.Valor.TryToInt() * -1;
            _quantidadeMinimaDeDiasDaFatura = configuracaoQuantidadeMinimaDiasFaturaPosPagoEmpresarial.Valor.TryToInt();
            _quantidadeMaximaDeDiasDaFatura = configuracaoQuantidadeMaximaDiasFaturaPosPagoEmpresarial.Valor.TryToInt();
        }

        public void Calular(PassagemAprovadaArtesp passagemAprovadaArtesp)
        {

            RepasseDto = DataBaseConnection.HandleExecution(_repasseQuery.Execute, new ObterRepasseFilter(
                dataPassagem: passagemAprovadaArtesp.DataPassagem,
                conveniadoId: passagemAprovadaArtesp.Conveniado.Id ?? 0,
                pracaId: passagemAprovadaArtesp.Praca.Id ?? 0,
                codigoPista: passagemAprovadaArtesp.Pista.CodigoPista,
                planoId: passagemAprovadaArtesp.Adesao.PlanoId));

            if (RepasseDto == null)
                throw new Exception("ConfiguracaoDeRepasseInvalida");

            CalcularRepasse(passagemAprovadaArtesp);
        }

        private void CalcularRepasse(PassagemAprovadaArtesp passagemAprovadaArtesp)
        {
            passagemAprovadaArtesp.Transacao.TarifaDeInterconexao = RepasseDto.TarifaDeInterconexao.HasValue
                ? (passagemAprovadaArtesp.Transacao.Valor * RepasseDto.TarifaDeInterconexao.Value) / 100
                : 0;

            passagemAprovadaArtesp.Transacao.ValorRepasse =
                passagemAprovadaArtesp.Transacao.Valor - passagemAprovadaArtesp.Transacao.TarifaDeInterconexao;

            passagemAprovadaArtesp.Transacao.RepasseId = RepasseDto.RepasseId;
            passagemAprovadaArtesp.Transacao.TarifaDeInterconexao = RepasseDto.TarifaDeInterconexao ?? 0;

            var clienteId = passagemAprovadaArtesp.Adesao.Cliente.Id ?? 0;
            passagemAprovadaArtesp.Transacao.DataRepasse = ObterDataRepasse(clienteId);
        }

        private DateTime ObterDataRepasse(long clienteId)
        {
            switch (RepasseDto.TipoRepasse)
            {
                case Enums.TipoRepasse.DiaFixoDoMes:
                    return CalcularDiaFixoDeRepasse();
                case Enums.TipoRepasse.QuantidadeDeDiasAposTransacoes:
                    return BuscarQuantidadeDeDiasAposTransacoes();
                case Enums.TipoRepasse.QuantidadeDeDiasUteisAposVencimento:
                    return BuscarQuantidadeDeDiasUteisAposVencimento(clienteId);
                case Enums.TipoRepasse.QuantidadeDeDiasCorridosAposVencimento:
                    return BuscarQuantidadeDeDiasCorridosAposVencimento(clienteId);
            }
            return DateTime.Now;
        }


        private DateTime CalcularDiaFixoDeRepasse()
        {
            var mes = BuscarMesDeRepasse();
            var ano = BuscarAnoDeRepasse();

            try
            {
                var data = ValidarFinalDeSemanaFeriado(new DateTime(ano, mes, RepasseDto.Dias));
                return data;
            }
            catch
            {
                return new DateTime(ano, mes, 1).AddMonths(1).AddDays(-1);
            }
        }
        private DateTime BuscarQuantidadeDeDiasAposTransacoes()
        {
            return ValidarFinalDeSemanaFeriado(DateTimeUtil.Today.AddDays(RepasseDto.Dias));
        }
        private DateTime BuscarQuantidadeDeDiasUteisAposVencimento(long clienteId)
        {
            var dataVencimento = ObterDataVencimento(clienteId);
            var feriados = FeriadoCacheRepository.ListarFeriadosProximos2Anos(dataVencimento);
            return (dataVencimento).AddWorkingDays(RepasseDto.Dias, feriados.Select(x => x.Data).ToList());
        }
        private DateTime BuscarQuantidadeDeDiasCorridosAposVencimento(long clienteId)
        {
            var dataVencimento = ObterDataVencimento(clienteId);
            return dataVencimento.AddDays(RepasseDto.Dias);
        }
        private DateTime ObterDataVencimento(long clienteId)
        {

            var dataVencimento = DataBaseConnection.HandleExecution(_dataVencimentoUltimaFaturaQuery.Execute, clienteId);

            if (dataVencimento == null)
            {
                var diaDeVencimento = DataBaseConnection.HandleExecution(_diaVencimentoFaturaQuery.Execute, clienteId);

                var dataInicioReferencia = ObterDataReferenciaInicial(clienteId);

                var proximoPeriodo = ObterProximoPeriodo(dataInicioReferencia, diaDeVencimento);

                if (!PeriodoValido(null, proximoPeriodo))
                    throw new Exception(PeriodoInvalido);

                dataVencimento = proximoPeriodo.DataVencimento;
            }
            return (DateTime)dataVencimento;
        }
        private PeriodoFaturaDto ObterProximoPeriodo(DateTime dataInicioReferencia, int diaDeVencimento)
        {

            var dataVencimento = new DateTime(dataInicioReferencia.Year, dataInicioReferencia.Month, diaDeVencimento, 00, 00, 00);
            var dataFinalReferencia = dataVencimento.AddDays(_quantidadeDeDiasParaCorte);
            var dataMinimaPermitida = dataInicioReferencia.AddDays(_quantidadeMinimaDeDiasDaFatura);

            while (dataFinalReferencia.Date <= dataMinimaPermitida.Date)
            {
                dataVencimento = dataVencimento.AddMonths(1);
                dataFinalReferencia = dataVencimento.AddDays(_quantidadeDeDiasParaCorte);
            }

            return new PeriodoFaturaDto
            {
                DataReferenciaInicial = new DateTime(dataInicioReferencia.Year, dataInicioReferencia.Month, dataInicioReferencia.Day, 00, 00, 00),
                DataReferenciaFinal = new DateTime(dataFinalReferencia.Year, dataFinalReferencia.Month, dataFinalReferencia.Day, 23, 59, 59),
                DataVencimento = dataVencimento
            };
        }
        private bool PeriodoValido(DateTime? dataVencimentoUltimaFatura, PeriodoFaturaDto periodoFatura)
        {
            var dataMaximaPermitida = periodoFatura.DataReferenciaInicial.AddDays(_quantidadeMaximaDeDiasDaFatura);

            if (dataVencimentoUltimaFatura != null && periodoFatura.DataReferenciaFinal.Date > dataMaximaPermitida.Date)
                return false;

            if (dataVencimentoUltimaFatura != null && periodoFatura.DataVencimento.Month == ((DateTime)dataVencimentoUltimaFatura).Month)
                return false;

            if (dataVencimentoUltimaFatura != null && ((periodoFatura.DataVencimento.Month >= ((DateTime)dataVencimentoUltimaFatura).AddMonths(2).Month &&
                                          periodoFatura.DataVencimento.Year == ((DateTime)dataVencimentoUltimaFatura).AddMonths(2).Year)
                                         || periodoFatura.DataVencimento.Year > ((DateTime)dataVencimentoUltimaFatura).AddMonths(2).Year))

                return false;

            return true;
        }
        private DateTime ObterDataReferenciaInicial(long clienteId)
        {
            var dataReferenciaInicial = DateTime.Now;
            var dataCadastro = DataBaseConnection.HandleExecution(_dataCadastroConfiguracaoPlanoQuery.Execute, clienteId);

            if (dataCadastro != null)
                dataReferenciaInicial = (DateTime)dataCadastro;
            return dataReferenciaInicial;
        }
        private int BuscarMesDeRepasse()
        {
            if (DateTimeUtil.Today.Day >= RepasseDto.Dias && DateTimeUtil.Today.Month == 12)
            {
                return Janeiro;
            }

            if (DateTimeUtil.Today.Day >= RepasseDto.Dias)
            {
                return DateTimeUtil.Today.Month + 1;
            }

            return DateTimeUtil.Today.Month;
        }
        private int BuscarAnoDeRepasse()
        {
            if (DateTimeUtil.Today.Month == 12 && DateTimeUtil.Today.Day >= RepasseDto.Dias)
            {
                return DateTimeUtil.Today.Year + 1;
            }
            return DateTimeUtil.Today.Year;
        }
        private DateTime ValidarFinalDeSemanaFeriado(DateTime data)
        {

            while (data.DayOfWeek == DayOfWeek.Saturday || data.DayOfWeek == DayOfWeek.Sunday || FeriadoCacheRepository.EhFeriado(data))
            {
                data = data.AddDays(1);
            }

            return data;
        }

    }
}
