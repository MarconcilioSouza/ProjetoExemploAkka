using ConectCar.Transacoes.Domain.Model;

namespace ProcessadorPassagensActors.ActorsMessages.Edi
{
    public class ValidadorDivergenciaCategoriaEdiMessage
    {
        public PassagemPendenteEDI PassagemPendenteEdi { get; set; }
    }
}