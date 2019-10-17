using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.Enums;

namespace ProcessadorPassagensActors.ActorsMessages.Park
{
    public class CoordinatorParkMessage
    {
        /// <summary>
        /// Fluxo de execução
        /// </summary>
        public ParkActorsEnum FluxoExecucao { get; set; }

        /// <summary>
        /// Lista de passagens EDI
        /// </summary>
        public List<PassagemPendenteEstacionamento> PassagensPendentesEstacionamentos { get; set; }
    }
}