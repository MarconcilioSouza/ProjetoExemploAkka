using Akka.Actor;
using ProcessadorPassagensActors.ActorsMessages.Park;
using ProcessadorPassagensActors.ActorsUtils;
using ProcessadorPassagensActors.CommandQuery;
using ProcessadorPassagensActors.CommandQuery.Exceptions;
using ProcessadorPassagensActors.CommandQuery.Handlers.Park;
using ProcessadorPassagensActors.CommandQuery.Handlers.Park.Request;
using ProcessadorPassagensActors.CommandQuery.Handlers.Park.Response;
using ProcessadorPassagensActors.Enums;
using System;

namespace ProcessadorPassagensActors.Actors.Park
{
    public class ValidarPassagemSistemaParkActor : BaseParkActor<ValidarPassagemSistemaParkMessage, ValidarPassagemSistemaParkResponse, ValidarPassagemSistemaParkHandler>
    {
        protected override void PreStart()
        {
            Workers = ActorsParkCreator.CreateValidarPassagemSistemaParkActorChildrenActors(Context);
            FluxoAtual = ParkActorsEnum.ValidarPassagemSistemaParkActor;
        }

        protected override void ChamarRequisicao(ValidarPassagemSistemaParkMessage mensagem)
        {
            try
            {
                Response = Handler.Execute(
                    new ValidarPassagemSistemaParkRequest { PassagemPendenteEstacionamento = mensagem.PassagemPendenteEstacionamento });
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(Exception))
                    throw new PassagemInvalidaException(mensagem.PassagemPendenteEstacionamento.Ticket.TryToInt(), ex.Message, ex);
                throw;
            }

            Workers[ParkActorsEnum.GerarPassagemAprovadaParkActor].Tell(new GerarPassagemAprovadaParkMessage
            {
                PassagemPendenteEstacionamento = Response.PassagemPendenteEstacionamento
            });
        }
    }
}