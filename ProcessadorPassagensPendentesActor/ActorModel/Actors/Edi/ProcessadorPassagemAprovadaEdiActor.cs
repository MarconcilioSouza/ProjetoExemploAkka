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
    public class ProcessadorPassagemAprovadaEdiActor : BaseEdiActor<ProcessadorPassagemAprovadaEdiMessage, ProcessadorPassagemAprovadaEdiResponse, ProcessadorPassagemAprovadaEdiHandler>
    {
        protected override void PreStart()
        {
            FluxoAtual = EdiActorsEnum.ValidadorPassagemPendenteEdiActor;
        }

        protected override void ChamarRequisicao(ProcessadorPassagemAprovadaEdiMessage mensagem)
        {
            try
            {
                Handler.Execute(new ProcessadorPassagemAprovadaEdiRequest
                {
                    PassagemAprovadaEdi = mensagem.PassagemAprovadaEdi,
                    DetalheViagens = mensagem.DetalheViagens,
                    DetalheTrfRecusado = mensagem.DetalheTrfRecusado,
                    Evento = mensagem.Evento
                });
            }
            catch (Exception ex)
            {
                throw new PassagemInvalidaException(mensagem.PassagemAprovadaEdi.DetalheTrnId, ex.Message, ex);
            }

        }
    }
}