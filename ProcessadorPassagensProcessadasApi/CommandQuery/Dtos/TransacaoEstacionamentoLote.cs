using System;
using System.Collections.Generic;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Dtos
{
    public class TransacaoEstacionamentoLote
    {
        public DateTime DataReferencia { get; set; }
        public int TipoTransmissaoEstacionamentoId { get; set; }
        public int MotivoAtrasoTransmissaoId { get; set; }
        public long NumeroRPS { get; set; }
        public string SerieRPS { get; set; }
        public int TempoPermanencia { get; set; }
        public string MotivoDesconto { get; set; }
        public decimal ValorDesconto { get; set; }
        public DateTime DataHoraEntrada { get; set; }
        public DateTime DataHoraTransacao { get; set; }
        public int PracaId { get; set; }
        public int PistaId { get; set; }
        public int TagId { get; set; }
        public int ConveniadoId { get; set; }
        public List<DetalhePassagemEstacionamentoLote> Detalhes { get; set; }
        public string Ticket { get; set; }
        public bool Mensalista { get; set; }
        public long SurrogateKey { get; set; }
    }
}
