using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConectCar.Framework.Infrastructure.Cqrs.Commands;
using ConectCar.Framework.Infrastructure.Log;
using ProcessadorPassagensActors.CommandQuery.Handlers.Park.Request;
using ProcessadorPassagensActors.CommandQuery.Handlers.Park.Response;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Park
{
  public  class ProcessarPassagemInvalidaParkHandler : Loggable, ICommand<ProcessarPassagemInvalidaParkRequest, ProcessarPassagemInvalidaParkResponse>
    {
        public ProcessarPassagemInvalidaParkResponse Execute(ProcessarPassagemInvalidaParkRequest args)
        {
            return new ProcessarPassagemInvalidaParkResponse();
        }
    }
}
