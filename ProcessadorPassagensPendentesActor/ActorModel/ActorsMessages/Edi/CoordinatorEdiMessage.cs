using System.Collections.Generic;
using ConectCar.Transacoes.Domain.Model;
using ProcessadorPassagensActors.Enums;

namespace ProcessadorPassagensActors.ActorsMessages.Edi
{
    public class CoordinatorEdiMessage
    {
        /// <summary>
        /// Fluxo de execução
        /// </summary>
        public EdiActorsEnum FluxoExecucao { get; set; }

        /// <summary>
        /// Lista de passagens EDI
        /// </summary>
        public List<PassagemPendenteEDI> PassagensPendentesEdi { get; set; }
    }
}