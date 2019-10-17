using ConectCar.Transacoes.Domain.Model;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Request
{
 public   class IdentificadorPassagemDuplicadaEdiRequest
    {
        public PassagemPendenteEDI PassagemPendenteEdi { get; set; }
    }
}
