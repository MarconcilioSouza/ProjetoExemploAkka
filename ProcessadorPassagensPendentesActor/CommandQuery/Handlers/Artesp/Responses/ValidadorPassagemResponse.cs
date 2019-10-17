using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Responses
{
    public class ValidadorPassagemResponse
    {
        /// <summary>
        /// PassagemPendenteArtesp 
        /// </summary>
        public PassagemPendenteArtesp PassagemPendenteArtesp { get; set; }

        public MotivoNaoCompensado MotivoNaoCompensado { get; set; }

        public bool PassagemInvalida { get; set; }
    }
}
