using Dapper;
using System.Data;
using ProcessadorPassagensActors.CommandQuery.Dtos;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterAdesaoPorPlacaDocumentoQuery : IQuery<ObterStatusAdesaoIdPlacaDocumentoPortransacaoIdOriginalDto, int>
    {
        public ObterAdesaoPorPlacaDocumentoQuery()
        {
        }

        public int Execute(ObterStatusAdesaoIdPlacaDocumentoPortransacaoIdOriginalDto filter)
        {
            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var transacao = conn.Query<int>(
             "[dbo].[spObterAdesaoPorPlacaDocumento]",
             new
             {
                 placa = filter.Placa,
                 documento = filter.Documento

             },
             commandType: CommandType.StoredProcedure);

                return transacao.TryToInt();
            }
            
        }
    }
}
