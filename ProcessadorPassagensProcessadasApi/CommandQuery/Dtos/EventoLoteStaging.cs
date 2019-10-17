using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Dtos
{
    public class EventoLoteStaging
    {
        public Guid ExecucaoId { get; set; }
        public int? Id { get; set; }
        public DateTime? DataCriacao { get; set; }
        public long? IdRegistro { get; set; }
        public bool? Processado { get; set; }
        public int? TipoEventoId { get; set; }
        public bool Falha { get; set; }
        public int? StagingId { get; set; }
    }
}
