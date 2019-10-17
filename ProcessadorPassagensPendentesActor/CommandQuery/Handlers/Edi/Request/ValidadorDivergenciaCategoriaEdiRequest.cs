using ConectCar.Transacoes.Domain.Model;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Request
{
    public class ValidadorDivergenciaCategoriaEdiRequest
    {
        public PassagemPendenteEDI PassagemPendenteEdi { get; set; }
    }
}
