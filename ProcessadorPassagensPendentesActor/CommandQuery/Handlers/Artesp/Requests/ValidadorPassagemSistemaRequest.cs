using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests
{
   public class ValidadorPassagemSistemaRequest
    {
        /// <summary>
        /// Passagem
        /// </summary>
        public PassagemPendenteArtesp PassagemPendenteArtesp { get; set; }

    }
}
