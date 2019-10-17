using Dapper;
using ProcessadorPassagensActors.Infrastructure;
using System.Linq;
using ConectCar.Transacoes.Domain.ValueObject;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ProcessadorPassagensActors.CommandQuery.Connections;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterPassagemPorCodigoPassagemConveniadoQuery : IQuery<PassagemPendenteArtesp, PassagemPendenteArtesp>
    {
        public ObterPassagemPorCodigoPassagemConveniadoQuery()
        {
        }

        public PassagemPendenteArtesp Execute(PassagemPendenteArtesp filter)
        {
            const string query = @"
                            SELECT TOP 1 p.PassagemId Id, p.Reenvio NumeroReenvio
                FROM    dbo.Passagem p (NOLOCK)
                        INNER JOIN dbo.TransacaoPassagem tp (NOLOCK) ON tp.PassagemId = p.PassagemId
                WHERE   p.ConveniadoId = @ConveniadoId
                        AND p.CodigoPassagemConveniado = @ConveniadoPassagemId
                ORDER BY p.PassagemId DESC";

            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var ret = conn.Query<PassagemPendenteArtesp>(sql: query,
                param: new
                {
                    ConveniadoId = filter.Conveniado.Id,
                    ConveniadoPassagemId = filter.ConveniadoPassagemId,
                },
                commandTimeout: TimeHelper.CommandTimeOut);

                return ret.FirstOrDefault();
            }

            
        }
    }
}
