using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using Dapper;
using ProcessadorPassagensActors.Infrastructure;
using System.Data;
using ProcessadorPassagensActors.CommandQuery.Connections;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterAdesaoPorDocumentoQuery : IQuery<long, int>
    {
        public ObterAdesaoPorDocumentoQuery()
        {
        }

        public int Execute(long documento)
        {
            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var retorno = conn.Query<int>(
             "[dbo].[spObterAdesaoPorDocumento]",
             new
             {
                 documento = documento

             },
             commandType: CommandType.StoredProcedure);

                return retorno.TryToInt();
            }
            
        }
    }
}
