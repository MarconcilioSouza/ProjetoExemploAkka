using ProcessadorPassagensActors.CommandQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ProcessadorPassagensActors.CommandQuery.Dtos;
using ProcessadorPassagensActors.CommandQuery.Queries;
using ProcessadorPassagensActors.CommandQuery.Util;
using ProcessadorPassagensActors.Infrastructure;
using ConectCar.Framework.Infrastructure.Cqrs.Ado.Queries;
using ProcessadorPassagensActors.CommandQuery.Cache;

namespace ProcessadorPassagensActors.CommandQuery.Bo
{
    public abstract class RepasseBase
    {
        public const int Janeiro = 1;
        public const string PeriodoInvalido = "Período inválido para o dia de vencimento selecionado.";
        public const string TipoDeRepasseInvalido = "O Tipo de Repasse é inválido.";
        public RepasseDto RepasseDto { get; set; }
        public DbConnectionDataSource DataSourceConectSysReadOnly { get; set; }
        public DbConnectionDataSource DataSourceFallBack { get; set; }

        public DateTime BuscarQuantidadeDeDiasAposTransacoes()
        {
            return ValidarFinalDeSemanaFeriado(DateTimeUtil.Today.AddDays(RepasseDto.Dias));
        }

        public DateTime CalcularDiaFixoDeRepasse()
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

        public DateTime ValidarFinalDeSemanaFeriado(DateTime data)
        {
            var feriados = FeriadoCacheRepository.Listar();

            while (data.DayOfWeek == DayOfWeek.Saturday || data.DayOfWeek == DayOfWeek.Sunday || feriados.Any(x => x.Data.Date == data.Date))
            {
                data = data.AddDays(1);
            }

            return data;
        }

        public DateTime BuscarQuantidadeDeDiasUteisAposVencimento(int clienteId, DateTime data)
        {
            var obterDataVencimentoUltimaFaturaQueryQuery = new ObterDataVencimentoUltimaFaturaQuery();
            var obterProximosFeriadosQuery = new ObterProximosFeriadosQuery();

            var dataVencimentoUltimaFatura = obterDataVencimentoUltimaFaturaQueryQuery.Execute(clienteId);
            var proximosFeriados = obterProximosFeriadosQuery.Execute(data).ToList();

            return (dataVencimentoUltimaFatura ?? DateTime.Now).AddWorkingDays(RepasseDto.Dias, proximosFeriados);
        }

        public DateTime BuscarQuantidadeDeDiasCorridosAposVencimento(int clienteId)
        {
            var obterDataVencimentoUltimaFaturaQuery = new ObterDataVencimentoUltimaFaturaQuery();
            var dataVencimentoUltimaFatura = obterDataVencimentoUltimaFaturaQuery.Execute(clienteId);
            return (dataVencimentoUltimaFatura ?? DateTime.Now).AddDays(RepasseDto.Dias);
        }
    }
}
