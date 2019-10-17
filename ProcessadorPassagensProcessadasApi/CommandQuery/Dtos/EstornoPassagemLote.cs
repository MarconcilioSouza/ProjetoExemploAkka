using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Dtos
{
    public class EstornoPassagemLote
    {
        public DateTime? DataEstorno { get; set; }
        public bool? SomenteInformacoesAlterada { get; set; }
        public int? TransacaoPassagemOriginalId { get; set; }
        public DateTime? Data { get; set; }
        public int? StatusId { get; set; }
        public int? Id { get; set; }
        public int? AdesaoId { get; set; }
        public int? TipoOperacaoId { get; set; }
        public bool? Credito { get; set; }
        public decimal? Valor { get; set; }
        public long? SurrogateKey { get; set; }
        public string ChaveCriptografiaBanco { get; set; }
        public int? SaldoId { get; set; }
        public int? StagingId { get; set; }    
    }
}
