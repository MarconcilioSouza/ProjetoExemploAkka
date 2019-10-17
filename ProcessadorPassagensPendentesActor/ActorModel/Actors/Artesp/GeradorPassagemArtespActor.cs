using System;
using Akka.Actor;
using ProcessadorPassagensActors.ActorsMessages.Artesp;
using ProcessadorPassagensActors.ActorsUtils;
using ProcessadorPassagensActors.CommandQuery.Exceptions;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Responses;
using ProcessadorPassagensActors.Enums;

namespace ProcessadorPassagensActors.Actors.Artesp
{
    public class GeradorPassagemArtespActor : BaseArtespActor<GeradorPassagemArtespMessage
        , GeradorPassagemResponse
        , GeradorPassagemArtespHandler>
    {

        protected override void PreStart()
        {
            Workers = ActorsArtespCreator.CreateGeradorPassagemChildrenActors(Context);
            FluxoAtual = ArtespActorsEnum.GeradorPassagemActor;
        }

        protected override void ActorActivator(bool fake)
        {
            base.ActorActivator(fake);
            Workers[ArtespActorsEnum.ValidadorPassagemAceiteManualReenvioActor].Tell(fake);
            Workers[ArtespActorsEnum.ValidadorPassagemActor].Tell(fake);
        }

        protected override void ChamarRequisicao(GeradorPassagemArtespMessage mensagem)
        {
            try
            {
                Response = Handler.Execute(new GeradorPassagemRequest { PassagemPendenteArtesp = mensagem.PassagemPendenteArtesp });
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(Exception))
                    throw new PassagemInvalidaException(mensagem.PassagemPendenteArtesp.MensagemItemId, ex.Message, ex);
                throw;
            }

            if (Response.PassagemPendenteArtesp.PossuiAceiteManualReenvioPassagem)
            {
                Workers[ArtespActorsEnum.ValidadorPassagemAceiteManualReenvioActor].Tell(new ValidadorPassagemAceiteManualReenvioArtespMessage
                {
                    PassagemPendenteArtesp = Response.PassagemPendenteArtesp
                });
            }
            else
            {
                Workers[ArtespActorsEnum.ValidadorPassagemActor].Tell(new ValidadorPassagemArtespMessage
                {
                    PassagemPendenteArtesp = Response.PassagemPendenteArtesp
                });
            }

            
        }
    }
}