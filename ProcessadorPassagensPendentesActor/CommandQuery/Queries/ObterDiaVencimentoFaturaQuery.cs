using System;
using System.Linq;
using Dapper;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterDataCadastroConfiguracaoPlanoQuery : IQuery<long, DateTime?>
    {
        public ObterDataCadastroConfiguracaoPlanoQuery()
        {
        }

        public DateTime? Execute(long clienteId)
        {
            var query = @"
                            SELECT TOP 1 cpc.DataDeCadastro
	                            FROM dbo.ConfiguracaoPlanoCliente cpc (NOLOCK)			
                                WHERE cpc.PlanoDePagamentoId		 = 4 AND cpc.ClienteId	= @clienteId";

            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var result = conn.Query<DateTime?>(query, 
                    new
                    {
                        clienteId
                    }).FirstOrDefault();
                return result;
            }

            
        }
    }
}
