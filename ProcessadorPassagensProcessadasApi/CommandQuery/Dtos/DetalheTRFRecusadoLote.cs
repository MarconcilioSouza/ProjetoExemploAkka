using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Dtos
{
    public class DetalheTRFRecusadoLote
    {
        public int Id { get; set; }
        public int ArquivoTRFId { get; set; }
        public int DetalheTRNId { get; set; }
        public int CodigoRetornoId { get; set; }
        public long SurrogateKey { get; set; }
    }
}
