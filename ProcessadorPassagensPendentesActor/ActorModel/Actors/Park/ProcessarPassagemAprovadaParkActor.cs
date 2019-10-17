using ProcessadorPassagensActors.ActorsMessages.Park;
using ProcessadorPassagensActors.CommandQuery;
using ProcessadorPassagensActors.CommandQuery.Exceptions;
using ProcessadorPassagensActors.CommandQuery.Handlers.Park;
using ProcessadorPassagensActors.CommandQuery.Handlers.Park.Request;
using ProcessadorPassagensActors.CommandQuery.Handlers.Park.Response;
using ProcessadorPassagensActors.Enums;
using System;

namespace ProcessadorPassagensActors.Actors.Park
{
    public class ProcessarPassagemAprovadaParkActor : BaseParkActor<ProcessarPassagemAprovadaParkMessage, ProcessarPassagemAprovadaParkResponse, ProcessadorPassagemAprovadaParkHandler>
    {
        protected override void PreStart()
        {
            FluxoAtual = ParkActorsEnum.ProcessarPassagemAprovadaParkActor;
        }

        protected override void ChamarRequisicao(ProcessarPassagemAprovadaParkMessage mensagem)
        {
            try
            {
                Response = Handler.Execute(
                    new ProcessarPassagemAprovadaParkRequest
                    {
                        PassagemAprovadaEstacionamento = mensagem.PassagemAprovadaEstacionamento
                    });
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(Exception))
                    throw new PassagemInvalidaException(mensagem.PassagemAprovadaEstacionamento.Ticket.TryToInt(), ex.Message, ex);
                throw;
            }
        }
    }
}