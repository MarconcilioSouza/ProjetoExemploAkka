using LeitorPassagensProcessadasBatch.CommandQuery.Messages.Park;
using System.Collections.Generic;

namespace LeitorPassagensProcessadasBatch.CommandQuery.Handlers.Request.Park
{
    public sealed class ProcessarReprovadasParkRequest
    {
        public IList<PassagemReprovadaParkMessage> Mensagens { get; set; }
        public int ConcessionariaId { get; }

        public ProcessarReprovadasParkRequest(IList<PassagemReprovadaParkMessage> mensagens)
            => Mensagens = mensagens;
    }
}
