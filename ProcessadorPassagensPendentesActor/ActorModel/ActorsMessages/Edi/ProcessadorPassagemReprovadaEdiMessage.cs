using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.ActorsMessages.Edi
{
    public class ProcessadorPassagemReprovadaEdiMessage
    {
        public PassagemReprovadaEDI PassagemReprovadaEdi { get; set; }
    }
}