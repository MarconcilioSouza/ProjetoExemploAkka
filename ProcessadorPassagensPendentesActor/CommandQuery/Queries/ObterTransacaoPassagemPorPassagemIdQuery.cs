using System.Linq;
using Dapper;
using ProcessadorPassagensActors.CommandQuery.Dtos;
using ProcessadorPassagensActors.Infrastructure;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ProcessadorPassagensActors.CommandQuery.Connections;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterTransacaoPassagemPorPassagemIdQuery : IQuery<long, TransacaoPassagemDto>
    {
        public ObterTransacaoPassagemPorPassagemIdQuery()
        {
        }

        public TransacaoPassagemDto Execute(long passagemId)
        {
            var query = @" SELECT 
                            tp.TransacaoId
                            , tp.DataRepasse
                            , tp.ValorRepasse ,
				            (
                                SELECT 
                                    Count(1) 
                                FROM dbo.DetalheViagem dviagem (NOLOCK) 
                                WHERE dviagem.TransacaoId = tp.TransacaoId 
                                OR dviagem.TransacaoProvisoriaId = tp.TransacaoId
                            ) AS ValePedagio
                            FROM dbo.TransacaoPassagem tp (NOLOCK) 
                            WHERE tp.PassagemId = @passagemId";

            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var result = conn.Query<TransacaoPassagemDto>(sql: query,
                    param: new
                    {
                        passagemId
                    },
                    commandTimeout: TimeHelper.CommandTimeOut).FirstOrDefault();

                return result;
            }
                
        }
    }
}
