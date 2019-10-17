using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests
{
   public class GeradorPassagemReprovadaDivergenciaCategoriaRequest
    {
        /// <summary>
        /// Passagem
        /// </summary>
        public PassagemPendenteArtesp PassagemPendenteArtesp { get; set; }

        public MotivoNaoCompensado MotivoNaoCompensado { get; set; }
    }
}
