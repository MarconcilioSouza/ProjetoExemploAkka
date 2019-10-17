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
    public class GerarPassagemAprovadaParkActor : BaseParkActor<GerarPassagemAprovadaParkMessage, GerarPassagemAprovadaParkResponse, GerarPassagemAprovadaParkHandler>
    {
        protected override void PreStart()
        {
            Workers = ActorsParkCreator.CreateGerarPassagemAprovadaParkActorChildrenActors(Context);
            FluxoAtual = ParkActorsEnum.GerarPassagemAprovadaParkActor;
        }

        protected override void ChamarRequisicao(GerarPassagemAprovadaParkMessage mensagem)
        {
            try
            {
                Response = Handler.Execute(
                    new GerarPassagemAprovadaParkRequest { PassagemPendenteEstacionamento = mensagem.PassagemPendenteEstacionamento });
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(Exception))
                    throw new PassagemInvalidaException(mensagem.PassagemPendenteEstacionamento.Ticket.TryToInt(), ex.Message, ex);
                throw;
            }

            Workers[ParkActorsEnum.ProcessarPassagemAprovadaParkActor].Tell(new ProcessarPassagemAprovadaParkMessage
            {
                PassagemAprovadaEstacionamento = Response.PassagemAprovadaEstacionamento
            });
        }
    }
}