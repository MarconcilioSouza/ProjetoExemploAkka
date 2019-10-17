using LeitorPassagensPendentesBatch.CommandQuery.Messages;
using System.Collections.Generic;

namespace LeitorPassagensPendentesBatch.CommandQuery.Handlers.Request
{
    public class EnviarPassagensParkFilter
    {
        public IList<PassagemPendenteMessagePark> Passagens { get; set; }
        public EnviarPassagensParkFilter(IList<PassagemPendenteMessagePark> passagens)
        {
            Passagens = passagens;
        }
    }
}
