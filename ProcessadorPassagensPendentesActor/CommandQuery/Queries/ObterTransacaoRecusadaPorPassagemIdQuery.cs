using System.Linq;
using ConectCar.Transacoes.Domain.Model;
using Dapper;
using ProcessadorPassagensActors.Infrastructure;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ProcessadorPassagensActors.CommandQuery.Connections;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterTransacaoRecusadaPorPassagemIdQuery : IQuery<long, TransacaoRecusada>
    {
        public ObterTransacaoRecusadaPorPassagemIdQuery()
        {
        }

        public TransacaoRecusada Execute(long passagemId)
        {
            var query = @"SELECT 
                            tr.TransacaoRecusadaId as Id
                            , tr.MotivoRecusadoId
                            , tr.DataProcessamento
                            , tr.PassagemId 
                        FROM dbo.TransacaoRecusada tr (NOLOCK)	                                        
                        WHERE tr.PassagemId = @passagemId";

            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var result = conn.Query<TransacaoRecusada>(sql: query,
                param: new
                {
                    passagemId
                },
                commandTimeout: TimeHelper.CommandTimeOut).FirstOrDefault();

                return result;
            }            
        }
    }
}
