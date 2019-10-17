using System.Collections.Generic;
using ConectCar.Framework.Infrastructure.Cqrs.Ado.Queries;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using Dapper;
using GeradorPassagensPendentesBatch.CommandQuery.Messages;
using GeradorPassagensPendentesBatch.CommandQuery.Queries.Filters;
using System.Data;

namespace GeradorPassagensPendentesBatch.CommandQuery.Queries
{
    public class ObterPassagensPendentesQuery : DbConnectionQueryBase<ObterPassagensPendentesFilter, IEnumerable<PassagemPendenteMessage>>
    {
        public ObterPassagensPendentesQuery(DbConnectionDataSource dataSource) : base(dataSource)
        {
        }

        public override IEnumerable<PassagemPendenteMessage> Execute(ObterPassagensPendentesFilter filter)
        {
            var resultado = DataSource.Connection.Query<PassagemPendenteMessage>(
                "spObterMensagemPendenteProcessamento",
                new
                {
                    filter.QtdMaximaPassagens,
                },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 600
            );

            foreach (var item in resultado)
            {
                item.DataCriacao =
                    new System.DateTime
                    (year: item.DataCriacao.Year, month: item.DataCriacao.Month, day: item.DataCriacao.Day,
                    hour: item.DataCriacao.Hour, minute: item.DataCriacao.Minute, second: item.DataCriacao.Second, kind: System.DateTimeKind.Utc);
            }

            return resultado;
        }
    }
}
