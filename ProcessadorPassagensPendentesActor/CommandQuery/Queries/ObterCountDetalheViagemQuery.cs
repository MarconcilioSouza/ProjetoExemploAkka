using ConectCar.Framework.Infrastructure.Cqrs.Ado.Queries;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using Dapper;
using System.Data;
using System.Linq;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterCountDetalheViagemQuery : DbConnectionQueryBase<long, bool>
    {
        public ObterCountDetalheViagemQuery(DbConnectionDataSource dataSource) : base(dataSource)
        {
        }

        public override bool Execute(long viagemId)
        {
            var transacao = DataSource.Connection.Query<int>(
               "[dbo].[spObterCountDetalheViagem]",
               new
               {
                   ViagemId = viagemId
               },
               commandTimeout: TimeHelper.CommandTimeOut,
               commandType: CommandType.StoredProcedure).FirstOrDefault();

            return transacao > 0;
        }
    }
}
