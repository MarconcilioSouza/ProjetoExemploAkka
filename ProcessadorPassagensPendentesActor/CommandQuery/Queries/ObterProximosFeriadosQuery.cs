
using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using ProcessadorPassagensActors.Infrastructure;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ProcessadorPassagensActors.CommandQuery.Cache;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterProximosFeriadosQuery : IQuery<DateTime, List<DateTime>>
    {
        public ObterProximosFeriadosQuery()
        {
        }

        public List<DateTime> Execute(DateTime data)
        {
            var dataFim = data.AddYears(2);

            //var query = @"
            //                SELECT f.Data FROM dbo.Feriado f (NOLOCK)
            //                WHERE f.Data BETWEEN  @dateInit AND @dateFim";

            //using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            //{
            //    var result = conn.Query<DateTime>(sql: query,
            //    param: new
            //    {
            //        data,
            //        dataFim
            //    },
            //    commandTimeout: TimeHelper.CommandTimeOut);

            //    return result;
            //}

            var feriados = FeriadoCacheRepository.Listar();

            if (feriados.Any())
            {
                var retorno = feriados.Where(c => c.Data >= data && c.Data <= dataFim).Select(c => c.Data).ToList();

                return retorno;
            }
            return null;
        }
    }
}
