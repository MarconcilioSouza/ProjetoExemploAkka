using ConectCar.Framework.Infrastructure.Cqrs.Ado.Queries;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using Dapper;
using System.Data;
using System.Linq;
using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterPlacaPorObuidEDataPassagemQuery : DbConnectionQueryBase<PassagemPendenteArtesp, string>
    {
        public ObterPlacaPorObuidEDataPassagemQuery(DbConnectionDataSource dataSource) : base(dataSource)
        {
        }

        public override string Execute(PassagemPendenteArtesp filter)
        {
            var result = DataSource.Connection.Query<string>(
              "[dbo].[spObterPlacaPorOBUIdEDataPassagem]",
              new
              {
                  obuId = filter.Tag.OBUId,
                  dataPassagem = filter.DataPassagem

              },
              commandType: CommandType.StoredProcedure).FirstOrDefault();

            return result;
        }
    }
}
