using ConectCar.Transacoes.Domain.Model;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Responses
{
    public class CategoriaUtilizadasResponse
    {
        public CategoriaVeiculo CategoriaUtilizada { get; set; }
        public int ItemListaDeParaUtilizado { get; set; }
    }
}
