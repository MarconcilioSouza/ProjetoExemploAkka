using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessadorPassagensActors.CommandQuery.Dtos
{
    public class HistoricoListaNelaDto
    {
        public int HistoricoListaNelaId { get; set; }
        public DateTime DataEntrada { get; set; }
        public DateTime? DataSaida { get; set; }

    }
}
