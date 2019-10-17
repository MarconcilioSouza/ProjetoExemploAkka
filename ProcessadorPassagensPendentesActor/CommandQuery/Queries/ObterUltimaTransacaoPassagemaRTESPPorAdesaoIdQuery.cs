using System.Data;
using System.Linq;
using Dapper;
using ProcessadorPassagensActors.CommandQuery.Dtos;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterUltimaTransacaoPassagemArtespPorAdesaoIdQuery : IQuery<long, UltimaTransacaoPassagemDto>
    {
        public ObterUltimaTransacaoPassagemArtespPorAdesaoIdQuery()
        {
        }

        public UltimaTransacaoPassagemDto Execute(long adesaoId)
        {
            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var result = conn.Query<UltimaTransacaoPassagemDto>(
                                "[dbo].[spObterUltimaTransacaoPassagemArtespPorAdesaoId]",
                                new
                                {
                                    adesaoId = adesaoId

                                },
                                commandType: CommandType.StoredProcedure,
                                commandTimeout: TimeHelper.CommandTimeOut).ToList();

                return result.FirstOrDefault();
            }

            
        }
    }
}
