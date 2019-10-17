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
    public class ObterCountPossuiTransacaoProvisoriaQuery : IQuery<PassagemPendenteEDI, bool>
    {
        public bool Execute(PassagemPendenteEDI filter)
        {
            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var query = @"
                        SELECT COUNT(*) FROM dbo.TransacaoProvisoria tp	
		                        INNER JOIN	dbo.TransacaoPassagem tp2 ON tp.TransacaoPassagemId = tp2.TransacaoId	
		                        INNER JOIN dbo.DetalheTRN dt ON tp2.DetalheTRNId = dt.DetalheTRNId	
	                        WHERE dt.NumeroTag	= @NumeroTag
	                        AND dt.NumeroPraca	= @NumeroPraca
	                        AND tp2.DataDePassagem	= @DataDePassagem
	                        AND tp.TransacaoDeCorrecaoId  IS NULL ";

                var result = conn.ExecuteScalar<int>(
                    sql: query,
                    param: new
                    {
                        NumeroTag = filter.Tag.OBUId,
                        NumeroPraca = filter.Praca.CodigoPraca,
                        DataDePassagem = filter.DataPassagem
                    },
                    commandTimeout: TimeHelper.CommandTimeOut);

                return result > 0; 
            }
        }
    }
}
