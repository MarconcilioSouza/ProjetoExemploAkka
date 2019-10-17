using ConectCar.Framework.Infrastructure.Cqrs.Ado.Queries;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterListaTransacaoPassagemQuery : DbConnectionQueryBase<PassagemPendenteArtesp, List<TransacaoPassagem>>
    {
        public ObterListaTransacaoPassagemQuery(DbConnectionDataSource dataSource) : base(dataSource) { }

        public override List<TransacaoPassagem> Execute(PassagemPendenteArtesp filter)
        {
            var resultado = DataSource.Connection.Query<TransacaoPassagem>(
               "[dbo].[spObterListaTransacaoPassagem]",
               new
               {
                   codigoPista = filter.Pista.CodigoPista,
                   numeroPraca = filter.Praca.CodigoPraca,
                   numeroTag = filter.Tag.OBUId,
                   conveniadoId = filter.Conveniado.Id
               },
               commandTimeout: TimeHelper.CommandTimeOut,
               commandType: CommandType.StoredProcedure);

            if (filter.DataPassagem != DateTime.MinValue)
                resultado = resultado.Where(x => x.DataDePassagem == filter.DataPassagem);

            return resultado.ToList();
        }
    }
}
