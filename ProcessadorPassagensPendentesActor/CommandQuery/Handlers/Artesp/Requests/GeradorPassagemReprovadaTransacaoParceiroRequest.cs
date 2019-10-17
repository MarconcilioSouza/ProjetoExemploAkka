using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests
{
    public class GeradorPassagemReprovadaTransacaoParceiroRequest
    {
        public MotivoNaoCompensado MotivoNaoCompensado { get;  set; }
        public int DetalheViagemId { get;  set; }
        public PassagemPendenteArtesp PassagemPendenteArtesp { get; set; }
    }
}
