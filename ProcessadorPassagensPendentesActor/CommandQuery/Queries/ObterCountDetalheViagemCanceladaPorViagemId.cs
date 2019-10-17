using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using Dapper;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterCountDetalheViagemCanceladaPorViagemId : IQuery<int, bool>
    {
        public ObterCountDetalheViagemCanceladaPorViagemId()
        {
        }

        public bool Execute(int viagemId)
        {
            var query = @"SELECT count(*) FROM DetalheViagem dv (NOLOCK) 
                            WHERE dv.ViagemId = @viagemId 
                            AND dv.StatusId = 6";

            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var result = conn.ExecuteScalar<int>(
                    sql: query,
                    param: new
                    {
                        viagemId
                    },
                    commandTimeout: TimeHelper.CommandTimeOut);
                return result > 0;
            }
            
        }
    }
}
