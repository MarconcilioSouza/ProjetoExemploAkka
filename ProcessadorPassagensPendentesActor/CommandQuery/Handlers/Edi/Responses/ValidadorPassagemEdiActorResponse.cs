﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConectCar.Transacoes.Domain.Model;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Responses
{
  public  class ValidadorPassagemEdiActorResponse
    {
        public PassagemPendenteEDI PassagemPendenteEdi { get; set; }
    }
}
