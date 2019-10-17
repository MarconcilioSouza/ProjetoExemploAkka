using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessadorPassagensActors.CommandQuery.Queries.Filter
{
    public class ObterHistoricoListaNelaFilter
    {
        public long TagId { get; set; }
        public DateTime DataDePassagem { get; set; }
    }
}
