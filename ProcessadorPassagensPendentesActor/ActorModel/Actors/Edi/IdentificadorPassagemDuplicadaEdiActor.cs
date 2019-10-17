using Akka.Actor;
using ConectCar.Transacoes.Domain.Enum;
using ProcessadorPassagensActors.ActorsMessages.Edi;
using ProcessadorPassagensActors.ActorsUtils;
using ProcessadorPassagensActors.CommandQuery.Handlers.Edi;
using ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Request;
using ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Responses;
using ProcessadorPassagensActors.Enums;

namespace ProcessadorPassagensActors.Actors.Edi
{
    public class IdentificadorPassagemDuplicadaEdiActor : BaseEdiActor<IdentificadorPassagemDuplicadaEdiMessage, IdentificadorPassagemDuplicadaEdiResponse, IdentificadorPassagemDuplicadaEdiHandler>
    {
        protected override void PreStart()
        {
            Workers = ActorsEdiCreator.CreateIdentificadorPassagemChildrenActors(Context);
            FluxoAtual = ArtespActorsEnum.IdentificadorPassagemActor;
        }

        protected override void ChamarRequisicao(IdentificadorPassagemDuplicadaEdiMessage mensagem)
        {
            Response = Handler.Execute(
                new IdentificadorPassagemDuplicadaEdiRequest { PassagemPendenteEdi = mensagem.PassagemPendenteEdi });

            if (Response.PassagemDuplicada)
            {
                Workers[EdiActorsEnum.GeradorPassagemReprovadaEdiActor].Tell(new GeradorPassagemReprovadaPorTransacaoExceptionEdiMessage
                {
                    PassagemPendenteEdi = mensagem.PassagemPendenteEdi,
                    CodigoRetornoTransacaoTrf = CodigoRetornoTransacaoTRF.TransacaoRepetida
                });
            }
            else
            {
                Workers[EdiActorsEnum.ValidadorPassagemPendenteEdiActor].Tell(new ValidadorPassagemPendenteEdiMessage
                {
                    PassagemPendenteEdi = mensagem.PassagemPendenteEdi
                });
            }
        }
    }
}