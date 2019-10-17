using ConectCar.Framework.Infrastructure.Cqrs.Ado.Queries;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.Model;
using ConectCar.Transacoes.Domain.ValueObject;
using Dapper;
using ProcessadorPassagensActors.Infrastructure;
using System;
using System.Linq;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class CriarPassagemAprovadaCompletaParkQuery : DbConnectionQueryBase<PassagemAprovadaEstacionamento, PassagemAprovadaEstacionamento>
    {
        private readonly DbConnectionDataSource _dataSourceConectSysReadOnly;
        private readonly DbConnectionDataSource _dataSourceFallBack;

        public CriarPassagemAprovadaCompletaParkQuery(DbConnectionDataSource dataSourceConectSysReadOnly, DbConnectionDataSource dataSourceFallBack)
            : base(dataSourceConectSysReadOnly)
        {
            _dataSourceConectSysReadOnly = dataSourceConectSysReadOnly;
            _dataSourceFallBack = dataSourceFallBack;
        }

        public override PassagemAprovadaEstacionamento Execute(PassagemAprovadaEstacionamento paEstacionamento)
        {
            paEstacionamento.ConveniadoInformacoesRPS = ObterConveniadoInformacoesRPS(paEstacionamento.Conveniado?.Id ?? 0);

            long numeroRps = 0;
            string serieRps = string.Empty;
            if (paEstacionamento.ConveniadoInformacoesRPS != null)
            {
                var qtdConveniadoDayChanges = ObterConveniadoDayChanges(paEstacionamento.Conveniado?.Id ?? 0);
                
                if (paEstacionamento.ConveniadoInformacoesRPS.TipoRps == TipoRps.PorPista)
                {
                    paEstacionamento.PistaInformacoesRPS = ObterPistaInformacoesRPS(paEstacionamento.Pista?.Id ?? 0);

                    serieRps = paEstacionamento.PistaInformacoesRPS.SerieRPS;
                    if (qtdConveniadoDayChanges == 0)
                    {
                        numeroRps = ++paEstacionamento.PistaInformacoesRPS.NumeroRPS;                        
                    }
                }
                else
                {
                    serieRps = paEstacionamento.ConveniadoInformacoesRPS.SerieRPS;
                    if (qtdConveniadoDayChanges == 0)
                    {
                        numeroRps = ++paEstacionamento.ConveniadoInformacoesRPS.NumeroRPS;
                    }
                }
            }

            paEstacionamento.TransacaoEstacionamento.NumeroRPS = numeroRps;
            paEstacionamento.TransacaoEstacionamento.SerieRPS = serieRps;
            return paEstacionamento;
        }

        private ConveniadoInformacoesRps ObterConveniadoInformacoesRPS(long conveniadoId)
        {
            return DataSource.Connection.Query<ConveniadoInformacoesRps>(
                @"  SELECT
                        ciRps.ConveniadoInformacoesRPSId AS [Id],
				        ciRps.SerieRPS AS [SerieRPS],
				        ciRps.NumeroRPS AS [NumeroRPS],
				        ciRps.TipoRpsId AS [TipoRps]
			        FROM ConveniadoInformacoesRPS ciRps
			        WHERE ConveniadoId = @ConveniadoId",
                new { ConveniadoId = conveniadoId }).FirstOrDefault();
        }

        private PistaInformacoesRps ObterPistaInformacoesRPS(long pistaId)
        {
            var ret = DataSource.Connection.Query<PistaInformacoesRps>(
                   @"   SELECT
	                        piRps.PistaInformacoesRPSId AS [Id],
	                        piRps.ConveniadoInformacoesRPSId AS [ConveniadoInformacoesRPSId],
	                        piRps.SerieRPS AS [SerieRPS],
	                        piRps.NumeroRPS AS [NumeroRPS],
	                        piRps.PistaId AS [PistaId],
	                        piRps.DataCriacao AS [DataCriacao]
                        FROM PistaInformacoesRPS piRps
						WHERE piRps.PistaId = @PistaId",
                   new { PistaId = pistaId }).FirstOrDefault();

            if (ret == null)
            {
                ret = new PistaInformacoesRps
                {
                    PistaId = pistaId.TryToInt(),
                    SerieRPS = string.Empty,
                    NumeroRPS = 0,
                    DataCriacao = DateTime.Now
                };
            }

            return ret;
        }

        private int ObterConveniadoDayChanges(long conveniadoId)
        {
            var qry = new ObterCountConveniadoDayChangeQuery();
            return qry.Execute(conveniadoId);
        }
    }
}