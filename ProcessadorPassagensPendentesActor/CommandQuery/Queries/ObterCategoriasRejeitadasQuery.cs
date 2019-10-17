using Dapper;
using ProcessadorPassagensActors.CommandQuery.Queries.Filter;
using ProcessadorPassagensActors.Infrastructure;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ProcessadorPassagensActors.CommandQuery.Connections;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterCategoriasRejeitadasQuery : IQuery<ObterCategoriasRejeitadasFilter, bool> 
    {
        public ObterCategoriasRejeitadasQuery()
        {
        }
        public bool Execute(ObterCategoriasRejeitadasFilter filter)
        {
            var query = @"
                          SELECT count(*) FROM dbo.CategoriaRejeitada cr    (nolock)
                                WHERE cr.ConveniadoId    = @ConveniadoId
                                AND cr.CategoriaVeiculoId    = @CategoriaVeiculoId
                                and cr.VigenciaCategoriaRejeitada    <= getdate()";

            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                var result = conn.ExecuteScalar<int>(sql: query,
                    param: new
                    {
                        ConveniadoId = filter.ConveniadoId,
                        CategoriaVeiculoId = filter.CategoriaId
                    },
                commandTimeout: TimeHelper.CommandTimeOut);
                return result > 0;
            }
            
        }
    }

}
