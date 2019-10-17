using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Park.Response
{
    public class ValidarPassagemPendenteParkResponse
    {
        public PassagemPendenteEstacionamento PassagemPendenteEstacionamento { get; set; }
    }
}
