using ConectCar.Framework.Infrastructure.Cqrs.Ado.Queries;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.Model;
using Dapper;
using System.Data;
using System.Linq;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ConectCar.Transacoes.Domain.Enum;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterCountDetalheTrfRecusadoPorDetalhesDaPassagemQuery : IQuery<PassagemPendenteEDI, int>
    {
        public int Execute(PassagemPendenteEDI filter)
        {
            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var count = conn.Query<int>(
                       @"SELECT count(*) FROM dbo.DetalheTRFRecusado dt	 (NOLOCK)
                    INNER JOIN dbo.DetalheTRN dt2
                    WHERE   dt2.NumeroTag = @NumeroTag
                        and dt2.PlacaTag = @PlacaTag
                        AND dt2.NumeroPraca = @NumeroPraca
                        AND dt2.Data = @Data
                        AND dt.CodigoRetornoId = @CodigoRetornoId",
                       new
                       {
                           NumeroTag = filter.Tag.OBUId,
                           PlacaTag = filter.Placa,
                           NumeroPraca = filter.Praca.CodigoPraca,
                           Data = filter.DataPassagem,
                           CodigoRetornoId = CodigoRetornoTransacaoTRF.ValorNaoCorrespondenteCAT
                       },
                       commandType: CommandType.StoredProcedure).FirstOrDefault();

                return count; 
            }
        }
    }
}
