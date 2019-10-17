using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessadorPassagensActors.CommandQuery.Dtos
{
    public class DetalheViagemAgendadaDto
    {
        public int? TransacaoPassagemId { get; set; }
        public int PracaId { get; set; }
        public long CodigoPracaRoadCard { get; set; }
        public long Sequencia { get; set; }
        public decimal ValorPassagem { get; set; }
        public DateTime? DataCancelamento { get; set; }
        public int StatusId { get; set; }
        public int ViagemId { get; set; }
        public int DetalheViagemId { get; set; }
        public int Id { get; set; }
        public string Embarcador { get; set; }
        public long CnpjEmbarcador { get; set; }
        public long CodigoViagemParceiro { get; set; }
    }
}
