using ConectCar.Transacoes.Domain.Model;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Request
{
    public class ValidadorSlaListaNelaEdiRequest
    {
        public PassagemPendenteEDI PassagemPendenteEdi { get; set; }
    }
}
