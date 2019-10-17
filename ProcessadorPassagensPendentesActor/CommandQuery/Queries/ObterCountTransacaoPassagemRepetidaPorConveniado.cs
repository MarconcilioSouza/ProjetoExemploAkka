using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ConectCar.Transacoes.Domain.ValueObject;
using Dapper;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterCountTransacaoPassagemRepetidaPorConveniado : IQuery<PassagemPendenteArtesp, bool>
    {
        public ObterCountTransacaoPassagemRepetidaPorConveniado()
        {
        }

        public bool Execute(PassagemPendenteArtesp filter)
        {
            var query = @"SELECT count(t.TransacaoId) 
                            FROM dbo.TransacaoPassagem tp (nolock)	
		                    INNER JOIN	dbo.Transacao t (nolock) ON tp.TransacaoId = t.TransacaoId	
		                    INNER JOIN	dbo.Pista p (nolock) ON tp.PistaId = p.PistaId	
		                    INNER JOIN	dbo.Praca p2 (nolock) ON p.PracaId = p2.PracaId	
		                    INNER JOIN	dbo.Adesao a (nolock) ON t.AdesaoId = a.AdesaoId	
		                    INNER JOIN	dbo.Tag t2 (nolock) ON a.TagId = t2.TagId	
		                    INNER JOIN	dbo.Passagem p3 (nolock) ON tp.PassagemId = p3.PassagemId	
	                        WHERE	
	                        p.CodigoPista	= @NumeroPista AND
	                        p2.CodigoPraca	= @NumeroPraca AND	
	                        t2.OBUId	= @NumeroTag AND
                            p3.CodigoPassagemConveniado	= @CodigoPassagemConveniado AND			
                            p3.ConveniadoId	= @ConveniadoId AND
	                        p3.Reenvio < @NumeroReenvio ";

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
                    CodigoPassagemConveniado = filter.ConveniadoPassagemId,
                    NumeroReenvio = filter.NumeroReenvio
                },
                commandTimeout: TimeHelper.CommandTimeOut);

                return result > 0;
            }
            
            
            

        }
    }
}
