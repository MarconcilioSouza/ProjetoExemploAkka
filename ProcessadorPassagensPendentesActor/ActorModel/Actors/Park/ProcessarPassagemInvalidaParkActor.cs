using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProcessadorPassagensActors.ActorsMessages.Park;
using ProcessadorPassagensActors.CommandQuery.Handlers.Park;
using ProcessadorPassagensActors.CommandQuery.Handlers.Park.Response;

namespace ProcessadorPassagensActors.Actors.Park
{
    public class ProcessarPassagemInvalidaParkActor : BaseParkActor<ProcessarPassagemInvalidaParkMessage, ProcessarPassagemInvalidaParkResponse, ProcessarPassagemInvalidaParkHandler>
    {
    }
}