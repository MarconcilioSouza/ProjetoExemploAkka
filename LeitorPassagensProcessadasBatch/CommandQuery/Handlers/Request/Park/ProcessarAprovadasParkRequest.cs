using LeitorPassagensProcessadasBatch.CommandQuery.Messages.Park;
using System.Collections.Generic;

namespace LeitorPassagensProcessadasBatch.CommandQuery.Handlers.Request.Park
{
    public sealed class ProcessarAprovadasParkRequest
    {
        public IList<PassagemAprovadaParkMessage> Mensagens { get; set; }
        public int ConcessionariaId { get; set; }
        public ProcessarAprovadasParkRequest(IList<PassagemAprovadaParkMessage> mensagens)
            => Mensagens = mensagens;
    }
}
