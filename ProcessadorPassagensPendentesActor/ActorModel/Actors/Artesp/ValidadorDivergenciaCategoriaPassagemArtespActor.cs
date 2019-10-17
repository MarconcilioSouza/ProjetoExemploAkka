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
    public class ValidadorDivergenciaCategoriaPassagemArtespActor : BaseArtespActor<ValidadorDivergenciaCategoriaPassagemArtespMessage, ValidadorDivergenciaCategoriaPassagemResponse, ValidadorDivergenciaCategoriaPassagemArtespHandler>
    {
        protected override void PreStart()
        {
            Workers = ActorsArtespCreator.CreateValidadorDivergenciaCategoriaPassagemChildrenActors(Context);
            FluxoAtual = ArtespActorsEnum.ValidadorDivergenciaCategoriaPassagemActor;
        }

        protected override void ActorActivator(bool fake)
        {
            base.ActorActivator(fake);
            Workers[ArtespActorsEnum.GeradorPassagemAprovadaActor].Tell(fake);
        }

        protected override void ChamarRequisicao(ValidadorDivergenciaCategoriaPassagemArtespMessage mensagem)
        {
            try
            {
                Response = Handler.Execute(new ValidadorDivergenciaCategoriaPassagemRequest { PassagemPendenteArtesp = mensagem.PassagemPendenteArtesp });
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(Exception))
                    throw new PassagemInvalidaException(mensagem.PassagemPendenteArtesp.MensagemItemId, ex.Message, ex);
                throw;
            }

            Workers[ArtespActorsEnum.GeradorPassagemAprovadaActor].Tell(new GeradorPassagemAprovadaArtespMessage
            {
                PassagemPendenteArtesp = Response.PassagemPendenteArtesp,
                ViagensAgendadas = mensagem.ViagensAgendadas
            });
        }
    }
}