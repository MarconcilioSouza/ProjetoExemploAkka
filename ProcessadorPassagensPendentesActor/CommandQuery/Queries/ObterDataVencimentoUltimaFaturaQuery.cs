using System;
using System.Linq;
using Dapper;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterDataVencimentoUltimaFaturaQuery : IQuery<long, DateTime?>
    {
        public ObterDataVencimentoUltimaFaturaQuery()
        {
        }

        public DateTime? Execute(long clienteId)
        {
            var query = @"SELECT TOP 1 f.DataVencimento from dbo.Fatura f (NOLOCK)	
                                WHERE f.ClienteId	 = @clienteId ORDER BY f.DataCriacao desc";

            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var result = conn.Query<DateTime?>
                    (query, new
                        {
                        clienteId
                    }).FirstOrDefault();
                return result;
            }
            
        }
    }
}
