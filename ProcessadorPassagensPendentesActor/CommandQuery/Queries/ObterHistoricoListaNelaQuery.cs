using System.Data;
using System.Linq;
using ConectCar.Framework.Infrastructure.Cqrs.Ado.Queries;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.Model;
using Dapper;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.CommandQuery.Dtos;
using ProcessadorPassagensActors.CommandQuery.Queries.Filter;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterHistoricoListaNelaQuery : IQuery<ObterHistoricoListaNelaFilter, HistoricoListaNelaDto>
    {
        public HistoricoListaNelaDto Execute(ObterHistoricoListaNelaFilter filter)
        {
            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var historico = conn.Query<HistoricoListaNelaDto>(
                    @"select top 1
                        hln.HistoricoListaNelaId,
                        hln.DataEntrada,
                        hln.DataSaida
                    from HistoricoListaNela hln (nolock)
                where hln.TagId = @TagId and hln.DataEntrada <= @DataPassagem and (hln.DataSaida is null or hln.DataSaida > @DataPassagem )
                order by hln.HistoricoListaNelaId desc",
                    new
                    {
                        filter.TagId,
                        DataPassagem = filter.DataDePassagem
                    }).FirstOrDefault();
                return historico;
            }
        }
    }
}
