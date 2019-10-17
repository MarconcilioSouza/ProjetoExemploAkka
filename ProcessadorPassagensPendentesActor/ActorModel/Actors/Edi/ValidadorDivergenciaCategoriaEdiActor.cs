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
    public class ValidadorDivergenciaCategoriaEdiActor : BaseEdiActor<ValidadorDivergenciaCategoriaEdiMessage, ValidadorDivergenciaCategoriaEdiResponse, ValidadorDivergenciaCategoriaEdiHandler>
    {
        protected override void PreStart()
        {
            Workers = ActorsEdiCreator.CreateValidadorDivergenciaCategoriaEdiChildrenActors(Context);
            FluxoAtual = EdiActorsEnum.ValidadorDivergenciaCategoriaEdiActor;
        }


        protected override void ChamarRequisicao(ValidadorDivergenciaCategoriaEdiMessage mensagem)
        {
            try
            {
                Response = Handler.Execute(
                        new ValidadorDivergenciaCategoriaEdiRequest { PassagemPendenteEdi = mensagem.PassagemPendenteEdi });
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(Exception))
                    throw new PassagemInvalidaException(mensagem.PassagemPendenteEdi.DetalheTrnId, ex.Message, ex);
                throw;
            }

            Workers[EdiActorsEnum.ValidadorPassagemValePedagioEdiActor].Tell(new ValidadorPassagemValePedagioEdiMessage
            {
                PassagemPendenteEdi = mensagem.PassagemPendenteEdi,
            });
        }
    }
}