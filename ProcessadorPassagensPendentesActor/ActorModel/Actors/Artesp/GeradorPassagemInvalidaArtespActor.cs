using Akka.Actor;
using ProcessadorPassagensActors.ActorsMessages.Artesp;
using ProcessadorPassagensActors.ActorsUtils;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Responses;
using ProcessadorPassagensActors.Enums;

namespace ProcessadorPassagensActors.Actors.Artesp
{
    public class GeradorPassagemInvalidaArtespActor : BaseArtespActor<GeradorPassagemInvalidaArtespMessage, GeradorPassagemInvalidaResponse, GeradorPassagemInvalidaArtespHandler>
    {
        protected override void PreStart()
        {
            Workers = ActorsArtespCreator.CreateGeradorPassagemInvalidaChildrenActors(Context);
            FluxoAtual = ArtespActorsEnum.GeradorPassagemInvalidaActor;
        }

        protected override void ActorActivator(bool fake)
        {
            base.ActorActivator(fake);
            Workers[ArtespActorsEnum.ProcessadorPassagemInvalidaActor].Tell(fake);
        }

        protected override void ChamarRequisicao(GeradorPassagemInvalidaArtespMessage mensagem)
        {
            Response = Handler.Execute(new GeradorPassagemInvalidaRequest { PassagemPendenteArtesp = mensagem.PassagemPendenteArtesp });

            Workers[ArtespActorsEnum.ProcessadorPassagemInvalidaActor].Tell(new ProcessadorPassagemInvalidaArtespMessage
            {
                PassagemInvalidaArtesp = Response.PassagemInvalidaArtesp
            });
        }
    }
}