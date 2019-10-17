using ConectCar.Transacoes.Domain.Model;
using ConectCar.Transacoes.Domain.ValueObject;
using System.Collections.Generic;

namespace ProcessadorPassagensActors.ActorsMessages.Artesp
{
    public class ProcessadorPassagemAprovadaArtespMessage
    {
        /// <summary>
        /// Passagem Aprovada
        /// </summary>
        public PassagemAprovadaArtesp PassagemAprovadaArtesp { get; set; }

        public int CodigoProtocoloArtesp { get; set; }

        public string ChaveCriptografiaBanco { get; set; }

    }
}