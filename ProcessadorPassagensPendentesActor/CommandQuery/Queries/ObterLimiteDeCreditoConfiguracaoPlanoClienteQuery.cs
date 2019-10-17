using ConectCar.Framework.Infrastructure.Cqrs.Ado.Queries;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using Dapper;
using System.Data;
using System.Linq;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterLimiteDeCreditoConfiguracaoPlanoClienteQuery : IQuery<int, decimal>
    {

        public decimal Execute(int planoId)
        {
            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var result = conn.Query<decimal>(
                   "[dbo].[spObterLimiteDeCreditoConfiguracaoPlanoCliente]",
                   new
                   {
                       planoId = planoId

                   },
                   commandTimeout: TimeHelper.CommandTimeOut,
                   commandType: CommandType.StoredProcedure);

                return result.FirstOrDefault(); 
            }
        }
    }
}
