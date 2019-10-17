using System.Collections.Generic;
using Dapper;
using ProcessadorPassagensActors.CommandQuery.Dtos;
using ProcessadorPassagensActors.Infrastructure;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ProcessadorPassagensActors.CommandQuery.Connections;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterFeriadosQuery : IQuery<IEnumerable<FeriadoDto>>
    {
        public ObterFeriadosQuery()
        {
        }

        public IEnumerable<FeriadoDto> Execute()
        {
            
            var query = @" SELECT * FROM dbo.Feriado f (NOLOCK)";

            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var result = conn.Query<FeriadoDto>(sql: query,
                commandTimeout: TimeHelper.CommandTimeOut);

                return result;
            }
            
        }
    }
}
