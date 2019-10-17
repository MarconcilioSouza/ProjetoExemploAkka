using System.Linq;
using ConectCar.Transacoes.Domain.ValueObject;
using Dapper;
using ProcessadorPassagensActors.Infrastructure;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ProcessadorPassagensActors.CommandQuery.Connections;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterTransacaoPassagemPorAdesaoIdQuery : IQuery<long, int?>
    {
        public ObterTransacaoPassagemPorAdesaoIdQuery()
        {
        }

        public int? Execute(long AdesaoId)
        {
            var query = @"SELECT TOP 1
                                TP.CategoriaUtilizadaId 
                            FROM Transacao T 
                            INNER JOIN TransacaoPassagem TP
                            ON T.TransacaoId = TP.TransacaoId
					        INNER JOIN Adesao A
					        ON A.AdesaoId = T.AdesaoId                            
	                        WHERE A.AdesaoId =	@AdesaoId
                            ORDER BY TP.TransacaoId DESC"; 

            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var result = conn.Query<int?>(sql: query,
                                param: new
                                {
                                    AdesaoId
                                },
                                commandTimeout: TimeHelper.CommandTimeOut).FirstOrDefault();

                return result;
            }
            
        }
    }
}
