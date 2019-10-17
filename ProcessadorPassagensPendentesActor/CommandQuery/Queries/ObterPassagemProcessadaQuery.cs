using System.Data;
using System.Linq;
using ConectCar.Transacoes.Domain.ValueObject;
using Dapper;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterPassagemProcessadaQuery : IQuery<long, PassagemAprovadaArtesp>
    {
        
        public ObterPassagemProcessadaQuery()
        {
            
        }

        public PassagemAprovadaArtesp Execute(long transacaoId)
        {
            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var transacao = conn.Query<PassagemAprovadaArtesp>(
                "[dbo].[spObterPassagemProcessada]",
                new
                {
                    TransacaoId = transacaoId
                },
                commandType: CommandType.StoredProcedure).FirstOrDefault();

                return transacao;
            }
            
        }
    }
}
