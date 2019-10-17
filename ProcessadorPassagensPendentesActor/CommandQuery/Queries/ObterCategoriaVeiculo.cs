using System.Data;
using System.Linq;
using ConectCar.Transacoes.Domain.Model;
using Dapper;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.Infrastructure;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterCategoriaVeiculo : IQuery<int, CategoriaVeiculo>
    {
        
        public ObterCategoriaVeiculo()
        {
            
        }

        public CategoriaVeiculo Execute(int Codigo)
        {
            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var transacao = conn.Query<CategoriaVeiculo>(
                                "[dbo].[spObterCategoriaVeiculo]",
                                new
                                {
                                    Codigo = Codigo
                                },
                                commandType: CommandType.StoredProcedure).FirstOrDefault();

                return transacao;
            }
            
        }
    }
}
