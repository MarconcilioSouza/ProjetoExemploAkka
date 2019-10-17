using System;

namespace GeradorPassagensPendentesParkBatch.CommandQuery.Commands
{
    public class AlterarDetalheParkCommandArgs
    {
        public DateTime DataTtl { get; set; }
        public long RegistroTransacaoIdMin { get; set; }
        public long RegistroTransacaoIdMax { get; set; }
    }
}