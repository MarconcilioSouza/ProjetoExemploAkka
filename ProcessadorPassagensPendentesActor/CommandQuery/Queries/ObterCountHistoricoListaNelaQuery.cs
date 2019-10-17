using ConectCar.Framework.Infrastructure.Cqrs.Ado.Queries;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.Model;
using Dapper;
using System.Data;
using System.Linq;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterCountHistoricoListaNelaQuery : IQuery<PassagemPendenteEDI, bool>
    {

        public bool Execute(PassagemPendenteEDI filter)
        {
            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var transacao = conn.Query<int>(
                      "[dbo].[spObterCountHistoricoListaNela]",
                      new
                      {
                          tagId = filter.Tag.Id,
                          data = filter.DataPassagem
                      },
                      commandTimeout: TimeHelper.CommandTimeOut,
                      commandType: CommandType.StoredProcedure).FirstOrDefault();

                return transacao > 0; 
            }
        }
    }
}
