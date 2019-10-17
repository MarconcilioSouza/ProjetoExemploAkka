using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Park.Request
{
    public class ValidarPassagemParkRequest
    {
        public PassagemPendenteEstacionamento PassagemPendenteEstacionamento { get; set; }
    }
}
