using System.Collections.Generic;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.Enums;

namespace ProcessadorPassagensActors.ActorsMessages.Artesp
{
    public class CoordinatorArtespMessage
    {
        /// <summary>
        /// Fluxo de execução
        /// </summary>
        public ArtespActorsEnum FluxoExecucao { get; set; }

        ///// <summary>
        ///// Lista de passagens Artesp
        ///// </summary>
        //public List<PassagemPendenteArtesp> PassagensPendentesArtesp { get; set; }

        /// <summary>
        /// Passagens Artesp
        /// </summary>
        public PassagemPendenteArtesp PassagemPendenteArtesp { get; set; }
    }
}