using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests
{
    public class ValidadorPassagemAceiteManualReenvioRequest
    {
        /// <summary>
        /// PassagemPendenteArtesp 
        /// </summary>
        public PassagemPendenteArtesp PassagemPendenteArtesp { get; set; }

    }
}
