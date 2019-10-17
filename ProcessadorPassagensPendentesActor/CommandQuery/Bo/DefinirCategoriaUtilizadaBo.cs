using System.Linq;
using ConectCar.Comercial.Cliente.Adesao.Backend.CommonQuery.Query;
using ConectCar.Framework.Infrastructure.Cqrs.Handlers;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.Model;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Queries;

namespace ProcessadorPassagensActors.CommandQuery.Bo
{
    public class DefinirCategoriaUtilizadaBo : DataSourceHandlerBase,
        IAdoDataSourceProvider
    {
        public DbConnectionDataSourceProvider AdoDataSourceProvider => GetAdoProvider();
        private PassagemPendenteArtesp PassagemPendenteArtesp { get; }

        protected override void Init()
        {
            AddProvider(new DbConnectionDataSourceProvider());
        }

        public DefinirCategoriaUtilizadaBo(PassagemPendenteArtesp passagemPendenteArtesp)
        {
            PassagemPendenteArtesp = passagemPendenteArtesp;
        }


        public void Definir()
        {
            var dataSourceConectSysReadOnly = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSysReadOnly);
            
            var queryCategorias = new ObterCategoriaVeiculoQuery(true, dataSourceConectSysReadOnly);
            var categorias = queryCategorias.Execute().ToList();

            
            var codigoCategoria = PassagemPendenteArtesp.CategoriaCobrada?.Codigo ?? PassagemPendenteArtesp.CategoriaDetectada.Codigo;
            var categoria = categorias.FirstOrDefault(c => c.Codigo == codigoCategoria);

            PassagemPendenteArtesp.CategoriaUtilizada =
                new CategoriaVeiculo {Id = categoria?.CategoriaVeiculoId, Codigo = categoria?.Codigo ?? 0};

            var queryListaDeParaCategVeic = new ObterListaDeParaCategoriaVeiculoPorIdQuery(dataSourceConectSysReadOnly);
            var listaDeParaCategoriaVeiculo =
                queryListaDeParaCategVeic.Execute(PassagemPendenteArtesp.Conveniado.ListaDeParaCategoriaVeiculoId);


            if (listaDeParaCategoriaVeiculo != null && listaDeParaCategoriaVeiculo.ValidarLista())
            {
                var queryItemListaDePara = new ObterItemListaDeParaPorCodigoEntradaQuery(dataSourceConectSysReadOnly);
                var itemListaDePara = queryItemListaDePara.Execute(codigoCategoria);

                if (itemListaDePara != null && itemListaDePara.ValidarLista()) //  definição por itemDeParaUtilizado
                {
                    categoria = categorias.FirstOrDefault(
                        c => c.CategoriaVeiculoId == itemListaDePara.CategoriaVeiculoId);

                    PassagemPendenteArtesp.CategoriaUtilizada =
                        new CategoriaVeiculo {Id = categoria?.CategoriaVeiculoId, Codigo = categoria?.Codigo ?? 0};
                    PassagemPendenteArtesp.ItemListaDeParaUtilizado = itemListaDePara.ItemListaDeParaId;
                }
            }

        }
    }
}
