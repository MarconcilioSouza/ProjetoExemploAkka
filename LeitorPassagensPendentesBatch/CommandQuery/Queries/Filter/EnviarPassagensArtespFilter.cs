using System.Collections.Generic;
using LeitorPassagensPendentesBatch.CommandQuery.Messages;

namespace LeitorPassagensPendentesBatch.CommandQuery.Handlers.Request
{
    public class EnviarPassagensArtespFilter
    {
        public IList<PassagemPendenteMessageArtesp> Passagens { get; set; }
        public EnviarPassagensArtespFilter(IList<PassagemPendenteMessageArtesp> passagens)
        {
            Passagens = passagens;
        }
    }
}
