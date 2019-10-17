using System.Linq;
using Dapper;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterParceiroNegocioIdPorCodigoQuery : IQuery<string, int>
    {
        public ObterParceiroNegocioIdPorCodigoQuery()
        {
        }

        public int Execute(string codigo)
        {
            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var query = @" 
                            SELECT 
                                p.ParceiroId 
                            FROM dbo.Parceiro  p (NOLOCK)	 
                            WHERE p.CodigoParceiro = @codigo";

                var result = conn.Query<int>
                (query, new
                {
                    codigo
                }).FirstOrDefault();
                return result;
            }

            
        }
    }
}
