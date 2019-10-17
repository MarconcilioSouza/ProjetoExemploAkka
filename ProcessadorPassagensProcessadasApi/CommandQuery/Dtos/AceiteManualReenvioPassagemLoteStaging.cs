
using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Dtos
{
    public class AceiteManualReenvioPassagemLoteStaging : AceiteManualReenvioPassagemLote
    {
        public Guid ExecucaoId { get; set; }
    }
}