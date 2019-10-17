using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Enums;

namespace ProcessadorPassagensActors.ActorsMessages.Park
{
    public class GerarPassagemReprovadaParkMessage
    {
        public PassagemPendenteEstacionamento PassagemPendenteEstacionamento { get; set; }
        public EstacionamentoErros Erro { get; set; }
    }
}