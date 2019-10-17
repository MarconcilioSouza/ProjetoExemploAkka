using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ConectCar.Framework.Infrastructure.Cqrs.Ado.Queries;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.Model;
using ConectCar.Transacoes.Domain.ValueObject;
using Dapper;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterCountTransacaoPassagemRepetida : IQuery<PassagemPendenteArtesp, bool>
    {
        public ObterCountTransacaoPassagemRepetida()
        {
        }

        public bool Execute(PassagemPendenteArtesp filter)
        {
            var query = @"SELECT count(t.TransacaoId) FROM dbo.TransacaoPassagem tp	
		                        INNER JOIN	dbo.Transacao t ON tp.TransacaoId = t.TransacaoId	
		                        INNER JOIN	dbo.Pista p ON tp.PistaId = p.PistaId	
		                        INNER JOIN	dbo.Praca p2 ON p.PracaId = p2.PracaId	
		                        INNER JOIN	dbo.Adesao a ON t.AdesaoId = a.AdesaoId	
		                        INNER JOIN	dbo.Tag t2 ON a.TagId = t2.TagId	
		                        INNER JOIN	dbo.Passagem p3 ON tp.PassagemId = p3.PassagemId
                                
	                        WHERE	
	                        p.CodigoPista	= @NumeroPista AND
	                        p2.CodigoPraca	= @NumeroPraca AND	
	                        t2.OBUId	= @NumeroTag and
	                        p3.ConveniadoId	= @ConveniadoId and 
                            tp.DataDePassagem = @DataDePassagem";

            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var result = conn.ExecuteScalar<int>(
                sql: query,
                param: new
                {
                    NumeroPista = filter.Pista.CodigoPista,
                    NumeroPraca = filter.Praca.CodigoPraca,
                    NumeroTag = filter.Tag.OBUId,
                    ConveniadoId = filter.Conveniado.Id,
                    DataDePassagem = filter.DataPassagem
                },
                commandTimeout: TimeHelper.CommandTimeOut);

                return result > 0;
            }

            

        }
    }
}
