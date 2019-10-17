using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessadorPassagensActors.CommandQuery.Queries.Filter
{
    public class ObterRepasseFilter
    {
        public int CodigoPista { get; }
        public long PracaId { get; }
        public long ConveniadoId { get; }
        public long PlanoId { get; }
        public DateTime DataPassagem { get; }

        public ObterRepasseFilter(int codigoPista, long pracaId, long conveniadoId, long planoId, DateTime dataPassagem)
        {
            CodigoPista = codigoPista;
            PracaId = pracaId;
            ConveniadoId = conveniadoId;
            PlanoId = planoId;
            DataPassagem = dataPassagem;
        }

        public ObterRepasseFilter(long planoId, DateTime dataPassagem)
        {
            PlanoId = planoId;
            DataPassagem = dataPassagem;
        }
    }
}
