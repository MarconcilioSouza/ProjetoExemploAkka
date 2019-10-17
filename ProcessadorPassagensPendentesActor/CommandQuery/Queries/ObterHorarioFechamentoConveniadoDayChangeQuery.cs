
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
    public class ObterHorarioFechamentoConveniadoDayChangeQuery : IQuery<long, DateTime?>
    {
        public DateTime? Execute(long conveniadoId)
        {
            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var query = @" SELECT TOP 1 HoraFechamento FROM dbo.ConveniadoDayChange
                            WHERE ConveniadoId = @conveniadoId ORDER BY 1 desc ";

                var result = conn.Query<DateTime?>(
                   sql: query,
                   param: new { conveniadoId },
                   commandTimeout: TimeOutHelper.DezMinutos).FirstOrDefault();

                return result;
            }
        }
    }
}
