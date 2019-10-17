using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Responses
{
    public class IdentificadorPassagemAceiteManualReenvioResponse
    {
        /// <summary>
        /// Passagem Pendente
        /// </summary>
        public PassagemPendenteArtesp PassagemPendenteArtesp { get; set; }

    }
}
