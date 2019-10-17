using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessadorPassagensActors.CommandQuery.Queries.Filter
{
    public class ObterCountFechamentoPorConveniadoEDataFilter
    {
        public long ConveniadoId { get; set; }
        public DateTime DataReferenciaTransacao { get; set; }
        public bool DayChangeAposMeioDia { get; set; }
    }
}
