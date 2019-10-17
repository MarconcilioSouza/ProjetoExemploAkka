using ConectCar.Transacoes.Domain.Model;

namespace ProcessadorPassagensActors.ActorsMessages.Edi
{
    public class ValidadorSlaListaNelaEdiMessage
    {
        public PassagemPendenteEDI PassagemPendenteEdi { get; set; }
    }
}