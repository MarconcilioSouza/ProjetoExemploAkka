using ConectCar.Transacoes.Domain.Model;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Request
{
    public class ValidadorPassagemSistemaEdiActorRequest
    {
        public PassagemPendenteEDI PassagemPendenteEdi { get; set; }
    }
}
