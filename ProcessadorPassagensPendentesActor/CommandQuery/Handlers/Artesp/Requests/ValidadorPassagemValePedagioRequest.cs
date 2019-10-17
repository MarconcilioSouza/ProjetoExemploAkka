using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests
{
   public class ValidadorPassagemValePedagioRequest
    {
        /// <summary>
        /// PassagemPendenteArtesp
        /// </summary>
        public PassagemPendenteArtesp PassagemPendenteArtesp { get; set; }
    }
}
