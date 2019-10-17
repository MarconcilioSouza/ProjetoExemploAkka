using System;
using System.Linq;
using ConectCar.Framework.Infrastructure.Cqrs.Ado.Queries;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using Dapper;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.CommandQuery.Queries.Filter;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterCountFechamentoPorConveniadoEDataQuery : IQuery<ObterCountFechamentoPorConveniadoEDataFilter, int>
    {

        public int Execute(ObterCountFechamentoPorConveniadoEDataFilter filter)
        {
            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var dataReferencia = (filter.DayChangeAposMeioDia
                      ? filter.DataReferenciaTransacao.Date
                      : filter.DataReferenciaTransacao.Date.AddDays(1));

                var query = @" SELECT COUNT(*) FROM dbo.ConveniadoFechamentoDeDias
                                WHERE ConveniadoId = @conveniadoId  AND
	                                  DataReferencia >= @dataReferencia AND	
	                                  Fechado = 1 ";

                var result = conn.ExecuteScalar<int>(
                   sql: query,
                   param: new
                   {
                       conveniadoId = filter.ConveniadoId,
                       dataReferencia
                   },
                   commandTimeout: TimeOutHelper.DezMinutos);
                return result;
            }
        }
    }
}
