﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessadorPassagensActors.CommandQuery.Dtos
{
    public class UltimaTransacaoPassagemDto
    {
        public long Id { get; set; }
        public int CategoriaUtilizadaId { get; set; }
        public int StatusPassagemId { get; set; }
    }
}
