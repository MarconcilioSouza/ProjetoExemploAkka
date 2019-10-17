using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Dtos
{
    public class ExtratoLoteStaging
    {
        public Guid ExecucaoId { get; set; }
        public int? AdesaoId { get; set; }
        public string ChaveCriptografiaBanco { get; set; }
        public DateTime? DataTransacao { get; set; }
        public string Descricao { get; set; }
        public long? Id { get; set; }
        public string Placa { get; set; }
        public string SubDescricao { get; set; }
        public int? SurrogateKey { get; set; }
        public int? TipoOperacaoId { get; set; }
        public int? TransacaoId { get; set; }
        public decimal? ValorD { get; set; }
        public int? StagingId { get; set; }
    }
}
