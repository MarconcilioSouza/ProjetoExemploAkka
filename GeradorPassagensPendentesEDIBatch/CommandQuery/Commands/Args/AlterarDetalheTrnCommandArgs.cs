using System;

namespace GeradorPassagensPendentesEDIBatch.CommandQuery.Commands
{
    public class AlterarDetalheTrnCommandArgs
    {
        public DateTime DataTtl { get; set; }
        public long DetalheTrnIdMin { get; set; }
        public long DetalheTrnIdMax { get; set; }
    }
}