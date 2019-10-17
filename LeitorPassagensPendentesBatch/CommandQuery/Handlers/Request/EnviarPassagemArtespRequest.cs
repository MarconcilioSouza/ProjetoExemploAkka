using LeitorPassagensPendentesBatch.CommandQuery.Messages;
using System.Collections.Generic;

namespace LeitorPassagensPendentesBatch.CommandQuery.Handlers.Request
{
    public class EnviarPassagemArtespRequest
    {
        public List<PassagemPendenteMessageArtesp> passagemPendenteMessageArtesp { get; set; }
    }
}
