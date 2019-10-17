using System.Linq;
using ConectCar.Transacoes.Domain.Model;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Queries;
using ConectCar.Framework.Infrastructure.Log;
using ProcessadorPassagensActors.CommandQuery.Cache;
using System.Collections.Generic;
using ConectCar.Comercial.Domain.Model;
using ProcessadorPassagensActors.CommandQuery.Connections;

namespace ProcessadorPassagensActors.CommandQuery.Bo
{
    public class DefinirCategoriaUtilizadaArtesp : Loggable
    {              
        private ObterListaDeParaCategoriaVeiculoPorIdQuery _listaDeParaCategoriaVeiculoPorIdQuery;
        private ObterItemListaDeParaPorCodigoEntradaQuery _itemListaDeParaPorCodigoEntradaQuery;
        

        public DefinirCategoriaUtilizadaArtesp()
        {                        
            _listaDeParaCategoriaVeiculoPorIdQuery = new ObterListaDeParaCategoriaVeiculoPorIdQuery();
            _itemListaDeParaPorCodigoEntradaQuery = new ObterItemListaDeParaPorCodigoEntradaQuery();
        }


        public void Definir(PassagemPendenteArtesp passagemPendenteArtesp)
        {            
            var codigoCategoria = passagemPendenteArtesp.CategoriaCobrada?.Codigo ?? passagemPendenteArtesp.CategoriaDetectada.Codigo;
            var categorias = CategoriaVeiculoCacheRepository.Listar();
            var categoria = categorias.FirstOrDefault(c => c.Codigo == codigoCategoria);

            passagemPendenteArtesp.CategoriaUtilizada =
                new CategoriaVeiculo {
                    Id = categoria?.CategoriaVeiculoId,
                    Codigo = categoria?.Codigo ?? 0
                };
            
            var listaDeParaCategoriaVeiculo =
                DataBaseConnection.HandleExecution(_listaDeParaCategoriaVeiculoPorIdQuery.Execute,passagemPendenteArtesp.Conveniado.ListaDeParaCategoriaVeiculoId);


            if (listaDeParaCategoriaVeiculo != null && listaDeParaCategoriaVeiculo.ValidarLista())
            {                
                var itemListaDePara = DataBaseConnection.HandleExecution(_itemListaDeParaPorCodigoEntradaQuery.Execute,codigoCategoria);

                if (itemListaDePara != null && itemListaDePara.ValidarLista()) //  definição por itemDeParaUtilizado
                {
                    categoria = categorias.FirstOrDefault(
                        c => c.CategoriaVeiculoId == itemListaDePara.CategoriaVeiculoId);

                    passagemPendenteArtesp.CategoriaUtilizada =
                        new CategoriaVeiculo {
                            Id = categoria?.CategoriaVeiculoId,
                            Codigo = categoria?.Codigo ?? 0
                       };
                    passagemPendenteArtesp.ItemListaDeParaUtilizado = itemListaDePara.ItemListaDeParaId;
                }
            }

        }
    }
}
