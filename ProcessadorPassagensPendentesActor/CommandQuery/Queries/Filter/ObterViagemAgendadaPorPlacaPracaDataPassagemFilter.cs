using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessadorPassagensActors.CommandQuery.Queries.Filter
{
   public class ObterViagemAgendadaPorPlacaPracaDataPassagemFilter
    {
        public string Placa { get; set; }
        public long? PracaId { get; set; }
        public DateTime DataPassagem { get; set; }
        public long? ViagemId { get; set; }
    }
}
