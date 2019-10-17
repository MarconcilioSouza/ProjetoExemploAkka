using System.Linq;
using Dapper;
using ProcessadorPassagensActors.CommandQuery.Dtos;
using ProcessadorPassagensActors.Infrastructure;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ProcessadorPassagensActors.CommandQuery.Connections;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterListaDeParaCategoriaVeiculoPorIdQuery : IQuery<int, ListaDeParaCategoriaVeiculoDto>
    {
        public ObterListaDeParaCategoriaVeiculoPorIdQuery()
        {
        }

        public ListaDeParaCategoriaVeiculoDto Execute(int listaDeParaCategoriaVeiculoId)
        {
            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {
                
                var query = @"
                        SELECT 
                            ldpcv.ListaDeParaCategoriaVeiculoId
                            , ldpcv.Descricao
                            , ldpcv.StatusId 
                        FROM dbo.ListaDeParaCategoriaVeiculo ldpcv (NOLOCK)
                        WHERE ldpcv	.ListaDeParaCategoriaVeiculoId	= @listaDeParaCategoriaVeiculoId";

                var result = conn.Query<ListaDeParaCategoriaVeiculoDto>
                (query,
                    new
                    {
                        listaDeParaCategoriaVeiculoId
                    }, commandTimeout: TimeHelper.CommandTimeOut).FirstOrDefault();                

                return result;
            }               
        }
    }
}
