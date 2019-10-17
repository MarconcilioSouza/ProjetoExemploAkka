using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Dtos
{
    public class SolicitacaoImagemLoteStaging: SolicitacaoImagemLote
    {
        public Guid ExecucaoId { get; set; }
    }
}
