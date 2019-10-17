using System.Linq;
using Dapper;
using ProcessadorPassagensActors.CommandQuery.Dtos;
using ProcessadorPassagensActors.Infrastructure;
using ConectCar.Framework.Infrastructure.Cqrs.Queries;
using ProcessadorPassagensActors.CommandQuery.Connections;

namespace ProcessadorPassagensActors.CommandQuery.Queries
{
    public class ObterItemListaDeParaPorCodigoEntradaQuery : IQuery<int, ItemListaDeParaDto>
    {
        public ObterItemListaDeParaPorCodigoEntradaQuery() : base()
        {
        }

        /// <summary>
        /// ObterItemListaDeParaPorCodigoEntradaQuerys
        /// </summary>
        /// <param name="codigoEntrada">codigoEntrada</param>
        /// <returns></returns>
        public ItemListaDeParaDto Execute(int codigoEntrada)
        {
            using (var conn = DataBaseConnection.GetConnection(true, TimeHelper.CommandTimeOut))
            {                
                var query = @"
                SELECT 
                    ildp.ItemListaDeParaId
                    , ildp.CategoriaVeiculoId
                    , ildp.StatusId 
                FROM dbo.ItemListaDePara ildp (NOLOCK)	
                WHERE ildp.CodigoEntrada = @codigoEntrada";

                var result = conn.Query<ItemListaDeParaDto>
                (query,
                    new
                    {
                        codigoEntrada
                    }, commandTimeout: TimeHelper.CommandTimeOut).FirstOrDefault();
                

                return result;
            }                
        }
    }
}
