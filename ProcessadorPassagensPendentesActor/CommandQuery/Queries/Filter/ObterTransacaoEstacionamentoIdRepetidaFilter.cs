using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessadorPassagensActors.CommandQuery.Queries.Filter
{
    public class ObterTransacaoEstacionamentoIdRepetidaFilter
    {
        public int TagId { get; set; }
        public int PracaId { get; set; }
        public int PistaId { get; set; }
        public int ConveniadoId { get; set; }
        public DateTime DataHoraEntrada { get; set; }
        public DateTime DataHoraTransacao { get; set; }
        public int TempoPermanencia { get; set; }
        
    }
}
