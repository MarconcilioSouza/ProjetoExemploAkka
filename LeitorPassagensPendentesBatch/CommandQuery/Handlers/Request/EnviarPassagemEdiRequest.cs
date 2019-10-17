using LeitorPassagensPendentesBatch.CommandQuery.Messages;
using System.Collections.Generic;

namespace LeitorPassagensPendentesBatch.CommandQuery.Handlers.Request
{
    public class EnviarPassagemEdiRequest
    {
        public List<PassagemPendenteMessageEdi> passagemPendenteMessageEdi { get; set; }
    }
}
