using LeitorPassagensProcessadasBatch.CommandQuery.Messages.EDI;
using System.Collections.Generic;

namespace LeitorPassagensProcessadasBatch.CommandQuery.Handlers.Request.EDI
{
    public sealed class ProcessarReprovadasEdiRequest
    {
        public IList<PassagemReprovadaEdiMessage> Mensagens { get; set; }
        public ProcessarReprovadasEdiRequest(IList<PassagemReprovadaEdiMessage> mensagens)
            => Mensagens = mensagens;
    }
}
