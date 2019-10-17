using Dapper;
using System.Data;
using System.Linq;
using ProcessadorPassagensActors.CommandQuery.Queries.Filter;
using ProcessadorPassagensActors.CommandQuery.Dtos;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterPassagemAnteriorValidaQuery : IQuery<ObterPassagemAnteriorValidaCompletaFilter, PassagemAnteriorValidaDto>
    {
        
        public ObterPassagemAnteriorValidaQuery()
        {
            
        }

        public PassagemAnteriorValidaDto Execute(ObterPassagemAnteriorValidaCompletaFilter filter)
        {
            if (filter.Reenvio == 0)
                return null;

            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var passagemDto = conn.Query<PassagemAnteriorValidaDto>(
                "[dbo].[spObterPassagemAnteriorValida]",
                new
                {
                    conveniadoId = filter.ConveniadoId,
                    codigoProtocoloArtesp = filter.CodigoProtocoloArtesp,
                    reenvio = filter.Reenvio,
                    codigoPassagemConveniado = filter.CodigoPassagemConveniado
                },
                commandType: CommandType.StoredProcedure).FirstOrDefault();

                return passagemDto;
            }            
        }
    }
}
