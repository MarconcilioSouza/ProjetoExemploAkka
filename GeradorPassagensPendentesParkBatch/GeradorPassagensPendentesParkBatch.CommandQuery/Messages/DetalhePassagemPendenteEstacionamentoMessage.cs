using System;

namespace GeradorPassagensPendentesParkBatch.CommandQuery.Messages
{
    public class DetalhePassagemPendenteEstacionamentoMessage
    {
        public int RegistroTransacaoId { get; set; }
        public int? CodigoConveniado { get; set; }
        public long? Tag { get; set; }
        public int? Praca { get; set; }
        public int? Pista { get; set; }
        public DateTime DataHoraTransacao { get; set; }
        public DateTime DataHoraEntrada { get; set; }
        public decimal ValorCobrado { get; set; }
        public decimal ValorDesconto { get; set; }
        public string MotivoDesconto { get; set; }
        public int TempoPermanencia { get; set; }
        public int MotivoAtrasoTransmissao { get; set; }
        public int? TipoTransacao { get; set; }
        public string Ticket { get; set; }
        public bool Mensalista { get; set; }
        public int? PistaDetalhe { get; set; }
        public int? PracaDetalhe { get; set; }
        public DateTime DataHoraPassagem { get; set; }
    }
}
