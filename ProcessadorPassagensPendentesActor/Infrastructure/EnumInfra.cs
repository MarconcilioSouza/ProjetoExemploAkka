using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessadorPassagensActors.Infrastructure
{
    public class EnumInfra
    {
        public enum ProtocolosEnum
        {
            PassagensPadrao = 0,

            PassagensAprovadasArtesp = 1,
            PassagensReprovadasArtesp = 2,
            PassagensInvalidasArtesp = 3,

            PassagensAprovadasEdi = 4,
            PassagensReprovadasEDI = 5,
            PassagensInvalidasEDI = 6,

            PassagensAprovadasPark = 7,
            PassagensReprovadasPark = 8,
            PassagensInvalidasPark = 9,
        }
    }
}
