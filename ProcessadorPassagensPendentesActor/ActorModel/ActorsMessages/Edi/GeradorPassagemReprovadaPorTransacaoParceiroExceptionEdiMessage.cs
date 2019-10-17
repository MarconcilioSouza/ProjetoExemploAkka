using System.Collections.Generic;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.Model;

namespace ProcessadorPassagensActors.ActorsMessages.Edi
{
    public class GeradorPassagemReprovadaPorTransacaoParceiroExceptionEdiMessage
    {
        public PassagemPendenteEDI PassagemPendenteEdi { get; set; }
        public CodigoRetornoTransacaoTRF CodigoRetornoTransacaoTrf { get; set; }
        public int? DetalheViagemId { get; set; }
    }
}