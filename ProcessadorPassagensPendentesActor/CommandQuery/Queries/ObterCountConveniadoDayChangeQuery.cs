using System;
using System.Linq;
using ConectCar.Framework.Infrastructure.Cqrs.Ado.Queries;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using Dapper;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterCountConveniadoDayChangeQuery : IQuery<long, int>
    {
        /// <summary>
        /// Obter total de ConveniadoDayChange por conveniadoId
        /// </summary>
        /// <param name="conveniadoId">conveniadoId</param>
        /// <returns></returns>
        public int Execute(long conveniadoId)
        {
            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var query = @" SELECT Count(*) FROM dbo.ConveniadoDayChange
                            WHERE ConveniadoId = @conveniadoId ORDER BY 1 desc
                    ";

                var result = conn.ExecuteScalar<int>(
                   sql: query,
                   param: new { conveniadoId },
                   commandTimeout: TimeOutHelper.DezMinutos);

                return result; 
            }
        }
    }
}
