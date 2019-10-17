using System.Collections.Generic;
using LeitorPassagensProcessadasBatch.CommandQuery.Messages.Artesp;

namespace LeitorPassagensProcessadasBatch.CommandQuery.Handlers.Request.Artesp
{
    public class ProcessarInvalidasArtespRequest
    {
        public IList<PassagemInvalidaArtespMessage> Mensagens { get; set; }
        public ProcessarInvalidasArtespRequest(IList<PassagemInvalidaArtespMessage> mensagens)
        {
            Mensagens = mensagens;
        }
    }
}
