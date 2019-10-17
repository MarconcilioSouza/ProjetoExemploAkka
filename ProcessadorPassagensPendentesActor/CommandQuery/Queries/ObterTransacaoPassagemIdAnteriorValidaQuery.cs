using ConectCar.Framework.Infrastructure.Cqrs.Ado.Queries;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.ValueObject;
using Dapper;
using System.Data;
using System.Linq;
using ConectCar.Cadastros.Conveniados.Backend.CommonQuery.Query;
using ProcessadorPassagensActors.Infrastructure;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ProcessadorPassagensActors.CommandQuery.Connections;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterTransacaoPassagemIdAnteriorValidaQuery : IQuery<PassagemPendenteArtesp, long>
    {
        public ObterTransacaoPassagemIdAnteriorValidaQuery()
        {
        }

        public long Execute(PassagemPendenteArtesp filter)
        {
            if (filter.NumeroReenvio < 0)
                return 0;

            long transacaoId = 0;

            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                if (filter.Conveniado.Id > 0)
                {
                    var query = @"
                                SELECT TOP 1 tp.TransacaoId         
				                FROM TransacaoPassagem tp (NOLOCK)        
				                INNER JOIN Passagem p (NOLOCK) ON tp.PassagemId = p.PassagemId        
				                WHERE p.CodigoPassagemConveniado = @codigoPassagemConveniado
				                AND p.ConveniadoId = @conveniadoId
				                AND p.Reenvio < @reenvio  ORDER BY 1 DESC";

                    

                    transacaoId = conn.Query<long>(
                        query,
                        new
                        {
                            codigoPassagemConveniado = filter.ConveniadoPassagemId,
                            conveniadoId = filter.Conveniado.Id ?? 0,
                            reenvio = filter.NumeroReenvio
                        },
                        commandTimeout: TimeHelper.CommandTimeOut).FirstOrDefault();

                    
                }
                else
                {
                    var query = @"
                                SELECT TOP 1 tp.TransacaoId         
				                FROM TransacaoPassagem tp (NOLOCK)        
				                INNER JOIN Passagem p (NOLOCK) ON tp.PassagemId = p.PassagemId        
				                INNER JOIN dbo.Conveniado c (NOLOCK) ON p.ConveniadoId = c.ConveniadoId	
				                WHERE p.CodigoPassagemConveniado = @codigoPassagemConveniado
				                AND c.CodigoProtocoloArtesp	= @codigoProtocoloArtesp
				                AND p.Reenvio < @reenvio ORDER BY 1 DESC";
                    

                    transacaoId = conn.Query<long>(query,
                        new
                        {
                            codigoPassagemConveniado = filter.ConveniadoPassagemId,
                            codigoProtocoloArtesp = filter.Conveniado.CodigoProtocoloArtesp,
                            reenvio = filter.NumeroReenvio
                        },
                        commandTimeout: TimeHelper.CommandTimeOut).FirstOrDefault();                    
                }
            }

            return transacaoId;
        }
    }
}
