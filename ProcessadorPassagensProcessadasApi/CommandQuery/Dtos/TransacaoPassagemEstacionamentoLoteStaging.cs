using ConectCar.Transacoes.Domain.Enum;
using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Dtos
{
    public class TransacaoPassagemEstacionamentoLoteStaging
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public bool Credito { get; set; }
        public decimal Valor { get; set; }
        public int AdesaoId { get; set; }
        public int StatusId { get; set; }
        public int TipoOperacaoId { get; set; }
        public DateTime DataHoraTransacao { get; set; }
        public DateTime DataHoraEntrada { get; set; }
        public decimal ValorDesconto { get; set; }
        public string MotivoDesconto { get; set; }
        public int TempoPermanencia { get; set; }
        public decimal ValorRepasse { get; set; }
        public decimal TarifaDeInterconexao { get; set; }
        public DateTime DataRepasse { get; set; }
        public int? ConveniadoId { get; set; }
        public int? TagId { get; set; }
        public int? PracaId { get; set; }
        public int? PistaId { get; set; }
        public MotivoAtrasoTransmissaoEstacionamento MotivoAtrasoTransmissaoId { get; set; }
        public int? RepasseId { get; set; }
        public string SerieRPS { get; set; }
        public long NumeroRPS { get; set; }
        public DateTime DataReferencia { get; set; }
        public TipoTransacaoEstacionamento TipoTransacaoEstacionamentoId { get; set; }
        public string Ticket { get; set; }
        public bool Mensalista { get; set; }
        public long SurrogateKey { get; set; }
        public int SaldoId { get; set; }
    }
}