using ConectCar.Framework.Infrastructure.Cqrs.Ado.Queries;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using Dapper;
using System.Data;
using System.Linq;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ProcessadorPassagensActors.CommandQuery.Bo;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.Infrastructure;


namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterUltimaTransacaoPassagemTrnPorAdesaoIdQuery : IQuery<long, UltimaTransacaoPassagemDto>
    {
        public UltimaTransacaoPassagemDto Execute(long adesaoId)
        {
            var query = @"SELECT count(t.transacaoId) FROM dbo.TransacaoPassagem (NOLOCK) tp
			                      INNER JOIN dbo.Transacao (NOLOCK) t ON t.TransacaoId = tp.TransacaoId
			            WHERE t.AdesaoId = @adesaoId";

            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var count = conn.ExecuteScalar<int>(
                       sql: query,
                       param: new
                       {
                           adesaoId
                       },
                       commandTimeout: TimeOutHelper.DezMinutos);



                if (count > 0)
                {
                    query = @"SELECT TOP 1 tp.TransacaoId AS Id, tp.CategoriaUtilizadaId FROM dbo.TransacaoPassagem (NOLOCK) tp
			                      INNER JOIN dbo.Transacao (NOLOCK) t ON t.TransacaoId = tp.TransacaoId
					    WHERE t.AdesaoId = @adesaoId
	                    ORDER BY tp.TransacaoId DESC";

                    var result = conn.Query<UltimaTransacaoPassagemDto>(
                        query,
                        new
                        { adesaoId },
                        commandTimeout: ProcessadorPassagensActors.Infrastructure.TimeOutHelper.DezMinutos).ToList();

                    return result.FirstOrDefault();
                }

                return new UltimaTransacaoPassagemDto(); 
            }
        }
    }
}
