using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Dtos
{
    public class DetalheTransacaoEstacionamentoRecusadaLote
    {
        public int? Pista { get; set; }
        public int? Praca { get; set; }
        public DateTime DataHoraPassagem { get; set; }
        public int? TransacaoEstacionamentoRecusada { get; set; }
    }
}
