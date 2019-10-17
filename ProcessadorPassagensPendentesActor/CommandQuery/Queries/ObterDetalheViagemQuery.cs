using ConectCar.Framework.Infrastructure.Cqrs.Ado.Queries;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.Model;
using Dapper;
using ProcessadorPassagensActors.CommandQuery.Queries.Filter;
using System.Data;
using System.Linq;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterDetalheViagemQuery : DbConnectionQueryBase<ObterDetalheViagemFilter, DetalheViagem>
    {
        public ObterDetalheViagemQuery(DbConnectionDataSource dataSource) : base(dataSource)
        {
        }

        public override DetalheViagem Execute(ObterDetalheViagemFilter filter)
        {
            var transacao = DataSource.Connection.Query<DetalheViagem>(
               "[dbo].[spObterDetalheViagem]",
               new
               {
                   ViagemId = filter.ViagemId,
                   Sequencia = filter.Sequencia

               },
               commandTimeout: TimeHelper.CommandTimeOut,
               commandType: CommandType.StoredProcedure).FirstOrDefault();

            return transacao;
        }
    }
}
