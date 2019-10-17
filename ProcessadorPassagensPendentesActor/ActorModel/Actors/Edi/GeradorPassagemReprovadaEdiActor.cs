using Akka.Actor;
using ProcessadorPassagensActors.ActorsMessages.Edi;
using ProcessadorPassagensActors.ActorsUtils;
using ProcessadorPassagensActors.CommandQuery.Handlers.Edi;
using ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Request;
using ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Responses;
using ProcessadorPassagensActors.Enums;

namespace ProcessadorPassagensActors.Actors.Edi
{
    public class GeradorPassagemReprovadaEdiActor : BaseEdiActor<GeradorPassagemReprovadaPorTransacaoExceptionEdiMessage, GeradorPassagemReprovadaEdiResponse, GeradorPassagemReprovadaEdiHandler>
    {
        protected override void PreStart()
        {
            Workers = ActorsEdiCreator.CreateGeradorPassagemReprovadaEdiChildrenActors(Context);
            FluxoAtual = EdiActorsEnum.GeradorPassagemReprovadaEdiActor;
        }

        public GeradorPassagemReprovadaEdiActor()
        {
            Receive<GeradorPassagemReprovadaPorTransacaoParceiroExceptionEdiMessage>(item =>
            {
                Processar(item, ChamarRequisicaoTransacaoParceiroException);
            });
        }

        private void ChamarRequisicaoTransacaoParceiroException(GeradorPassagemReprovadaPorTransacaoParceiroExceptionEdiMessage mensagem)
        {
            Response = Handler.Execute(
                new GeradorPassagemReprovadaPorTransacaoParceiroExceptionEdiRequest
                {
                    PassagemPendenteEdi = mensagem.PassagemPendenteEdi,
                    CodigoRetornoTransacaoTrf = mensagem.CodigoRetornoTransacaoTrf,
                    DetalheViagemId = mensagem.DetalheViagemId
                });

            Workers[EdiActorsEnum.ProcessadorPassagemReprovadaEdiActor].Tell(new ProcessadorPassagemReprovadaEdiMessage
            {
                PassagemReprovadaEdi = Response.PassagemReprovadaEdi
            });
        }

        protected override void ChamarRequisicao(GeradorPassagemReprovadaPorTransacaoExceptionEdiMessage mensagem)
        {
            Response = Handler.Execute(
                new GeradorPassagemReprovadaPorTransacaoExceptionEdiRequest
                {
                    PassagemPendenteEdi = mensagem.PassagemPendenteEdi,
                    CodigoRetornoTransacaoTrf = mensagem.CodigoRetornoTransacaoTrf
                });

            Workers[EdiActorsEnum.ProcessadorPassagemReprovadaEdiActor].Tell(new ProcessadorPassagemReprovadaEdiMessage
            {
                PassagemReprovadaEdi = Response.PassagemReprovadaEdi
            });
        }
    }
}