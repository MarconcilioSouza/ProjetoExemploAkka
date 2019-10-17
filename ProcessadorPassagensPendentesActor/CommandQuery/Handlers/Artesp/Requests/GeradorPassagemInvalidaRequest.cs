using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests
{
   public class GeradorPassagemInvalidaRequest
    {
        /// <summary>
        /// Passagem Inválida
        /// </summary>
        public PassagemPendenteArtesp PassagemPendenteArtesp { get; set; }
    }
}
