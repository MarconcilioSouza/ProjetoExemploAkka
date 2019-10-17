using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.Model;
using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Request
{
    public class ProcessadorPassagemReprovadaEdiRequest
    {
        public PassagemReprovadaEDI PassagemReprovadaEDI { get; set; }
    }
}
