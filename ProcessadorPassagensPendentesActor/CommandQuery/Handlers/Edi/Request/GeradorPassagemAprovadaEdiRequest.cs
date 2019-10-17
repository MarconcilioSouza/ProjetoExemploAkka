using ConectCar.Transacoes.Domain.Model;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Request
{
    public class GeradorPassagemAprovadaEdiRequest
    {
        public PassagemPendenteEDI PassagemPendenteEdi { get; set; }
    }
}
