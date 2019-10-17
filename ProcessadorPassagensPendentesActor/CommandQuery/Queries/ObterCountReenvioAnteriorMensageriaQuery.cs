using System.Linq;
using ConectCar.Transacoes.Domain.ValueObject;
using Dapper;
using ProcessadorPassagensActors.Infrastructure;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ProcessadorPassagensActors.CommandQuery.Connections;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterCountReenvioAnteriorMensageriaQuery : IQuery<PassagemPendenteArtesp, bool>
    {
        public ObterCountReenvioAnteriorMensageriaQuery()
        {
        }

        public bool Execute(PassagemPendenteArtesp filter)
        {
            const string query = @"SELECT TOP 1 p.MensagemItemId AS PassagemAnterior
                                       FROM dbo.Passagem p WITH(NOLOCK)
                                            INNER JOIN MensagemItem mi WITH(NOLOCK)
                                            ON p.MensagemItemId = mi.Id
                                            INNER JOIN Mensagem m WITH(NOLOCK)
                                            ON m.Id = mi.MensagemId
                                      WHERE EXISTS( SELECT 1 FROM PassagemProcessada pp WITH(NOLOCK)
                                                     WHERE pp.PassagemId = p.MensagemItemId )
                                        AND p.ConveniadoPassagemId = @ConveniadoPassagemId
                                        AND m.ConcessionariaId = @ConcessionariaId";

            using (var conn = DataBaseConnection.GetConnection(DataBaseSourceType.Mensageria))
            {
                var result = conn.Query<int>(query,
                    new
                    {
                        filter.ConveniadoPassagemId,
                        ConcessionariaId = filter.Conveniado.CodigoProtocoloArtesp
                    },commandTimeout: TimeHelper.CommandTimeOut)
                    .FirstOrDefault();

                return result > 0;
            }


        }
    }
}
