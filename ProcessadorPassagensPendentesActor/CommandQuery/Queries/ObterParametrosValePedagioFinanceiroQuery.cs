using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using Dapper;
using ProcessadorPassagensActors.Infrastructure;
using System.Data;
using System.Linq;
using ProcessadorPassagensActors.CommandQuery.Connections;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterParametrosValePedagioFinanceiroQuery : IQuery<int>
    {
        public int Execute()
        {
            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.LagSeconds))
            {
                var result = conn.Query<int>(
                    "SELECT TOP 1 NumeroVezesRecusado FROM ParametrosValePedagioFinanceiro (nolock)",
                    commandTimeout: TimeHelper.CommandTimeOut,
                    commandType: CommandType.StoredProcedure).FirstOrDefault();

                return result;
            }

        }
    }
}
