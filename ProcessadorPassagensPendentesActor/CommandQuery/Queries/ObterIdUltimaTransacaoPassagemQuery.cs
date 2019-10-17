using ConectCar.Framework.Infrastructure.Cqrs.Ado.Queries;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.Model;
using Dapper;
using ProcessadorPassagensActors.CommandQuery.Dtos;
using ProcessadorPassagensActors.CommandQuery.Queries.Filter;
using System.Data;
using System.Linq;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterIdUltimaTransacaoPassagemQuery : IQuery<DetalheTrnFilter, long?>
    {
        public long? Execute(DetalheTrnFilter filter)
        {
            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var result = conn.Query<long>(
                   @"SELECT tp.TransacaoId
                    FROM[TransacaoPassagem] tp
                        INNER JOIN Transacao tp_1_
                        ON tp.TransacaoId = tp_1_.TransacaoId
                    LEFT OUTER JOIN[TransacaoProvisoria] tp_2_ ON tp.TransacaoId = tp_2_.TransacaoPassagemId
                    JOIN(SELECT MAX(TP.transacaoId) transacaoId FROM[TransacaoPassagem] tp
                                    INNER JOIN DetalheTRN dtrn ON tp.DetalheTRNId = dtrn.DetalheTRNId WHERE  dtrn.NumeroTag = @obuId
                                    AND dtrn.NumeroPraca = @numeroPraca
                                    AND dtrn.Data > @dataReferencia) t ON t.transacaoId = tp.TransacaoId",
               new
               {
                   obuId = filter.OBUId,
                   numeroPraca = filter.CodigoPraca,
                   dataReferencia = filter.Data

               },
               commandTimeout: TimeOutHelper.DezMinutos);
                return result.FirstOrDefault();
            }
        }
    }
}
