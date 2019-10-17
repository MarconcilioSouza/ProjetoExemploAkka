using System;
using System.Collections.Generic;
using Akka.Actor;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.ActorsMessages.Artesp;
using ProcessadorPassagensActors.ActorsUtils;
using ProcessadorPassagensActors.CommandQuery;
using ProcessadorPassagensActors.CommandQuery.Exceptions;
using ProcessadorPassagensActors.Enums;
using Common.Logging;
using System.Diagnostics;

namespace ProcessadorPassagensActors.Actors.Artesp
{
    public class BaseArtespActor<TMessage, TResponse, THandler> : ReceiveActor where THandler : class, new()
    {

        #region Properties
        protected Dictionary<ArtespActorsEnum, IActorRef> Workers;
        protected TResponse Response;
        protected Enum FluxoAtual;
        protected THandler Handler;
        protected readonly ILog _log;
        protected Stopwatch _sw;
        #endregion

        #region Ctor
        public BaseArtespActor()
        {
            _log = LogManager.GetLogger(GetType().FullName);
            _sw = new Stopwatch();

            try
            {
                Handler = new THandler();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            


            Receive<bool>(m=> {
                ActorActivator(m);
            });


            Receive<TMessage>(item =>
            {
                _log.Debug($"{item} - Início Fluxo Ator: {Self.Path.Name} - {Self.Path.Uid}");
                _sw.Start();

                Processar(item, ChamarRequisicao);

                _sw.Stop();                
                _log.Debug($"{item} - Fim Fluxo Ator: {Self.Path.Name} - {Self.Path.Uid}. Tempo: {_sw.Elapsed}");
                _sw.Reset();
            });
        }
        #endregion

        /// <summary>
        /// Responsável por inicializar cada ator com uma mensagem fake
        /// </summary>
        /// <param name="fake">Parametro para ativar cada ator.</param>
        protected virtual void ActorActivator(bool fake)
        {
            _log.Debug($"Ativação do Ator {FluxoAtual.GetDescription()}.");
        }
        

        protected void Processar<TMessageProcessar>(TMessageProcessar mensagem, Action<TMessageProcessar> action)
        {
            try
            {
                action(mensagem);
            }
            catch (PassagemDivergenciaCategoriaException ex)
            {
                var m = new GeradorPassagemReprovadaDivergenciaCategoriaArtespArtespMessage
                {
                    PassagemPendenteArtesp = ex.PassagemPendenteArtesp,
                    MotivoNaoCompensado = ex.MotivoNaoCompensado
                };


                Workers[ArtespActorsEnum.GeradorPassagemReprovadaActor].Tell(m);
                //var mensagemItemId = ex.PassagemPendenteArtesp.MensagemItemId;
                //var metadata = new ActorMetaData($"GeradorPassagemReprovadaArtespActor{mensagemItemId}", $"akka://TransacaoArtesp/user/coordinator/GeradorPassagemReprovadaActor_{mensagemItemId}");
                //var actorRef = Context.ActorOf(Props.Create(() => new GeradorPassagemReprovadaArtespActor()), metadata.Name);

                //actorRef.Tell(m);
            }
            catch (PassagemReprovadaException ex)
            {
                var m = new GeradorPassagemReprovadaArtespMessage
                {
                    PassagemPendenteArtesp = ex.PassagemPendenteArtesp,
                    MotivoNaoCompensado = ex.MotivoNaoCompensado
                };

                Workers[ArtespActorsEnum.GeradorPassagemReprovadaActor].Tell(m);
                //var mensagemItemId = ex.PassagemPendenteArtesp.MensagemItemId;
                //var metadata = new ActorMetaData($"GeradorPassagemReprovadaArtespActor{mensagemItemId}", $"akka://TransacaoArtesp/user/coordinator/GeradorPassagemReprovadaActor_{mensagemItemId}");
                //var actorRef = Context.ActorOf(Props.Create(() => new GeradorPassagemReprovadaArtespActor()), metadata.Name);

                //actorRef.Tell(m);
            }
            catch (PassagemPendenteReprovadaException ex)
            {
                var m = new GeradorPassagemPendenteReprovadaArtespMessage
                {
                    PassagemPendenteArtesp = ex.PassagemPendenteArtesp,
                    MotivoNaoCompensado = ex.MotivoNaoCompensado
                };

                Workers[ArtespActorsEnum.GeradorPassagemReprovadaActor].Tell(m);
                //var mensagemItemId = ex.PassagemPendenteArtesp.MensagemItemId;
                //var metadata = new ActorMetaData($"GeradorPassagemReprovadaArtespActor{mensagemItemId}", $"akka://TransacaoArtesp/user/coordinator/GeradorPassagemReprovadaActor_{mensagemItemId}");
                //var actorRef = Context.ActorOf(Props.Create(() => new GeradorPassagemReprovadaArtespActor()), metadata.Name);

                //actorRef.Tell(m);
            }
            catch (TransacaoPassagemExistenteException ex)
            {
                var m = new GeradorPassagemAprovadaComTransacaoExistenteArtespMessage
                {
                    TransacaoId = ex.TransacaoId,
                    MensagemItemId = ex.MensagemItemId,
                    CodigoProtocoloArtesp = ex.CodigoProtocoloArtesp
                };

                Workers[ArtespActorsEnum.GeradorPassagemReprovadaActor].Tell(m);
                //var mensagemItemId = ex.MensagemItemId;
                //var metadata = new ActorMetaData($"GeradorPassagemReprovadaArtespActor{mensagemItemId}", $"akka://TransacaoArtesp/user/coordinator/GeradorPassagemReprovadaActor_{mensagemItemId}");
                //var actorRef = Context.ActorOf(Props.Create(() => new GeradorPassagemReprovadaArtespActor()), metadata.Name);

                //actorRef.Tell(m);
            }
            catch (TransacaoReprovadaExistenteException ex)
            {
                var m = new GeradorPassagemReprovadaTransacaoReprovadaExistenteArtespMessage
                {
                    PassagemPendenteArtesp = ex.PassagemPendenteArtesp,
                    TransacaoRecusada = ex.TransacaoRecusada
                };



                Workers[ArtespActorsEnum.GeradorPassagemReprovadaActor].Tell(m);
                //var mensagemItemId = ex.PassagemPendenteArtesp.MensagemItemId;
                //var metadata = new ActorMetaData($"GeradorPassagemReprovadaArtespActor{mensagemItemId}", $"akka://TransacaoArtesp/user/coordinator/GeradorPassagemReprovadaActor_{mensagemItemId}");
                //var actorRef = Context.ActorOf(Props.Create(() => new GeradorPassagemReprovadaArtespActor()),metadata.Name);

                //actorRef.Tell(m);
            }
            catch (PassagemInvalidaException ex)
            {
                _log.Warn($"Passagem Inválida {ex.ReferenceKey} - {ex.Message} ",ex.InnerException);

                var m = new GeradorPassagemInvalidaArtespMessage
                {
                    PassagemPendenteArtesp = new PassagemPendenteArtesp
                    {
                        MensagemItemId = ex.ReferenceKey
                    }
                };

                Workers[ArtespActorsEnum.GeradorPassagemInvalidaActor].Tell(m);
            }
            catch (PassagemInvalidaNoSysException tpcpx)
            {
                var m = new GeradorPassagemProcessadaMensageriaMessage(
                    tpcpx.ResultadoPassagem,
                    tpcpx.PassagemPendenteArtesp,
                    tpcpx.TransacaoIdOriginal,
                    tpcpx.ValorRepasse,
                    tpcpx.DataPagamento,
                    tpcpx.MotivoNaoCompensado,
                    tpcpx.PassagemId);

                Workers[ArtespActorsEnum.ProcessadorPassagemAprovadaActor].Tell(m);
            }
            catch (TransacaoParceiroException tpx)
            {
                var m = new GeradorPassagemReprovadaTransacaParceiroArtespMessage
                {
                    DetalheViagemId = tpx.DetalheViagemId,
                    MotivoNaoCompensado = tpx.MotivoNaoCompensado,
                    PassagemPendenteArtesp = tpx.PassagemPendente
                };
                Workers[ArtespActorsEnum.GeradorPassagemReprovadaActor].Tell(m);
            }
            catch (Exception ex)
            {
                _log.Fatal(ex.Message, ex);
            }

        }

        protected virtual void ChamarRequisicao(TMessage mensagem)
        {
        }

    }
}