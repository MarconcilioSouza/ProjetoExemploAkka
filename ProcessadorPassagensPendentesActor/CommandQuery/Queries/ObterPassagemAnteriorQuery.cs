using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ConectCar.Transacoes.Domain.ValueObject;
using Dapper;
using ProcessadorPassagensActors.Infrastructure;
using System.Linq;
using ProcessadorPassagensActors.CommandQuery.Connections;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterPassagemAnteriorQuery : IQuery<PassagemPendenteArtesp, PassagemPendenteArtesp>
    {
        public ObterPassagemAnteriorQuery()
        {
        }

        public PassagemPendenteArtesp Execute(PassagemPendenteArtesp filter)
        {
            if (filter.NumeroReenvio == 0)
                return null;

            var query = @"
                SELECT TOP 1
                        p.*
                FROM    dbo.Passagem p (NOLOCK)
                        INNER JOIN dbo.Conveniado c (NOLOCK) ON c.ConveniadoId = p.ConveniadoId
                        LEFT JOIN dbo.TransacaoRecusada tr (NOLOCK) ON tr.PassagemId = p.PassagemId
                WHERE   c.ConveniadoId = @Id
                        AND p.Reenvio < @NumeroReenvio
                        AND p.CodigoPassagemConveniado = @ConveniadoPassagemId
                        AND tr.TransacaoRecusadaId IS NULL
                ORDER BY p.PassagemId DESC

            ";

            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var ret = conn.Query<PassagemPendenteArtesp>(sql: query,
                param: new
                {
                    filter.Conveniado.Id,
                    filter.NumeroReenvio,
                    filter.ConveniadoPassagemId
                },
                commandTimeout: TimeHelper.CommandTimeOut);

                return ret.FirstOrDefault();
            }

            
        }
    }
}
