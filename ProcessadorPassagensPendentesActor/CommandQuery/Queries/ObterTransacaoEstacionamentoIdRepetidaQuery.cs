using ConectCar.Framework.Infrastructure.Cqrs.Ado.Queries;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using Dapper;
using ProcessadorPassagensActors.CommandQuery.Queries.Filter;
using ProcessadorPassagensActors.Infrastructure;
using System.Linq;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ProcessadorPassagensActors.CommandQuery.Connections;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterTransacaoEstacionamentoIdRepetidaQuery : IQuery<ObterTransacaoEstacionamentoIdRepetidaFilter, int>
    {
        public int Execute(ObterTransacaoEstacionamentoIdRepetidaFilter filter)
        {
            var query = @"
                            SELECT TOP 1 te.TransacaoId 
                            FROM dbo.TransacaoEstacionamento te	(nolock)
                            WHERE 
	                            te.TagId	= @TagId AND
	                            te.PracaId	= @PracaId AND
	                            te.PistaId	= @PistaId AND 
	                            te.ConveniadoId	= @ConveniadoId AND	
	                            te.DataHoraEntrada	= @DataHoraEntrada AND
	                            te.DataHoraTransacao	= @DataHoraTransacao ";

            if (filter.TempoPermanencia > 0)
                query += "AND te.TempoPermanencia	 = @TempoPermanencia";
            else
                query += "AND te.TempoPermanencia	 > @TempoPermanencia";


            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var transacao = conn.Query<int>(
                     query,
                     new
                     {
                         filter.TagId,
                         filter.PracaId,
                         filter.PistaId,
                         filter.ConveniadoId,
                         filter.DataHoraEntrada,
                         filter.DataHoraTransacao,
                         filter.TempoPermanencia,
                     },
                     commandTimeout: TimeOutHelper.DezMinutos).FirstOrDefault();
                return transacao;
            }
        }
    }
}
