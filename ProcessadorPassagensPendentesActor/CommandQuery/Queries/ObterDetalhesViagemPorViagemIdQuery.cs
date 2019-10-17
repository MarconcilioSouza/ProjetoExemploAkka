using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ConectCar.Transacoes.Domain.Model;
using Dapper;
using ProcessadorPassagensActors.Infrastructure;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ProcessadorPassagensActors.CommandQuery.Connections;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterDetalhesViagemPorViagemIdQuery : IQuery<long, List<DetalheViagem>>
    {
        public ObterDetalhesViagemPorViagemIdQuery()
        {
        }

        public List<DetalheViagem> Execute(long viagemId)
        {
            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var transacao = conn.Query<DetalheViagem>(
              "[dbo].[spObterDetalheViagemPorViagemId]",
              new
              {
                  ViagemId = viagemId

              },
              commandType: CommandType.StoredProcedure);

                return transacao.ToList();
            }
            
        }
    }
}
