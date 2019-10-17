using ConectCar.Framework.Infrastructure.Cqrs.Ado.Queries;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.Model;
using Dapper;
using ProcessadorPassagensActors.CommandQuery.Queries.Filter;
using System.Data;
using System.Linq;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.CommandQuery.Dtos;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterDetalheTrnPorTransacaoIdQuery : IQuery<long, DetalheTrnDto>
    {
        
        public DetalheTrnDto Execute(long filter)
        {
            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var transacao = conn.Query<DetalheTrnDto>(
                  @"SELECT dt.* FROM dbo.DetalheTRN dt	
                    INNER JOIN dbo.TransacaoPassagem tp ON dt.DetalheTRNId = tp.DetalheTRNId
                    WHERE tp.TransacaoId = @transacaoId",
                   new
                   {
                       transacaoId = filter

                   },
                   commandTimeout: TimeOutHelper.DezMinutos).FirstOrDefault();

                return transacao; 
            }
        }
    }
}
