using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ConectCar.Transacoes.Domain.ValueObject;
using Dapper;
using ProcessadorPassagensActors.Infrastructure;
using System.Data;
using System.Linq;
using ProcessadorPassagensActors.CommandQuery.Connections;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterTransacaoPassagemPorTransacaoIdQuery : IQuery<long, TransacaoPassagemArtesp>
    {
        public ObterTransacaoPassagemPorTransacaoIdQuery()
        {
        }

        public TransacaoPassagemArtesp Execute(long filter)
        {
            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var result = conn.Query<TransacaoPassagemArtesp>("[dbo].[spObterTransacaoPorTransacaoId]",
                    new
                    {
                        @transacaoId = filter
                    },
                    commandType: CommandType.StoredProcedure).ToList();

                return result.FirstOrDefault();
            }

        }
    }
}
