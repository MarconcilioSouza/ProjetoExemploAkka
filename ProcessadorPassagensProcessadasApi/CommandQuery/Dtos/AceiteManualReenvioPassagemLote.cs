using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Dtos
{
    public class AceiteManualReenvioPassagemLote
    {
        public int? Id { get; set; }
        public bool? Processado { get; set; }
        public DateTime? DataProcessamento { get; set; }

    }
}
