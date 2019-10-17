using ConectCar.Transacoes.Domain.Model;

namespace ProcessadorPassagensActors.ActorsMessages.Edi
{
    public class ValidadorPassagemEdiMessage
    {
        public PassagemPendenteEDI PassagemPendenteEdi { get; set; }
    }
}