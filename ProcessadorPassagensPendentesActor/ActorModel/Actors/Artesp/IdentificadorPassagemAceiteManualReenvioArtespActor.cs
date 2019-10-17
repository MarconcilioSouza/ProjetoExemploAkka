using Akka.Actor;
using ProcessadorPassagensActors.ActorsMessages.Artesp;
using ProcessadorPassagensActors.ActorsUtils;
using ProcessadorPassagensActors.CommandQuery.Exceptions;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Responses;
using ProcessadorPassagensActors.Enums;
using System;

namespace ProcessadorPassagensActors.Actors.Artesp
{
    /// <summary>
    /// Responsável por verificar se a passagem é reenvio e possui aceite manual.
    /// Caso possua, redireciona para o ValidadorPassagemPendenteAceiteManualReenvioArtesp
    /// Caso não possua ou não seja reenvio, redireciona para o ValidadorPassagemPendenteArtesp.
    /// </summary>
    public class IdentificadorPassagemAceiteManualReenvioArtespActor:
        BaseArtespActor<IdentificadorPassagemAceiteManualReenvioArtespMessage
            , IdentificadorPassagemAceiteManualReenvioResponse
            , IdentificadorPassagemAceiteManualReenvioArtespHandler>
    {
        protected override void PreStart()
        {
            Workers = ActorsArtespCreator.CreateIdentificadorPassagemAceiteManualReenvioChildrenActors(Context);
            FluxoAtual = ArtespActorsEnum.IdentificadorPassagemAceiteManualReenvioActor;
        }

        protected override void ActorActivator(bool fake)
        {
            base.ActorActivator(fake);
            Workers[ArtespActorsEnum.ValidadorPassagemPendenteAceiteManualReenvioActor].Tell(fake);
            Workers[ArtespActorsEnum.ValidadorPassagemPendenteActor].Tell(fake);
        }

        protected override void ChamarRequisicao(IdentificadorPassagemAceiteManualReenvioArtespMessage mensagem)
        {
            try
            {
                Response = Handler.Execute(new IdentificadorPassagemAceiteManualReenvioRequest { PassagemPendenteArtesp = mensagem.PassagemPendenteArtesp });
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(Exception))
                    throw new PassagemInvalidaException(mensagem.PassagemPendenteArtesp.MensagemItemId, ex.Message, ex);
                throw;
            }

            //Caso possua aceite manual reenvio, redireciona para atores específicos.
            if (Response.PassagemPendenteArtesp.PossuiAceiteManualReenvioPassagem)
            {
                Workers[ArtespActorsEnum.ValidadorPassagemPendenteAceiteManualReenvioActor].Tell(new ValidadorPassagemPendenteAceiteManualReenvioArtespMessage
                {
                    PassagemPendenteArtesp = mensagem.PassagemPendenteArtesp
                });
            }
            else
            {
                Workers[ArtespActorsEnum.ValidadorPassagemPendenteActor].Tell(new ValidadorPassagemPendenteArtespMessage
                {
                    PassagemPendenteArtesp = mensagem.PassagemPendenteArtesp
                });
            }
        }

    }
}