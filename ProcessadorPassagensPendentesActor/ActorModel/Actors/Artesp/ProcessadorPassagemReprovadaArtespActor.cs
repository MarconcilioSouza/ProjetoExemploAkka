using ProcessadorPassagensActors.ActorsMessages.Artesp;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Responses;
using ProcessadorPassagensActors.Enums;

namespace ProcessadorPassagensActors.Actors.Artesp
{
    public class ProcessadorPassagemReprovadaArtespActor : BaseArtespActor<ProcessadorPassagemReprovadaArtespMessage, EndResponse, ProcessadorPassagemReprovadaArtespHandler>
    {
        protected override void PreStart()
        {
            FluxoAtual = ArtespActorsEnum.ProcessadorPassagemReprovadaActor;
        }
        protected override void ChamarRequisicao(ProcessadorPassagemReprovadaArtespMessage mensagem)
        {
            Handler.Execute(new ProcessadorPassagemReprovadaRequest { PassagemReprovadaArtesp = mensagem.PassagemReprovadaArtesp });
        }
    }
}