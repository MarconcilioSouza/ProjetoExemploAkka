using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Commands.Args
{
    public class AtualizarFalhaPassagensReprovadasArgs
    {
        public Guid ExecucaoId { get; set; }
        public string Motivo { get; set; }
    }
}
