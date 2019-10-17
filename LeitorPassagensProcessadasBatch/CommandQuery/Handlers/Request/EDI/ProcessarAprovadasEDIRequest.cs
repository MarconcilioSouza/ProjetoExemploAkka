using LeitorPassagensProcessadasBatch.CommandQuery.Messages.EDI;
using System.Collections.Generic;

namespace LeitorPassagensProcessadasBatch.CommandQuery.Handlers.Request.EDI
{
    public sealed class ProcessarAprovadasEdiRequest
    {
        public IList<PassagemAprovadaEdiMessage> Mensagens { get; set; }

        public ProcessarAprovadasEdiRequest(IList<PassagemAprovadaEdiMessage> mensagens)
        {
            Mensagens = mensagens;
        }
    }
}
