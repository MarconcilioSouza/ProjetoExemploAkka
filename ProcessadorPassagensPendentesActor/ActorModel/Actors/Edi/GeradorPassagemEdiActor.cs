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
    public class GeradorPassagemEdiActor : BaseEdiActor<GeradorPassagemEdiMessage, GeradorPassagemEdiResponse, GeradorPassagemEdiHandler>
    {
        protected override void PreStart()
        {
            Workers = ActorsEdiCreator.CreateGeradorPassagemEdiChildrenActors(Context);
            FluxoAtual = EdiActorsEnum.GeradorPassagemEdiActor;
        }

        protected override void ChamarRequisicao(GeradorPassagemEdiMessage mensagem)
        {
            try
            {
                Response = Handler.Execute(
                       new GeradorPassagemEdiRequest { PassagemPendenteEdi = mensagem.PassagemPendenteEdi });
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(Exception))
                    throw new PassagemInvalidaException(mensagem.PassagemPendenteEdi.DetalheTrnId, ex.Message, ex);
                throw;
            }

            Workers[EdiActorsEnum.ValidadorPassagemEdiActor].Tell(new ValidadorPassagemEdiMessage
            {
                PassagemPendenteEdi = mensagem.PassagemPendenteEdi
            });
        }
    }
}