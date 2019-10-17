using ConectCar.Transacoes.Domain.Model;

namespace ProcessadorPassagensActors.ActorsMessages.Edi
{
    public class IdentificadorPassagemDuplicadaEdiMessage
    {

        public PassagemPendenteEDI PassagemPendenteEdi { get; set; }
    }
}