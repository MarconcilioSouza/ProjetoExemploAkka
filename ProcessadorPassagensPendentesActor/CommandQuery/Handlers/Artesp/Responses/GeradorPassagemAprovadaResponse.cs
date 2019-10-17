using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Responses
{
    public class GeradorPassagemAprovadaResponse
    {

        /// <summary>
        /// Passagem Aprovada
        /// </summary>
        public PassagemAprovadaArtesp PassagemAprovadaArtesp { get; set; }

        public int CodigoProtocoloArtesp { get; set; }

    }
}
