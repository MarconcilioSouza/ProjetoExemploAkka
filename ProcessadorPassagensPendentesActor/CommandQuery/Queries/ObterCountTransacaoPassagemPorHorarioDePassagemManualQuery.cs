using Dapper;
using ProcessadorPassagensActors.Infrastructure;
using ProcessadorPassagensActors.CommandQuery.Queries.Filter;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ProcessadorPassagensActors.CommandQuery.Connections;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterCountTransacaoPassagemPorHorarioDePassagemManualQuery
        : IQuery<ObterCountTransacaoPassagemPorHorarioDePassagemManualFilter, bool>
    {
        public ObterCountTransacaoPassagemPorHorarioDePassagemManualQuery()
        {
        }

        public bool Execute(ObterCountTransacaoPassagemPorHorarioDePassagemManualFilter filter)
        {

            var query = @"SELECT count(tp.TransacaoId) FROM dbo.TransacaoPassagem tp (NOLOCK)
                                 INNER JOIN dbo.Passagem p (NOLOCK) ON tp.PassagemId = p.PassagemId	
	                             INNER	JOIN	dbo.Transacao t (NOLOCK) ON tp.TransacaoId = t.TransacaoId	
	                             INNER JOIN	dbo.Adesao a (NOLOCK) ON t.AdesaoId = a.AdesaoId	
	                             INNER JOIN	dbo.Tag t2 (NOLOCK) ON a.TagId = t2.TagId
                        WHERE t2.OBUId	 = @NumeroTag AND 
                              tp.DataDePassagem >= @dtInit AND tp.DataDePassagem <= @dtFim AND 
                              p.Reenvio	>= @Reenvio";

            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var result = conn.ExecuteScalar<int>(query,
                new
                {
                    NumeroTag = filter.PassagemPendenteArtesp.Tag.OBUId,
                    dtInit = filter.PassagemPendenteArtesp.DataPassagem.AddMinutes(-filter.tempoLimite),
                    dtFim = filter.PassagemPendenteArtesp.DataPassagem.AddMinutes(filter.tempoLimite),
                    Reenvio = filter.PassagemPendenteArtesp.NumeroReenvio
                },
                commandTimeout: TimeHelper.CommandTimeOut);

                return result > 0;
            }


        }

    }
}
