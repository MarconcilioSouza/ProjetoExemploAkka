using System;
using Akka.Actor;
using ProcessadorPassagensActors.ActorsMessages.Edi;
using ProcessadorPassagensActors.ActorsUtils;
using ProcessadorPassagensActors.CommandQuery.Exceptions;
using ProcessadorPassagensActors.CommandQuery.Handlers.Edi;
using ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Request;
using ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Responses;
using ProcessadorPassagensActors.Enums;

namespace ProcessadorPassagensActors.Actors.Edi
{
    public class GeradorPassagemAprovadaEdiActor : BaseEdiActor<GeradorPassagemAprovadaEdiMessage, GeradorPassagemAprovadaEdiResponse, GeradorPassagemAprovadaEdiHandler>
    {
        protected override void PreStart()
        {
            Workers = ActorsEdiCreator.CreateGeradorPassagemAprovadaEdiChildrenActors(Context);
            FluxoAtual = EdiActorsEnum.GeradorPassagemAprovadaEdiActor;
        }

        protected override void ChamarRequisicao(GeradorPassagemAprovadaEdiMessage mensagem)
        {
            try
            {
                Response = Handler.Execute(
                      new GeradorPassagemAprovadaEdiRequest { PassagemPendenteEdi = mensagem.PassagemPendenteEdi });
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(Exception))
                    throw new PassagemInvalidaException(mensagem.PassagemPendenteEdi.DetalheTrnId, ex.Message, ex);
                throw;
            }

            Workers[EdiActorsEnum.ProcessadorPassagemAprovadaEdiActor].Tell(new ProcessadorPassagemAprovadaEdiMessage
            {
                PassagemAprovadaEdi = Response.PassagemAprovadaEdi,
                DetalheViagens = mensagem.DetalheViagens,
                Evento = Response.Evento,
                DetalheTrfRecusado = Response.DetalheTrfRecusado
            });
        }
    }
}