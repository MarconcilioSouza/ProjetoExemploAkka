using ProcessadorPassagensActors.ActorsMessages.Artesp;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Responses;
using ProcessadorPassagensActors.Enums;

namespace ProcessadorPassagensActors.Actors.Artesp
{
    public class ProcessadorPassagemInvalidaArtespActor : BaseArtespActor<ProcessadorPassagemInvalidaArtespMessage, EndResponse, ProcessadorPassagemInvalidaArtespHandler>
    {
        protected override void PreStart()
        {
            FluxoAtual = ArtespActorsEnum.ProcessadorPassagemInvalidaActor;
        }

        protected override void ChamarRequisicao(ProcessadorPassagemInvalidaArtespMessage mensagem)
        {
            Handler.Execute(new ProcessadorPassagemInvalidaRequest { PassagemInvalidaArtesp = mensagem.PassagemInvalidaArtesp });
        }
    }
}