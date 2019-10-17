using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Enums;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Park.Request
{
   public class GerarPassagemReprovadaParkRequest
    {
        public PassagemPendenteEstacionamento PassagemPendenteEstacionamento { get; set; }
        public EstacionamentoErros Erro { get; set; }
    }
}
