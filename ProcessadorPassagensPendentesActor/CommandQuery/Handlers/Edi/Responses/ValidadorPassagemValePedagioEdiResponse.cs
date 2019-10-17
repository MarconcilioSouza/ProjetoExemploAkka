using System;
using System.Collections.Generic;
using ConectCar.Transacoes.Domain.Model;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Responses
{
  public  class ValidadorPassagemValePedagioEdiResponse
    {
        public PassagemPendenteEDI PassagemPendenteEdi { get; set; }
        public List<DetalheViagem> DetalheViagens { get; set; }
    }
}
