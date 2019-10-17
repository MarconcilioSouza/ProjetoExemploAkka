using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.ValueObject;
using Dapper;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterCountPassagemProcessadaCompensadaQuery : IQuery<PassagemPendenteArtesp, bool>
    {
        public ObterCountPassagemProcessadaCompensadaQuery()
        {
        }

        public bool Execute(PassagemPendenteArtesp filter)
        {
            const string query = @"SELECT Count(pp.Id)
                                   FROM dbo.Passagem p WITH(NOLOCK)
                                        INNER JOIN MensagemItem mi WITH(NOLOCK)
                                        ON p.MensagemItemId = mi.Id
                                        INNER JOIN Mensagem m WITH(NOLOCK)
                                        ON m.Id = mi.MensagemId
                                        inner join PassagemProcessada pp WITH(NOLOCK)
                                        on pp.PassagemId = p.MensagemItemId
                            WHERE p.ConveniadoPassagemId = @ConveniadoPassagemId AND m.ConcessionariaId = @ConcessionariaId
                            AND pp.ResultadoId = @ResultadoId 
                                ";


            using (var conn = DataBaseConnection.GetConnection(DataBaseSourceType.Mensageria))
            {
                var result = conn.ExecuteScalar<int>
                (query,
                    new
                    {
                        filter.ConveniadoPassagemId,
                        ConcessionariaId = filter.Conveniado.CodigoProtocoloArtesp,
                        ResultadoId = ResultadoPassagem.Compensado

                    },
                    commandTimeout: TimeHelper.CommandTimeOut);

                return result > 0;
            }                
        }
    }
}
