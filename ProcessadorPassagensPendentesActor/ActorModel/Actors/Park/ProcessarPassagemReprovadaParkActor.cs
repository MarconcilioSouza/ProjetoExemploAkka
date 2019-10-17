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
    public class ProcessarPassagemReprovadaParkActor : BaseParkActor<ProcessarPassagemReprovadaParkMessage, ProcessarPassagemReprovadaParkResponse, ProcessadorPassagemReprovadaParkHandler>
    {
        protected override void PreStart()
        {
            FluxoAtual = ParkActorsEnum.ProcessarPassagemReprovadaParkActor;
        }

        protected override void ChamarRequisicao(ProcessarPassagemReprovadaParkMessage mensagem)
        {
            try
            {
                Response = Handler.Execute(
                    new ProcessarPassagemReprovadaParkRequest
                    {
                        PassagemReprovadaEstacionamento = mensagem.PassagemReprovadaEstacionamento,
                    });
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(Exception))
                    throw new PassagemInvalidaException(mensagem.PassagemReprovadaEstacionamento.TransacaoEstacionamentoRecusada.RegistroTransacaoId, ex.Message, ex);
                throw;
            }
        }
    }
}