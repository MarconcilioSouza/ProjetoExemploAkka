using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.Model;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Request
{
    public class GeradorPassagemReprovadaPorTransacaoExceptionEdiRequest
    {
        public PassagemPendenteEDI PassagemPendenteEdi { get; set; }
        public CodigoRetornoTransacaoTRF CodigoRetornoTransacaoTrf { get; set; }
    }
}
