using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Dtos
{
    public class DetalheViagemLoteStaging
    {
        public Guid ExecucaoId { get; set; }
        public long? CodigoPracaRoadcard { get; set; }
        public DateTime? DataCancelamento { get; set; }
        public int? Id { get; set; }
        public int? StagingId { get; set; }
        public int? PracaId { get; set; }
        public long? Sequencia { get; set; }
        public int? StatusId { get; set; }
        public long? SurrogateKey { get; set; }
        public decimal? ValorPassagem { get; set; }
        public int? ViagemID { get; set; }
    }
}