using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Dtos
{
    public class EventoLote
    {
        public int? Id { get; set; }
        public DateTime? DataCriacao { get; set; }
        public long? IdRegistro { get; set; }
        public bool? Processado { get; set; }
        public int? TipoEventoId { get; set; }

    }
}
