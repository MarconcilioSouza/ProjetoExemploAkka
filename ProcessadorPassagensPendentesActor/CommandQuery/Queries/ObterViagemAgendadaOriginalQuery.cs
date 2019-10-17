using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using Dapper;
using ProcessadorPassagensActors.CommandQuery.Dtos;
using ProcessadorPassagensActors.CommandQuery.Queries.Filter;
using ProcessadorPassagensActors.Infrastructure;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ProcessadorPassagensActors.CommandQuery.Connections;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterViagemAgendadaOriginalQuery : IQuery<PlacaDataPassagemETransacaoIdOriginalFilter, IEnumerable<DetalheViagemAgendadaDto>>
    {
        public ObterViagemAgendadaOriginalQuery()
        {
        }

        public IEnumerable<DetalheViagemAgendadaDto> Execute(PlacaDataPassagemETransacaoIdOriginalFilter filter)
        {
            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var result = conn.Query<DetalheViagemAgendadaDto>("[dbo].[spObterViagemAgendadaOriginal]",
                    new
                    {
                        placa = filter.Placa,
                        dataPassagem = filter.DataPassagem,
                        transacaoPassagem = filter.TransacaoIdOriginal
                    },commandType: CommandType.StoredProcedure).ToList();

                return result;
            }

        }
    }
}
