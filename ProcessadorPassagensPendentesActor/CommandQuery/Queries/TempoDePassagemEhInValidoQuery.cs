using System;
using ConectCar.Framework.Infrastructure.Cqrs.Ado.Queries;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.Model;
using Dapper;
using System.Data;
using System.Linq;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class TempoDePassagemEhInValidoQuery : DbConnectionQueryBase<PassagemPendenteEDI, bool>
    {
        public TempoDePassagemEhInValidoQuery(DbConnectionDataSource dataSource) : base(dataSource)
        {
        }

        public override bool Execute(PassagemPendenteEDI filter)
        {
            var result = DataSource.Connection.Query<int>(
              "[dbo].[spOTempoDePassagemEhInValido]",
              new
              {
                  obuId = filter.Tag.OBUId,
                  numeroPraca = filter.Praca,
                  dataReferencia = filter.DataPassagem,
                  tempoComparacao = filter.Praca.TempoRetornoPraca * 60
              },
              commandTimeout: TimeHelper.CommandTimeOut,
              commandType: CommandType.StoredProcedure).FirstOrDefault();

            return result > 0;
        }
    }
}
