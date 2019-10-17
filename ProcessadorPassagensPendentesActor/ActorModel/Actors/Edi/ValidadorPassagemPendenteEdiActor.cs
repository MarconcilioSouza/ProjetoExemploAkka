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
    public class ValidadorPassagemPendenteEdiActor : BaseEdiActor<ValidadorPassagemPendenteEdiMessage, ValidadorPassagemPendenteEdiResponse, ValidadorPassagemPendenteEdiHandler>
    {
        protected override void PreStart()
        {
            Workers = ActorsEdiCreator.CreateValidadorPassagemPendenteEdiChildrenActors(Context);
            FluxoAtual = EdiActorsEnum.ValidadorPassagemPendenteEdiActor;
        }


        protected override void ChamarRequisicao(ValidadorPassagemPendenteEdiMessage mensagem)
        {
            try
            {
                Response = Handler.Execute(
                        new ValidadorPassagemPendenteEdiRequest { PassagemPendenteEdi = mensagem.PassagemPendenteEdi });
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(Exception))
                    throw new PassagemInvalidaException(mensagem.PassagemPendenteEdi.DetalheTrnId, ex.Message, ex);
                throw;
            }

            Workers[EdiActorsEnum.GeradorPassagemEdiActor].Tell(new GeradorPassagemEdiMessage
            {
                PassagemPendenteEdi = mensagem.PassagemPendenteEdi
            });
        }
    }
}