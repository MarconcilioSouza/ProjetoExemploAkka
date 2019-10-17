using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.ActorsMessages.Artesp
{
    public class ProcessadorPassagemReprovadaArtespMessage
    {
        /// <summary>
        /// Passagem Reprovada
        /// </summary>
        public PassagemReprovadaArtesp PassagemReprovadaArtesp { get; set; }

        public override string ToString()
        {
            return $"Passagem ID: {PassagemReprovadaArtesp.MensagemItemId} Conveniado {PassagemReprovadaArtesp.Conveniado.CodigoProtocoloArtesp}";
        }


    }
}