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
    public class ValidarPassagemPendenteParkActor : BaseParkActor<ValidarPassagemPendenteParkMessage, ValidarPassagemPendenteParkResponse, ValidarPassagemPendenteParkHandler>
    {
        protected override void PreStart()
        {
            Workers = ActorsParkCreator.CreateValidarPassagemPendenteParkActorChildrenActors(Context);
            FluxoAtual = ParkActorsEnum.ValidarPassagemPendenteParkActor;
        }

        protected override void ChamarRequisicao(ValidarPassagemPendenteParkMessage mensagem)
        {
            try
            {
                Response = Handler.Execute(
                    new ValidarPassagemPendenteParkRequest { PassagemPendenteEstacionamento = mensagem.PassagemPendenteEstacionamento });
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(Exception))
                    throw new PassagemInvalidaException(mensagem.PassagemPendenteEstacionamento.Ticket.TryToInt(), ex.Message, ex);
                throw;
            }

            Workers[ParkActorsEnum.GerarPassagemParkActor].Tell(new GerarPassagemParkMessage
            {
                PassagemPendenteEstacionamento = Response.PassagemPendenteEstacionamento
            });
        }
    }
}