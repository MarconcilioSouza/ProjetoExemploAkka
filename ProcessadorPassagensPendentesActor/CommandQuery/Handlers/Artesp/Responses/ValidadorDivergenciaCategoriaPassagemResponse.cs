using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Responses
{
    public class ValidadorDivergenciaCategoriaPassagemResponse
    {
        /// <summary>
        /// Passagem 
        /// </summary>
        public PassagemPendenteArtesp PassagemPendenteArtesp { get; set; }
    }
}
