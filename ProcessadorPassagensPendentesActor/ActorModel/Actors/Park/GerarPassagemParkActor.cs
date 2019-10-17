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
    public class GerarPassagemParkActor : BaseParkActor<GerarPassagemParkMessage, GerarPassagemParkResponse, GerarPassagemParkHandler>
    {
        protected override void PreStart()
        {
            Workers = ActorsParkCreator.CreateGerarPassagemParkChildrenActors(Context);
            FluxoAtual = ParkActorsEnum.GerarPassagemParkActor;
        }

        protected override void ChamarRequisicao(GerarPassagemParkMessage mensagem)
        {
            try
            {
                Response = Handler.Execute(
                    new GerarPassagemParkRequest { PassagemPendenteEstacionamento = mensagem.PassagemPendenteEstacionamento });
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(Exception))
                    throw new PassagemInvalidaException(mensagem.PassagemPendenteEstacionamento.Ticket.TryToInt(), ex.Message, ex);
                throw;
            }

            Workers[ParkActorsEnum.ValidarPassagemParkActor].Tell(new ValidarPassagemParkMessage
            {
                PassagemPendenteEstacionamento = Response.PassagemPendenteEstacionamento
            });
        }
    }
}