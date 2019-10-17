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
    public class GerarPassagemReprovadaParkActor : BaseParkActor<GerarPassagemReprovadaParkMessage, GerarPassagemReprovadaParkResponse, GerarPassagemReprovadaParkHandler>
    {
        protected override void PreStart()
        {
            Workers = ActorsParkCreator.CreateGerarPassagemReprovadaParkActorChildrenActors(Context);
            FluxoAtual = ParkActorsEnum.GerarPassagemReprovadaParkActor;
        }

        protected override void ChamarRequisicao(GerarPassagemReprovadaParkMessage mensagem)
        {
            try
            {
                Response = Handler.Execute(
                    new GerarPassagemReprovadaParkRequest
                    {
                        PassagemPendenteEstacionamento = mensagem.PassagemPendenteEstacionamento,
                        Erro = mensagem.Erro
                    });
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(Exception))
                    throw new PassagemInvalidaException(mensagem.PassagemPendenteEstacionamento.Ticket.TryToInt(), ex.Message, ex);
                throw;
            }

            Workers[ParkActorsEnum.ProcessarPassagemReprovadaParkActor].Tell(new ProcessarPassagemReprovadaParkMessage
            {
                PassagemReprovadaEstacionamento = Response.PassagemReprovadaEstacionamento
            });
        }
    }
}