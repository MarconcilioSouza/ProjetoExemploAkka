using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests
{
   public class ValidadorPassagemExistenteRequest
    {

        /// <summary>
        /// Passagem Pendente
        /// </summary>
        public PassagemPendenteArtesp PassagemPendenteArtesp { get; set; }

        public long PassagemId { get; set; }



    }
}
