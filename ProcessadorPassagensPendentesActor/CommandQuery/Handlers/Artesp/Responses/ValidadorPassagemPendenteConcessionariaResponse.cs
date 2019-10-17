using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Responses
{
    public class ValidadorPassagemPendenteConcessionariaResponse
    {
        public PassagemPendenteArtesp PassagemPendenteArtesp { get; set; }
        public MotivoNaoCompensado MotivoNaoCompensado { get; set; }
    }
}
