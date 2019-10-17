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
    public class ValidadorPassagemEdiActor : BaseEdiActor<ValidadorPassagemEdiMessage, ValidadorPassagemEdiActorResponse, ValidadorPassagemEdiHandler>
    {
        protected override void PreStart()
        {
            Workers = ActorsEdiCreator.CreateValidadorPassagemEdiChildrenActors(Context);
            FluxoAtual = EdiActorsEnum.ValidadorPassagemEdiActor;
        }


        protected override void ChamarRequisicao(ValidadorPassagemEdiMessage mensagem)
        {
            try
            {
                Response = Handler.Execute(
                       new ValidadorPassagemEdiActorRequest { PassagemPendenteEdi = mensagem.PassagemPendenteEdi });
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(Exception))
                    throw new PassagemInvalidaException(mensagem.PassagemPendenteEdi.DetalheTrnId, ex.Message, ex);
                throw;
            }

            Workers[EdiActorsEnum.ValidadorPassagemSistemaEdiActor].Tell(new ValidadorPassagemSistemaEdiMessage
            {
                PassagemPendenteEdi = mensagem.PassagemPendenteEdi
            });
        }
    }
}