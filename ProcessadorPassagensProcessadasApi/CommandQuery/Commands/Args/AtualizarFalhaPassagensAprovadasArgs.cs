﻿using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Commands.Args
{
    public class AtualizarFalhaPassagensAprovadasArgs
    {
        public Guid ExecucaoId { get; set; }
        public string Motivo { get; set; }
    }
}
