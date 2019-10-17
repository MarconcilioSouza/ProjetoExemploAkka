using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests
{
   public class GeradorPassagemPendenteReprovadaRequest
    {
        /// <summary>
        /// Passagem Reprovada
        /// </summary>
        public PassagemPendenteArtesp PassagemPendenteArtesp { get; set; }

        public MotivoNaoCompensado MotivoNaoCompensado { get; set; }
    }
}
