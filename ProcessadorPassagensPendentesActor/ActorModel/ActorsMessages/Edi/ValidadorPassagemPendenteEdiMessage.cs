using ConectCar.Transacoes.Domain.Model;

namespace ProcessadorPassagensActors.ActorsMessages.Edi
{
    public class ValidadorPassagemPendenteEdiMessage
    {
        public PassagemPendenteEDI PassagemPendenteEdi { get; set; }
    }
}