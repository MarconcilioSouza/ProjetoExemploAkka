using LeitorPassagensPendentesBatch.CommandQuery.Messages;
using System.Collections.Generic;

namespace LeitorPassagensPendentesBatch.CommandQuery.Handlers.Request
{
    public class EnviarPassagemParkRequest
    {
        public List<PassagemPendenteMessagePark> passagemPendenteMessagePark { get; set; }
    }
}
