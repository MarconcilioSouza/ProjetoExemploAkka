using System.Collections.Generic;
using LeitorPassagensProcessadasBatch.CommandQuery.Messages.Artesp;

namespace LeitorPassagensProcessadasBatch.CommandQuery.Handlers.Request.Artesp
{
    public class ProcessarAprovadasArtespRequest
    {
        public IList<PassagemAprovadaArtespMessage> Mensagens { get; set; }
        public int ConcessionariaId { get; set; }
        public ProcessarAprovadasArtespRequest(IList<PassagemAprovadaArtespMessage> mensagens)
        {
            Mensagens = mensagens;
        }
    }
}
