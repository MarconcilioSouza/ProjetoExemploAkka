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
    public class ValidarPassagemParkActor : BaseParkActor<ValidarPassagemParkMessage, ValidarPassagemParkResponse, ValidarPassagemParkHandler>
    {
        protected override void PreStart()
        {
            Workers = ActorsParkCreator.CreateValidarPassagemParkActorChildrenActors(Context);
            FluxoAtual = ParkActorsEnum.ValidarPassagemParkActor;
        }

        protected override void ChamarRequisicao(ValidarPassagemParkMessage mensagem)
        {
            try
            {
                Response = Handler.Execute(
                    new ValidarPassagemParkRequest { PassagemPendenteEstacionamento = mensagem.PassagemPendenteEstacionamento });
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(Exception))
                    throw new PassagemInvalidaException(mensagem.PassagemPendenteEstacionamento.Ticket.TryToInt(), ex.Message, ex);
                throw;
            }

            Workers[ParkActorsEnum.ValidarPassagemSistemaParkActor].Tell(new ValidarPassagemSistemaParkMessage
            {
                PassagemPendenteEstacionamento = Response.PassagemPendenteEstacionamento
            });
        }
    }
}