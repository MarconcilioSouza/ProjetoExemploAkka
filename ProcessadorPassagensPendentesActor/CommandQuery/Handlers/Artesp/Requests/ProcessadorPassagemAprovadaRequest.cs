using ConectCar.Transacoes.Domain.Model;
using ConectCar.Transacoes.Domain.ValueObject;
using System.Collections.Generic;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests
{
   public class ProcessadorPassagemAprovadaRequest
    {

        /// <summary>
        /// Passagem aprovada
        /// </summary>
        public PassagemAprovadaArtesp PassagemAprovadaArtesp { get; set; }

        public int CodigoProtocoloArtesp { get; set; }

    }
}
