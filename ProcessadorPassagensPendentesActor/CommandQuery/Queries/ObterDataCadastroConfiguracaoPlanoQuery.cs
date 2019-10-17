using System.Linq;
using Dapper;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterDiaVencimentoFaturaQuery : IQuery<long, int>
    {
        public ObterDiaVencimentoFaturaQuery()
        {
        }

        public int Execute(long clienteId)
        {
            var query = @"
                            SELECT TOP 1 vf.Dia
	                            FROM dbo.ConfiguracaoPlanoCliente cpc (NOLOCK)	
	                            INNER JOIN dbo.VencimentoFatura vf (NOLOCK) ON cpc.VencimentoFaturaId = vf.VencimentoFaturaId	
                            WHERE cpc.PlanoDePagamentoId		 = 4 AND cpc.ClienteId	= @clienteId";

            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var result = conn.Query<int>(query, 
                    new {
                        clienteId
                    }).FirstOrDefault();
                return result;
            }
            
        }
    }
}
