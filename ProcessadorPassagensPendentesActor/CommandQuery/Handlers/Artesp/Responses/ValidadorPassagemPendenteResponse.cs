using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Responses
{
    public class ValidadorPassagemPendenteResponse
    {
        /// <summary>
        /// Passagem Pendente
        /// </summary>
        public PassagemPendenteArtesp PassagemPendenteArtesp { get; set; }
        
        public MotivoNaoCompensado MotivoNaoCompensado { get; set; }        
    }
}
