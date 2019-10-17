using System.Linq;
using Dapper;
using ConectCar.Transacoes.Domain.ValueObject;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterCountTransacaoExistenteNoPeriodoQuery : IQuery<PassagemPendenteArtesp, bool>
    {
        public ObterCountTransacaoExistenteNoPeriodoQuery()
        {
        }

        public bool Execute(PassagemPendenteArtesp filter)
        {
            var tempoRetornoPraca = filter.Praca.TempoRetornoPraca;
            var dataLimiteInicial = filter.DataPassagem.AddMinutes(-1 * tempoRetornoPraca);
            var dataLimiteFinal = filter.DataPassagem.AddMinutes(tempoRetornoPraca);

            var query = @"SELECT ( 
                                CASE WHEN EXISTS(
                                    SELECT NULL AS [EMPTY]
                                    FROM [TransacaoPassagem] AS [tp] (nolock)
                                        INNER JOIN [Transacao] AS [t] (nolock) ON [t].[TransacaoId] = [tp].[TransacaoId]
                                        INNER JOIN [Pista] AS [pi] (nolock) ON [pi].[PistaId] = [tp].[PistaId]
                                        INNER JOIN [Praca] AS [pr] (nolock) ON [pr].[PracaId] = [pi].[PracaId]
                                        INNER JOIN [Adesao] AS [a] (nolock) ON [a].[AdesaoId] = [t].[AdesaoId]
                                        LEFT JOIN [Passagem] AS [p] (nolock) ON [p].[PassagemId] = [tp].[PassagemId]
                                    WHERE [pr].[PracaId] = @pracaId
                                      AND [a].TagId = @tagId
                                      AND [tp].[DataDePassagem] BETWEEN @dataLimiteInicial AND @dataLimiteFinal
                                      AND NOT( [pr].ConveniadoId = @conveniadoId 
                                                AND [p].[CodigoPassagemConveniado] = @codigoPassagemConveniado
                                                AND [p].[Reenvio] < @reenvio )
                                ) THEN CAST(1 as bit) 
                                ELSE CAST(0 as bit) 
                                END ) 
                            AS [value]";

            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var result = conn.Query<bool>(query, new
                {
                    pracaId = filter.Praca.Id,
                    tagId = filter.Tag.Id,
                    dataLimiteInicial,
                    dataLimiteFinal,
                    conveniadoId = filter.Conveniado.Id,
                    codigoPassagemConveniado = filter.ConveniadoPassagemId,
                    reenvio = filter.NumeroReenvio
                }).FirstOrDefault();
                return result;
            }


        }
    }
}
