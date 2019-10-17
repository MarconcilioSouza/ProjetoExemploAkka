using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using Dapper;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterSaldoQuery : IQuery<int, decimal>
    {
        public ObterSaldoQuery()
        {
        }

        public decimal Execute(int saldoId)
        {
            var query = @" 
                            SELECT s.Valor AS Valor 
                              FROM vwSaldo (nolock) AS s 
                             WHERE s.SaldoId = @saldoId
                    ";

            using (var conn = DataBaseConnection.GetConnection(DataBaseSourceType.ConectSys))
            {
                var result = conn.ExecuteScalar<decimal>(sql: query,
                    param: new { saldoId },
                    commandTimeout: TimeHelper.CommandTimeOut);

                return result;
            }


        }
    }
}
