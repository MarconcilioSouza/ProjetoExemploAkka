using ConectCar.Transacoes.Domain.Model;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Request
{
    public class CategoriaUtilizadasRequest
    {
        public int ListaDeParaCategoriaVeiculoId { get; set; }
        public int Codigo { get; set; }
    }
}
