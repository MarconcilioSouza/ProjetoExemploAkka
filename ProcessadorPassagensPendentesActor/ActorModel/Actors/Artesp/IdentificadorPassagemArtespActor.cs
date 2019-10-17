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
    public class IdentificadorPassagemArtespActor : BaseArtespActor<IdentificadorPassagemArtespMessage, IdentificadorPassagemResponse, IdentificadorPassagemArtespHandler>
    {
        protected override void PreStart()
        {
            Workers = ActorsArtespCreator.CreateIdentificadorPassagemChildrenActors(Context);
            FluxoAtual = ArtespActorsEnum.IdentificadorPassagemActor;
        }

        protected override void ActorActivator(bool fake)
        {
            base.ActorActivator(fake);
            Workers[ArtespActorsEnum.ValidadorPassagemExistenteActor].Tell(fake);
            Workers[ArtespActorsEnum.ValidadorPassagemPendenteConcessionariaActor].Tell(fake);
        }

        protected override void ChamarRequisicao(IdentificadorPassagemArtespMessage mensagem)
        {
            try
            {                
                Response = Handler.Execute(new IdentificadorPassagemRequest { PassagemPendenteArtesp = mensagem.PassagemPendenteArtesp });
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(Exception))
                    throw new PassagemInvalidaException(mensagem.PassagemPendenteArtesp.MensagemItemId, ex.Message, ex);
                throw;
            }
            
            //Redireciona de acordo com a decisão do handler...
            if (Response.ExistePassagem())
            {
                Workers[ArtespActorsEnum.ValidadorPassagemExistenteActor].Tell(new ValidadorPassagemExistenteArtespMessage
                {
                    PassagemPendenteArtesp = mensagem.PassagemPendenteArtesp,
                    PassagemId = Response.PassagemId ?? 0
                });
            }
            else
            {
                Workers[ArtespActorsEnum.ValidadorPassagemPendenteConcessionariaActor].Tell(new ValidadorPassagemPendenteConcessionariaArtespMessage
                {
                    PassagemPendenteArtesp = mensagem.PassagemPendenteArtesp
                });
            }
        }

    }
}