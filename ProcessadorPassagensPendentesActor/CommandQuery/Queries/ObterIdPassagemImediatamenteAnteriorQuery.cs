using Dapper;
using ProcessadorPassagensActors.Infrastructure;
using System.Linq;
using ConectCar.Transacoes.Domain.ValueObject;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ProcessadorPassagensActors.CommandQuery.Connections;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterIdPassagemImediatamenteAnteriorQuery : IQuery<PassagemPendenteArtesp, long?>
    {
        public ObterIdPassagemImediatamenteAnteriorQuery()
        {
        }

        public long? Execute(PassagemPendenteArtesp filter)
        {
            if (filter.NumeroReenvio == 0) return null;

            const string query = @"
                            SELECT  p.PassagemId
                            FROM    dbo.Passagem p (NOLOCK)       
                                    LEFT JOIN dbo.TransacaoRecusada tr (NOLOCK) ON tr.PassagemId = p.PassagemId
                            WHERE   p.ConveniadoId = @ConveniadoId
                                    AND p.Reenvio < @Reenvio
                                    AND p.CodigoPassagemConveniado = @CodigoPassagemConveniado
                                    AND tr.TransacaoRecusadaId IS NULL
                            UNION 
                            SELECT   p.PassagemId
                            FROM    dbo.Passagem p (NOLOCK)       
                                    INNER JOIN dbo.TransacaoRecusada tr (NOLOCK) ON tr.PassagemId = p.PassagemId
                            WHERE   p.ConveniadoId = @ConveniadoId
                                    AND p.Reenvio < @Reenvio
                                    AND p.CodigoPassagemConveniado = @CodigoPassagemConveniado        
                            ORDER BY p.PassagemId DESC";

            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var ret = conn.Query<long?>(sql: query,
                param: new
                {
                    ConveniadoId = filter.Conveniado.Id,
                    Reenvio = filter.NumeroReenvio,
                    CodigoPassagemConveniado = filter.ConveniadoPassagemId,
                },
                commandTimeout: TimeHelper.CommandTimeOut);

                return ret.FirstOrDefault();
            }

            
        }
    }
}
