using ConectCar.Cadastros.Conveniados.Backend.CommonQuery.Filter;
using ConectCar.Cadastros.Domain.Dto;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using Dapper;
using ProcessadorPassagensActors.Infrastructure;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ProcessadorPassagensActors.CommandQuery.Connections;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterPistaPracaConveniadoArtespCacheOriginQuery2 : IQuery<List<PistaPracaConveniadoDto>>
    {

        public ObterPistaPracaConveniadoArtespCacheOriginQuery2()
        {
        }

        public List<PistaPracaConveniadoDto> Execute()
        {
            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var resultado = conn.Query<PistaPracaConveniadoDto>(
                 "spListarPistasCodigoProtocoloArtesp",
                 commandType: CommandType.StoredProcedure
             );
                return resultado.ToList();
            }

            
        }
    }
}
