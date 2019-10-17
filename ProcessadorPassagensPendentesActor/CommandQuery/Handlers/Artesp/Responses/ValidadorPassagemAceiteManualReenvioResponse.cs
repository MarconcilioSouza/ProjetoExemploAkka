using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Responses
{
    public class ValidadorPassagemAceiteManualReenvioResponse
    {
        /// <summary>
        /// PassagemPendenteArtesp 
        /// </summary>
        public PassagemPendenteArtesp PassagemPendenteArtesp { get; set; }

        public MotivoNaoCompensado MotivoNaoCompensado { get; set; }
    }
}
