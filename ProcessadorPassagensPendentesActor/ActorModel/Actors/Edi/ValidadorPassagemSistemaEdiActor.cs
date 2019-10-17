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
    public class ValidadorPassagemSistemaEdiActor : BaseEdiActor<ValidadorPassagemSistemaEdiMessage, ValidadorPassagemSistemaEdiActorResponse, ValidadorPassagemSistemaEdiHandler>
    {
        protected override void PreStart()
        {
            Workers = ActorsEdiCreator.CreateValidadorPassagemSistemaEdiChildrenActors(Context);
            FluxoAtual = EdiActorsEnum.ValidadorPassagemSistemaEdiActor;
        }


        protected override void ChamarRequisicao(ValidadorPassagemSistemaEdiMessage mensagem)
        {
            try
            {
                Response = Handler.Execute(
                        new ValidadorPassagemSistemaEdiActorRequest { PassagemPendenteEdi = mensagem.PassagemPendenteEdi });
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(Exception))
                    throw new PassagemInvalidaException(mensagem.PassagemPendenteEdi.DetalheTrnId, ex.Message, ex);
                throw;
            }

            Workers[EdiActorsEnum.ValidadorSlaListaNelaEdiActor].Tell(new ValidadorSlaListaNelaEdiMessage
            {
                PassagemPendenteEdi = mensagem.PassagemPendenteEdi,
            });
        }
    }
}