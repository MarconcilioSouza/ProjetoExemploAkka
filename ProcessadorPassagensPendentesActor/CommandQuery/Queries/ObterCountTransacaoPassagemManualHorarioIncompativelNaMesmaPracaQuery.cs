using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using Dapper;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.CommandQuery.Queries.Filter;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterCountTransacaoPassagemManualHorarioIncompativelNaMesmaPracaQuery
        : IQuery<ObterCountTransacaoPassagemPorHorarioDePassagemManualFilter, bool>
    {
        public ObterCountTransacaoPassagemManualHorarioIncompativelNaMesmaPracaQuery()
        {
        }

        public bool Execute(ObterCountTransacaoPassagemPorHorarioDePassagemManualFilter filter)
        {

            var query = @"SELECT count(tp.TransacaoId) FROM dbo.TransacaoPassagem tp (NOLOCK)
                                 INNER JOIN dbo.Passagem p (NOLOCK) ON tp.PassagemId = p.PassagemId	
	                             INNER	JOIN	dbo.Transacao t (NOLOCK) ON tp.TransacaoId = t.TransacaoId	
	                             INNER JOIN	dbo.Adesao a (NOLOCK) ON t.AdesaoId = a.AdesaoId	
	                             INNER JOIN	dbo.Tag t2 (NOLOCK) ON a.TagId = t2.TagId
                                 INNER JOIN dbo.Pista p2 (NOLOCK) ON tp.PistaId = p2.PistaId	
	                             INNER JOIN dbo.Praca p3 (NOLOCK) ON p2.PracaId = p3.PracaId	
                        WHERE t2.OBUId	 = @NumeroTag AND 
                              tp.DataDePassagem >= @dtInit AND tp.DataDePassagem <= @dtFim AND 
                              p.Reenvio	>= @Reenvio AND
                              p3.CodigoPraca	= @CodigoPraca";

            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var result = conn.ExecuteScalar<int>
                (query,
                    new
                    {
                        NumeroTag = filter.PassagemPendenteArtesp.Tag.OBUId,
                        dtInit = filter.PassagemPendenteArtesp.DataPassagem.AddMinutes(-filter.tempoLimite),
                        dtFim = filter.PassagemPendenteArtesp.DataPassagem.AddMinutes(filter.tempoLimite),
                        Reenvio = filter.PassagemPendenteArtesp.NumeroReenvio,
                        CodigoPraca = filter.PassagemPendenteArtesp.Praca.CodigoPraca
                    },
                    commandTimeout: TimeHelper.CommandTimeOut);

                return result > 0;
            }

                
        }

    }
}
