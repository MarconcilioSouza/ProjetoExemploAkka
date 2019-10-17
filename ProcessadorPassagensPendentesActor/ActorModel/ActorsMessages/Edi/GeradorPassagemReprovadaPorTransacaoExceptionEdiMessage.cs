using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.Model;

namespace ProcessadorPassagensActors.ActorsMessages.Edi
{
    public class GeradorPassagemReprovadaPorTransacaoExceptionEdiMessage
    {
        public PassagemPendenteEDI PassagemPendenteEdi { get; set; }
        public CodigoRetornoTransacaoTRF CodigoRetornoTransacaoTrf { get; set; }
    }
}