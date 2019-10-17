using System.Linq;
using Dapper;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterPassagemIdPorMensagemItemIdQuery : IQuery<long, long>
    {
        public ObterPassagemIdPorMensagemItemIdQuery()
        {

        }

        public long Execute(long mensagemItemId)
        {
            var query = @"
                        SELECT 
                            p.PassagemId 
                        FROM Passagem p (NOLOCK)
                        WHERE p.MensagemItemId = @mensagemItemId";
            
            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var result = conn.Query<long?>(query, new{
                    mensagemItemId
                },commandTimeout: TimeHelper.CacheExpiration).FirstOrDefault();

                return result ?? 0;            
            }
        }
    }
}
