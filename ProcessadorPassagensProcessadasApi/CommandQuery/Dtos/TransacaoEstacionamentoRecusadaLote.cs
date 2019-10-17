using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Dtos
{
    public class TransacaoEstacionamentoRecusadaLote
    {
        public int TipoTransacaoEstacionamento { get; set; }
        public int EstacionamentoErros { get; set; }
        public int MotivoAtrasoTransmissaoEstacionamento { get; set; }
        public int TempoPermamencia { get; set; }
        public string MotivoDesconto { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal ValorCobrado { get; set; }
        public DateTime DataHoraEntrada { get; set; }
        public DateTime DataHoraTransacao { get; set; }
        public int? PracaId { get; set; }
        public int? PistaId { get; set; }
        public int TagId { get; set; }
        public int ConveniadoId { get; set; }
        public DateTime DataCadastro { get; set; }
        public int Id { get; set; }
        public string Ticket { get; set; }
        public bool Mensalista { get; set; }
    }
}
