using LeitorPassagensPendentesBatch.CommandQuery.Messages;
using System.Collections.Generic;

namespace LeitorPassagensPendentesBatch.CommandQuery.Handlers.Request
{
    public class EnviarPassagensEdiFilter
    {
        public IList<PassagemPendenteMessageEdi> Passagens { get; set; }
        public EnviarPassagensEdiFilter(IList<PassagemPendenteMessageEdi> passagens)
        {
            Passagens = passagens;
        }
    }
}
