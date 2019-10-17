using Dapper;
using System.Data;
using System.Linq;
using ProcessadorPassagensActors.CommandQuery.Dtos;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterStatusAdesaoIdPlacaDocumentoPortransacaoIdOriginalQuery : IQuery<long, ObterStatusAdesaoIdPlacaDocumentoPortransacaoIdOriginalDto>
    {
        public ObterStatusAdesaoIdPlacaDocumentoPortransacaoIdOriginalQuery()
        {
        }
        
        public ObterStatusAdesaoIdPlacaDocumentoPortransacaoIdOriginalDto Execute(long transacaoIdOriginal)
        {
            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var result = conn.Query<ObterStatusAdesaoIdPlacaDocumentoPortransacaoIdOriginalDto>(
             "[dbo].[spObterStatusAdesaoIdPlacaDocumentoPortransacaoIdOriginal]",
             new
             {
                 transacaoId = transacaoIdOriginal

             },
             commandType: CommandType.StoredProcedure);

                return result.FirstOrDefault();
            }
            
        }
    }
}
