using ConectCar.Transacoes.Domain.Model;

namespace ProcessadorPassagensActors.ActorsMessages.Edi
{
    public class GeradorPassagemEdiMessage
    {
        public PassagemPendenteEDI PassagemPendenteEdi { get; set; }
    }
}