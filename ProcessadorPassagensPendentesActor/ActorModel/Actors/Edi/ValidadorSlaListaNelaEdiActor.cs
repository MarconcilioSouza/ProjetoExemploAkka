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
    public class ValidadorSlaListaNelaEdiActor : BaseEdiActor<ValidadorSlaListaNelaEdiMessage, ValidadorSlaListaNelaEdiResponse, ValidadorSlaListaNelaEdiHandler>
    {
        protected override void PreStart()
        {
            Workers = ActorsEdiCreator.CreateValidadorSlaListaNelaEdiChildrenActors(Context);
            FluxoAtual = EdiActorsEnum.ValidadorSlaListaNelaEdiActor;
        }


        protected override void ChamarRequisicao(ValidadorSlaListaNelaEdiMessage mensagem)
        {
            try
            {
                Response = Handler.Execute(
                        new ValidadorSlaListaNelaEdiRequest { PassagemPendenteEdi = mensagem.PassagemPendenteEdi });
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(Exception))
                    throw new PassagemInvalidaException(mensagem.PassagemPendenteEdi.DetalheTrnId, ex.Message, ex);
                throw;
            }

            Workers[EdiActorsEnum.ValidadorDivergenciaCategoriaEdiActor].Tell(new ValidadorDivergenciaCategoriaEdiMessage
            {
                PassagemPendenteEdi = mensagem.PassagemPendenteEdi,
            });
        }
    }
}