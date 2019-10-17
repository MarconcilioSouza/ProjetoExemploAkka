using ConectCar.Framework.Infrastructure.Cqrs.Ado.Queries;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using Dapper;
using GeradorPassagensPendentesEDIBatch.CommandQuery.Messages;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GeradorPassagensPendentesEDIBatch.CommandQuery.Queries
{
    public class ListarDetalheTrnQuery : DbConnectionQueryBase<ListarDetalheTrnFilter, List<PassagemPendenteEDIMessage>>
    {
        public ListarDetalheTrnQuery(DbConnectionDataSource dataSource) : base(dataSource)
        {
        }

        public override List<PassagemPendenteEDIMessage> Execute(ListarDetalheTrnFilter filter)
        {
            var resultado = DataSource.Connection.Query<PassagemPendenteEDIMessage>(
                "spListarDetalheTrn",
                new
                {
                    filter.QuantidadeMaximaPassagens
                },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 600
            );

            return (resultado ?? new List<PassagemPendenteEDIMessage>()).ToList();
        }
    }
}