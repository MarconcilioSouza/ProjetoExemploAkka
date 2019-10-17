using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.ActorsMessages.Park
{
    public class ProcessarPassagemAprovadaParkMessage
    {
        public PassagemAprovadaEstacionamento PassagemAprovadaEstacionamento { get; set; }
    }
}