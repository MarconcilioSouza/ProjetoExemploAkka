using System;
using ProcessadorPassagensActors.ActorsMessages.Edi;
using ProcessadorPassagensActors.ActorsUtils;
using ProcessadorPassagensActors.CommandQuery.Exceptions;
using ProcessadorPassagensActors.CommandQuery.Handlers.Edi;
using ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Request;
using ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Responses;
using ProcessadorPassagensActors.Enums;

namespace ProcessadorPassagensActors.Actors.Edi
{
    public class ProcessadorPassagemReprovadaEdiActor : BaseEdiActor<ProcessadorPassagemReprovadaEdiMessage,
        ProcessadorPassagemReprovadaEdiResponse, ProcessadorPassagemReprovadaEdiHandler>
    {
        protected override void PreStart()
        {
            FluxoAtual = EdiActorsEnum.ProcessadorPassagemReprovadaEdiActor;
        }

        protected override void ChamarRequisicao(ProcessadorPassagemReprovadaEdiMessage mensagem)
        {
            try
            {
                Response = Handler.Execute(
                     new ProcessadorPassagemReprovadaEdiRequest
                     {
                         PassagemReprovadaEDI = mensagem.PassagemReprovadaEdi
                     });
            }
            catch (Exception ex)
            {
                throw new PassagemInvalidaException(mensagem.PassagemReprovadaEdi.DetalheTrnId, ex.Message, ex);
            }
        }
    }
}