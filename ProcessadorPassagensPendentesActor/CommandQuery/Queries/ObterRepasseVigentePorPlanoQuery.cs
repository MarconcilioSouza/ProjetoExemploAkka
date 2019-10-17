using System.Data;
using System.Linq;
using ConectCar.Framework.Infrastructure.Cqrs.Ado.Queries;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.ValueObject;
using Dapper;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.CommandQuery.Dtos;
using ProcessadorPassagensActors.CommandQuery.Queries.Filter;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterRepasseVigentePorPlanoQuery : IQuery<ObterRepasseFilter, RepasseDto>
    {
       public RepasseDto Execute(ObterRepasseFilter filter)
        {
            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var query = @"SELECT TOP 1 r.*, p.PistaId FROM dbo.Pista p (NOLOCK)	
		                INNER JOIN	dbo.Praca p2 (NOLOCK) ON p.PracaId = p2.PracaId	
		                INNER JOIN	dbo.Conveniado c (NOLOCK) ON p2.ConveniadoId = c.ConveniadoId	
		                INNER JOIN	dbo.Repasse r (NOLOCK) ON c.ConveniadoId = r.ConveniadoId
	                WHERE		
	                r.PlanoId	= @PlanoId AND 
	                (r.VigenciaInicio IS NULL OR r.VigenciaInicio < @DataDePassagem)
	                ORDER BY r.VigenciaInicio";

                var repasse = conn.Query<RepasseDto>(
                    query,
                    new
                    {
                        filter.PlanoId,
                        DataDePassagem = filter.DataPassagem
                    },
                    commandTimeout: TimeOutHelper.DezMinutos).FirstOrDefault();


                if (repasse == null || repasse.RepasseId == 0)
                {
                    query = @"SELECT TOP 1 r.*, p.PistaId FROM dbo.Pista p (NOLOCK)	
		                INNER JOIN	dbo.Praca p2 (NOLOCK) ON p.PracaId = p2.PracaId	
		                INNER JOIN	dbo.Conveniado c (NOLOCK) ON p2.ConveniadoId = c.ConveniadoId	
		                INNER JOIN	dbo.Repasse r (NOLOCK) ON c.ConveniadoId = r.ConveniadoId
	                WHERE		
	                r.PlanoId	IS NULL AND 
	                (r.VigenciaInicio IS NULL OR r.VigenciaInicio < @DataDePassagem)
	                ORDER BY r.VigenciaInicio";

                    repasse = conn.Query<RepasseDto>(
                        query,
                        new
                        {
                            DataDePassagem = filter.DataPassagem
                        },
                        commandTimeout: TimeOutHelper.DezMinutos).FirstOrDefault();
                    
                }
                return repasse; 
            }
        }
    }
}
