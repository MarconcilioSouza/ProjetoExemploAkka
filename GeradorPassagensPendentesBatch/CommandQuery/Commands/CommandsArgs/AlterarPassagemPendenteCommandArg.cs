using System;

namespace GeradorPassagensPendentesBatch.CommandQuery.Commands.CommandsArgs
{
    public class AlterarPassagemPendenteCommandArg
    {
        public DateTime DataTtl { get; set; }
        public long MensagemItemIdMin { get; set; }
        public long MensagemItemIdMax { get; set; }
    }
}
