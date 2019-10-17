using System.Linq;
using ConectCar.Framework.Infrastructure.Cqrs.Ado.Queries;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.Model;
using ConectCar.Transacoes.Domain.ValueObject;
using Dapper;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterTransacaoProvisoriaQuery : IQuery<PassagemPendenteEDI, TransacaoProvisoriaEDI>
    {
        public TransacaoProvisoriaEDI Execute(PassagemPendenteEDI filter)
        {

            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var query = @"
                        SELECT t.TransacaoId as Id, tp2.*, t.* FROM dbo.TransacaoProvisoria tp	
		                        INNER JOIN	dbo.TransacaoPassagem tp2 ON tp.TransacaoPassagemId = tp2.TransacaoId	
								INNER JOIN dbo.Transacao t ON t.TransacaoId = tp2.TransacaoId
		                        INNER JOIN dbo.DetalheTRN dt ON tp2.DetalheTRNId = dt.DetalheTRNId	
	                        WHERE dt.NumeroTag	= @NumeroTag
	                        AND dt.NumeroPraca	= @NumeroPraca
	                        AND tp2.DataDePassagem	= @DataDePassagem
	                        AND tp.TransacaoDeCorrecaoId  IS NULL ";

                var result = conn.Query<TransacaoProvisoriaEDI>(
                    sql: query,
                    param: new
                    {
                        NumeroTag = filter.Tag.OBUId,
                        NumeroPraca = filter.Praca.CodigoPraca,
                        DataDePassagem = filter.DataPassagem
                    },
                    commandTimeout: TimeOutHelper.DezMinutos).FirstOrDefault();

                return result; 
            }
        }
    }
}
