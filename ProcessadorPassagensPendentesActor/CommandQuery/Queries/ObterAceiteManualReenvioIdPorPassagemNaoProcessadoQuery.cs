using System.Linq;
using Dapper;
using ProcessadorPassagensActors.CommandQuery.Queries.Filter;
using ProcessadorPassagensActors.Infrastructure;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ProcessadorPassagensActors.CommandQuery.Connections;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterAceiteManualReenvioIdPorPassagemNaoProcessadoQuery : IQuery<AceiteManualReenvioPassagemPorPassagemNaoProcessadoFilter, int?>
    {
        public ObterAceiteManualReenvioIdPorPassagemNaoProcessadoQuery()
        {
        }

        public int? Execute(AceiteManualReenvioPassagemPorPassagemNaoProcessadoFilter filter)
        {
            var query = @"SELECT 
                            amrp.Id 
                        FROM dbo.AceiteManualReenvioPassagem amrp	(NOLOCK)
                        INNER JOIN	dbo.Conveniado c (NOLOCK) ON amrp.ConveniadoId = c.ConveniadoId	
                        WHERE c.CodigoProtocoloArtesp = @CodigoProtocoloArtesp 
                        AND amrp.CodigoPassagemConveniado = @CodigoPassagemConveniado 
                        AND amrp.Processado	= 0";

            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var result = conn.Query<int?>(
                sql: query,
                param: new
                {
                    filter.CodigoPassagemConveniado,
                    filter.CodigoProtocoloArtesp
                },
                commandTimeout: TimeHelper.CommandTimeOut).FirstOrDefault();

                return result ?? 0;
            }

            
        }
    }
}
