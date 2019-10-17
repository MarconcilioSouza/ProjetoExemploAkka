using ConectCar.Transacoes.Domain.Enum;
using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Dtos
{
    public class TransacaoEstacionamentoRecusadaLoteStaging
    {
        public int StagingId { get; set; }
        public DateTime DataCadastro { get; set; }
        public int? ConveniadoId { get; set; }
        public int? TagId { get; set; }
        public int? PistaId { get; set; }
        public int? PracaId { get; set; }
        public DateTime DataHoraTransacao { get; set; }
        public DateTime DataHoraEntrada { get; set; }
        public decimal ValorCobrado { get; set; }
        public decimal ValorDesconto { get; set; }
        public string MotivoDesconto { get; set; }
        public int TempoPermamencia { get; set; }
        public MotivoAtrasoTransmissaoEstacionamento MotivoAtrasoTransmissao { get; set; }
        public int MotivoRecusa { get; set; }
        public TipoTransacaoEstacionamento TipoTransacaoEstacionamento { get; set; }
        public string Ticket { get; set; }
        public bool Mensalista { get; set; }
        public long SurrogateKey { get; set; }
        public bool Falha { get; set; }
    }
}
