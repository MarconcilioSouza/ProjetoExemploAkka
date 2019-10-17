using System;
using System.Data;
using System.Linq;
using ConectCar.Framework.Infrastructure.Cqrs.Ado.Queries;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.Model;
using Dapper;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.CommandQuery.Dtos;
using ProcessadorPassagensActors.CommandQuery.Queries.Filter;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterSePossuiIncidentesQuery : IQuery<ObterSePossuiIncidentesFilter, bool>
    {
        public bool Execute(ObterSePossuiIncidentesFilter filter)
        {
            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var historico = conn.Query<IncidenteDto>(
                        @"SELECT i.Ativo, i.VigenciaFim FROM dbo.Incidente i
	                    WHERE i.AjustaSLA	= @AjustaSLA
	                    AND (i.PracaId	IS NULL OR i.PracaId	= @PracaId)
	                    AND (i.PistaId	IS NULL OR i.PistaId	= @PistaId)	                    
	                    AND (i.OrigemIncidenteId	= @OrigemIncidenteId OR (SELECT count(*) FROM dbo.IncidenteConveniado ic WHERE ic.IncidenteId	= i.IncidenteId	AND ic.ConveniadoId	= 1) > 0)",
                        new
                        {
                            filter.PracaId,
                            filter.PistaId,
                            OrigemIncidenteId = 1, // OrigemIncidente.Conectcar
                },
                        commandTimeout: TimeOutHelper.DezMinutos).ToList();


                var retorno = historico.Where(x => filter.DataPassagem.Between(x.VigenciaInicio.Value,
                        Convert.ToDateTime($"{x.VigenciaFim.Value:dd/MM/yyyy HH:mm:59}"))).ToList()
                    .Any(c => c.EstaAtivo());

                return retorno; 
            }
        }
    }
}
