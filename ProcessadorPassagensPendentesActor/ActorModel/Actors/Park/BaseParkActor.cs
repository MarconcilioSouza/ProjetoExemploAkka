using Akka.Actor;
using ProcessadorPassagensActors.ActorsMessages.Park;
using ProcessadorPassagensActors.ActorsUtils;
using ProcessadorPassagensActors.CommandQuery.Exceptions;
using ProcessadorPassagensActors.Enums;
using System;
using ProcessadorPassagensActors.CommandQuery;
using System.Collections.Generic;

namespace ProcessadorPassagensActors.Actors.Park
{
    public class BaseParkActor<TMessage, TResponse, THandler> : ReceiveActor where THandler : class, new()
    {

        #region Properties
        protected Dictionary<ParkActorsEnum, IActorRef> Workers;
        protected TResponse Response;
        protected Enum FluxoAtual;
        protected THandler Handler;
        private readonly ActorLogger _log = new ActorLogger();
        #endregion

        #region Contrutor
        public BaseParkActor()
        {
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
            catch (PassagemInvalidaException pie)
            {
                _log.Error($"Erro (PassagemException) ao processar Passagem park ({pie.ReferenceKey}):  {pie.Message}");
            }
            catch (ParkException pke)
            {
                var msgErro = pke.Erro.GetDescription();
                _log.Info($"(reprovada) - Passagem RegistroTransacaoId: {pke.PassagemPendenteEstacionamento.RegistroTransacaoId} | {msgErro} - {pke.Message}.");

                var m = new GerarPassagemReprovadaParkMessage
                {
                    PassagemPendenteEstacionamento = pke.PassagemPendenteEstacionamento,
                    Erro = pke.Erro
                    
                };

                Workers[ParkActorsEnum.GerarPassagemReprovadaParkActor].Tell(m);
            }
            catch(ParkDomainException pde)
            {
                _log.Error($"Erro (ParkException) ao processar Passagem RegistroTransacaoId: {pde.PassagemPendenteEstacionamento.RegistroTransacaoId} | {pde.Message}");
            }
            catch(Exception e)
            {
                _log.Error($"Erro (ParkException): {e.Message}");
            }
        }

        protected virtual void ChamarRequisicao(TMessage mensagem)
        {
        }
    }
}