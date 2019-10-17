using System.Linq;
using Dapper;
using ProcessadorPassagensActors.CommandQuery.Dtos;
using ProcessadorPassagensActors.CommandQuery.Queries.Filter;
using ProcessadorPassagensActors.Infrastructure;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ProcessadorPassagensActors.CommandQuery.Connections;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterTagNoMomentoDaPassagemMensageriaQuery : IQuery<TagNoMomentoDaPassagemFilter, TagMensageriaDto>
    {
        public ObterTagNoMomentoDaPassagemMensageriaQuery()
        {
        }
        public TagMensageriaDto Execute(TagNoMomentoDaPassagemFilter filter)
        {
            var momentoPassagemSla = filter.PassagemPendenteArtesp.DataPassagem.AddMinutes(-filter.TempoAtualizacaoPista);
            TagMensageriaDto tagMensageriaDto = null;

            using (var conn = DataBaseConnection.GetConnection(DataBaseSourceType.Mensageria))
            {
                var query = @"SELECT  t.Id,                                
                                t.SituacaoId                                
                            FROM ( 
	                            SELECT MAX(t.MensagemId) AS MensagemId, t.TagId
	                            FROM    dbo.Tag t (NOLOCK)
		                            INNER JOIN MensagemTag mt (NOLOCK) ON mt.Id = t.MensagemId
                                WHERE   t.TagId = @TagId
                                    AND     mt.ConcessionariaId = @ConcessionariaId
                                    AND     mt.DataEnvio between '1971-01-01 00:00:00.000' and @dataEnvio 
                                GROUP BY t.TagId
                            ) AS mt
	                            INNER JOIN dbo.Tag AS t (NOLOCK) ON t.MensagemId = mt.MensagemId
		                            AND t.TagId = mt.TagId";


                tagMensageriaDto = conn.Query<TagMensageriaDto>(sql: query,
                param: new
                {
                    TagId = filter.PassagemPendenteArtesp.Tag.OBUId,
                    ConcessionariaId = filter.PassagemPendenteArtesp.Conveniado.CodigoProtocoloArtesp,
                    dataEnvio = momentoPassagemSla
                },
                commandTimeout: TimeHelper.CommandTimeOut).FirstOrDefault();
            }


            if (tagMensageriaDto?.SituacaoId != null)
            {
                using (var conn = DataBaseConnection.GetConnection(DataBaseSourceType.Mensageria))
                {
                    var queryPracas = @"SELECT pb.CodigoPraca, pb.Id 
									FROM dbo.PracaBloqueada pb	INNER JOIN dbo.Tag t	ON pb.Id = t.Id
								WHERE t.Id	 = @id";

                    var pracas = conn.Query<PracasBloqueadas>(sql: queryPracas,
                        param: new
                        {
                            id = filter.PassagemPendenteArtesp.Tag.OBUId
                        },
                        commandTimeout: TimeHelper.CommandTimeOut).ToList();

                    tagMensageriaDto.PracasBloqueadases = pracas;
                }
                
            }            

            return tagMensageriaDto;
        }
    }
}
