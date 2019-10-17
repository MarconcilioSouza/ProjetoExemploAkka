using System;
using System.Collections.Generic;
using Akka.Actor;
using Common.Logging;
using ProcessadorPassagensActors.ActorsMessages.Edi;
using ProcessadorPassagensActors.ActorsUtils;
using ProcessadorPassagensActors.CommandQuery;
using ProcessadorPassagensActors.CommandQuery.Exceptions;
using ProcessadorPassagensActors.Enums;

namespace ProcessadorPassagensActors.Actors.Edi
{
    public class BaseEdiActor<TMessage, TResponse, THandler> : ReceiveActor where THandler : class, new()
    {

        #region Properties
        protected Dictionary<EdiActorsEnum, IActorRef> Workers;
        protected TResponse Response;
        protected Enum FluxoAtual;
        protected THandler Handler;
        private readonly ILog _log;
        #endregion

        #region Contrutor
        public BaseEdiActor()
        {
            _log = LogManager.GetLogger(GetType().FullName);
            Handler = new THandler();
            Receive<TMessage>(item =>
            {
                Processar(item, ChamarRequisicao);
            });
        }
        #endregion

        protected void Processar<TMessageProcessar>(TMessageProcessar mensagem, Action<TMessageProcessar> action)
        {
            try
            {
                action(mensagem);
            }
            catch (EdiTransacaoException etx)
            {
                var m = new GeradorPassagemReprovadaPorTransacaoExceptionEdiMessage
                {
                    PassagemPendenteEdi = etx.PassagemPendenteEdi,
                    CodigoRetornoTransacaoTrf = etx.CodigoRetornoTransacaoTrf
                };

                Workers[EdiActorsEnum.GeradorPassagemReprovadaEdiActor].Tell(m);

            }
            catch (EdiTransacaoParceiroException etpx)
            {
                var m = new GeradorPassagemReprovadaPorTransacaoParceiroExceptionEdiMessage
                {
                    PassagemPendenteEdi = etpx.PassagemPendenteEdi,
                    CodigoRetornoTransacaoTrf = etpx.CodigoRetornoTransacaoTrf,
                    DetalheViagemId = etpx.DetalheViagemId
                };

                Workers[EdiActorsEnum.GeradorPassagemReprovadaEdiActor].Tell(m);

            }
            catch (EdiDomainException etpx)
            {
                _log.Error($"Erro (EdiDomainException) ao processar Passagem TRN ({etpx.PassagemPendenteEdi.DetalheTrnId}):  {etpx.Message}");
            }
            catch (PassagemInvalidaException pie)
            {
                _log.Error($"Erro (PassagemException) ao processar Passagem TRN ({pie.ReferenceKey}):  {pie.Message}");
            }
        }

        protected virtual void ChamarRequisicao(TMessage mensagem)
        {
        }
    }
}