using System.Collections.Generic;
using LeitorPassagensProcessadasBatch.CommandQuery.Messages.Artesp;

namespace LeitorPassagensProcessadasBatch.CommandQuery.Handlers.Request.Artesp
{
    public class ProcessarReprovadasArtespRequest
    {
        public IList<PassagemReprovadaArtespMessage> Mensagens { get; set; }
        public int ConcessionariaId { get; }

        public ProcessarReprovadasArtespRequest(IList<PassagemReprovadaArtespMessage> mensagens)
        {
            Mensagens = mensagens;
        }
    }
}
