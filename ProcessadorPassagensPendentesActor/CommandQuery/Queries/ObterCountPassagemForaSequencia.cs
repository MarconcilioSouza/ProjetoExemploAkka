using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ConectCar.Transacoes.Domain.ValueObject;
using Dapper;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterCountPassagemForaSequencia : IQuery<PassagemPendenteArtesp, bool>
    {
        public ObterCountPassagemForaSequencia()
        {
        }

        public bool Execute(PassagemPendenteArtesp filter)
        {
            var query = @"
                        SELECT p.MensagemItemId AS PassagemAnterior
                        
                        FROM dbo.Passagem p WITH(NOLOCK)
                            INNER JOIN MensagemItem mi WITH(NOLOCK) ON p.MensagemItemId = mi.Id
                            INNER JOIN Mensagem m WITH(NOLOCK)      ON m.Id = mi.MensagemId
        
                        WHERE EXISTS( SELECT 1 FROM PassagemProcessada pp WITH(NOLOCK)
                                      WHERE pp.PassagemId = p.MensagemItemId )
                        AND p.ConveniadoPassagemId = @ConveniadoPassagemId
                        AND m.ConcessionariaId = @ConcessionariaId
                        AND p.NumeroReenvio >= @NumeroReenvio";

            using (var conn = DataBaseConnection.GetConnection(DataBaseSourceType.Mensageria))
            {
                var result = conn.ExecuteScalar<int>(
                sql: query,
                param: new
                {
                    ConveniadoPassagemId = filter.ConveniadoPassagemId,
                    ConcessionariaId = filter.Conveniado.CodigoProtocoloArtesp,
                    NumeroReenvio = filter.NumeroReenvio
                },
                commandTimeout: TimeHelper.CommandTimeOut);

                return result > 0;
            }

            
        }
    }
}
