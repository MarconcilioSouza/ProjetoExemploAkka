using System.Linq;
using ConectCar.Comercial.Cliente.Adesao.Backend.CommonQuery.Query;
using ConectCar.Framework.Infrastructure.Cqrs.Handlers;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.Model;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.CommandQuery.Queries;
using ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Responses;
using ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Request;

namespace ProcessadorPassagensActors.CommandQuery.Bo
{
    public class DefinirCategoriaUtilizadaEdiBo : DataSourceHandlerBase,
        IAdoDataSourceProvider
    {
        public DbConnectionDataSourceProvider AdoDataSourceProvider => GetAdoProvider();

        protected override void Init()
        {
            AddProvider(new DbConnectionDataSourceProvider());
        }

        public CategoriaUtilizadasResponse Definir(CategoriaUtilizadasRequest request)
        {
            var dataSourceConectSysReadOnly = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSysReadOnly);

            var queryCategorias = new ObterCategoriaVeiculoQuery(true, dataSourceConectSysReadOnly);
            var categorias = DataBaseConnection.HandleExecution(queryCategorias.Execute).ToList();

            var categoria = categorias.FirstOrDefault(c => c.Codigo == request.Codigo);

            var categoriaUtilizada = new CategoriaUtilizadasResponse
            {
                CategoriaUtilizada = new CategoriaVeiculo { Id = categoria?.CategoriaVeiculoId, Codigo = categoria?.Codigo ?? 0 }
            };

            var queryListaDeParaCategVeic = new ObterListaDeParaCategoriaVeiculoPorIdQuery();
            var listaDeParaCategoriaVeiculo =
                DataBaseConnection.HandleExecution(queryListaDeParaCategVeic.Execute,request.ListaDeParaCategoriaVeiculoId);

            if (listaDeParaCategoriaVeiculo != null && listaDeParaCategoriaVeiculo.ValidarLista())
            {
                var queryItemListaDePara = new ObterItemListaDeParaPorCodigoEntradaQuery();
                var itemListaDePara = DataBaseConnection.HandleExecution(queryItemListaDePara.Execute,request.Codigo);

                if (itemListaDePara != null && itemListaDePara.ValidarLista()) //  definição por itemDeParaUtilizado
                {
                    categoria = categorias.FirstOrDefault(
                        c => c.CategoriaVeiculoId == itemListaDePara.CategoriaVeiculoId);

                    categoriaUtilizada.CategoriaUtilizada =
                        new CategoriaVeiculo { Id = categoria?.CategoriaVeiculoId, Codigo = categoria?.Codigo ?? 0 };
                    categoriaUtilizada.ItemListaDeParaUtilizado = itemListaDePara.ItemListaDeParaId;
                }
            }
            return categoriaUtilizada;
        }
    }
}
