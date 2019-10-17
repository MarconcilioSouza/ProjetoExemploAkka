using System.Data;
using System.Linq;
using ConectCar.Transacoes.Domain.ValueObject;
using Dapper;
using ProcessadorPassagensActors.CommandQuery.Dtos;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.Infrastructure;
using ProcessadorPassagensActors.CommandQuery.Queries.Filter;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterRepasseQuery : IQuery<ObterRepasseFilter, RepasseDto>
    {

        public RepasseDto Execute(ObterRepasseFilter filter)
        {
            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var transacao = conn.Query<RepasseDto>(
                "[dbo].[spObterRepasse]",
                new
                {
                    CodigoPista = filter.CodigoPista,
                    PracaId = filter.PracaId,
                    ConveniadoId = filter.ConveniadoId,
                    PlanoId = filter.PlanoId,
                    DataDePassagem = filter.DataPassagem
                },
                commandType: CommandType.StoredProcedure).FirstOrDefault();

                return transacao;
            }
            
        }
    }
}
