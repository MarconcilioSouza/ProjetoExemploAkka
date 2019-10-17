using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessadorPassagensActors.CommandQuery.Queries.Filter
{
    public class ObterSePossuiIncidentesFilter
    {
        public int PracaId { get; set; }
        public int PistaId { get; set; }
        public DateTime DataPassagem { get; set; }
        
    }
}
