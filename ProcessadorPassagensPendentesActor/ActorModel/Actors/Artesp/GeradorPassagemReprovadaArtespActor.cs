using System;
using Akka.Actor;
using ProcessadorPassagensActors.ActorsMessages.Artesp;
using ProcessadorPassagensActors.ActorsUtils;
using ProcessadorPassagensActors.CommandQuery.Exceptions;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Responses;
using ProcessadorPassagensActors.Enums;
using ProcessadorPassagensActors.CommandQuery;

namespace ProcessadorPassagensActors.Actors.Artesp
{
    public class GeradorPassagemReprovadaArtespActor : BaseArtespActor<GeradorPassagemReprovadaArtespMessage, GeradorPassagemReprovadaResponse, GeradorPassagemReprovadaArtespHandler>
    {
        protected override void PreStart()
        {
            Workers = ActorsArtespCreator.CreateGeradorPassagemReprovadaChildrenActors(Context);
            FluxoAtual = ArtespActorsEnum.GeradorPassagemReprovadaActor;
        }

        public GeradorPassagemReprovadaArtespActor()
        {
            Receive<GeradorPassagemReprovadaTransacaoReprovadaExistenteArtespMessage>(item =>
            {
                InitLog(item);
                Processar(item, ChamarGeradorTransacaoReprovadaExistente);
                FinishLog(item);                
            });

            Receive<GeradorPassagemPendenteReprovadaArtespMessage>(item =>
            {
                InitLog(item);
                Processar(item, ChamarGeradorPassagemPendenteReprovada);
                FinishLog(item);                
            });

            Receive<GeradorPassagemReprovadaDivergenciaCategoriaArtespArtespMessage>(item =>
            {
                InitLog(item);
                Processar(item, ChamarGeradorPassagemReprovadaDivergenciaCategoria);
                FinishLog(item);
            });

            Receive<GeradorPassagemReprovadaTransacaParceiroArtespMessage>(item =>
            {
                InitLog(item);
                Processar(item, GeradorPassagemReprovadaTransacaParceiro);
                FinishLog(item);
            });
        }

        protected override void ActorActivator(bool fake)
        {
            base.ActorActivator(fake);
            Workers[ArtespActorsEnum.ProcessadorPassagemReprovadaActor].Tell(fake);
        }

        private void InitLog<TItem>(TItem item)
        {
            _log.Debug($"{item} - Início Fluxo: {FluxoAtual.GetDescription()} - Ator: {Self.Path.Name} - {Self.Path.Uid}");
            _sw.Start();
        }

        private void FinishLog<TItem>(TItem item)
        {
            _sw.Stop();
            _log.Debug($"{item} - Fim Fluxo: {FluxoAtual.GetDescription()}. Tempo: {_sw.Elapsed}");
            _sw.Reset();
        }

        public void ChamarGeradorTransacaoReprovadaExistente(GeradorPassagemReprovadaTransacaoReprovadaExistenteArtespMessage mensagem)
        {
            try
            {
                Response = Handler.Execute(new GeradorPassagemPendenteReprovadaTransacaoReprovadaExistenteRequest
                {
                    PassagemPendenteArtesp = mensagem.PassagemPendenteArtesp,
                    TransacaoRecusada = mensagem.TransacaoRecusada
                });
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(Exception))
                    throw new PassagemInvalidaException(mensagem.PassagemPendenteArtesp.MensagemItemId, ex.Message, ex);
                throw;
            }

            Workers[ArtespActorsEnum.ProcessadorPassagemReprovadaActor].Tell(new ProcessadorPassagemReprovadaArtespMessage
            {
                PassagemReprovadaArtesp = Response.PassagemReprovadaArtesp
            });
        }

        public void ChamarGeradorPassagemPendenteReprovada(GeradorPassagemPendenteReprovadaArtespMessage mensagem)
        {
            try
            {
                Response = Handler.Execute(new GeradorPassagemPendenteReprovadaRequest
                {
                    PassagemPendenteArtesp = mensagem.PassagemPendenteArtesp,
                    MotivoNaoCompensado = mensagem.MotivoNaoCompensado
                });
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(Exception))
                    throw new PassagemInvalidaException(mensagem.PassagemPendenteArtesp.MensagemItemId, ex.Message, ex);
                throw;
            }
            Workers[ArtespActorsEnum.ProcessadorPassagemReprovadaActor].Tell(new ProcessadorPassagemReprovadaArtespMessage
            {
                PassagemReprovadaArtesp = Response.PassagemReprovadaArtesp
            });
        }

        public void ChamarGeradorPassagemReprovadaDivergenciaCategoria(GeradorPassagemReprovadaDivergenciaCategoriaArtespArtespMessage mensagem)
        {
            try
            {
                Response = Handler.Execute(new GeradorPassagemReprovadaDivergenciaCategoriaRequest
                {
                    PassagemPendenteArtesp = mensagem.PassagemPendenteArtesp,
                    MotivoNaoCompensado = mensagem.MotivoNaoCompensado
                });
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(Exception))
                    throw new PassagemInvalidaException(mensagem.PassagemPendenteArtesp.MensagemItemId, ex.Message, ex);
                throw;
            }

            Workers[ArtespActorsEnum.ProcessadorPassagemReprovadaActor].Tell(new ProcessadorPassagemReprovadaArtespMessage
            {
                PassagemReprovadaArtesp = Response.PassagemReprovadaArtesp
            });
        }

        public void GeradorPassagemReprovadaTransacaParceiro(GeradorPassagemReprovadaTransacaParceiroArtespMessage mensagem)
        {
            try
            {
                Response = Handler.Execute(new GeradorPassagemReprovadaTransacaParceiroArtespRequest
                {
                    PassagemPendenteArtesp = mensagem.PassagemPendenteArtesp,
                    MotivoNaoCompensado = mensagem.MotivoNaoCompensado,
                    DetalheViagemId = mensagem.DetalheViagemId
                });
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(Exception))
                    throw new PassagemInvalidaException(mensagem.PassagemPendenteArtesp.MensagemItemId, ex.Message, ex);
                throw;
            }

            Workers[ArtespActorsEnum.ProcessadorPassagemReprovadaActor].Tell(new ProcessadorPassagemReprovadaArtespMessage
            {
                PassagemReprovadaArtesp = Response.PassagemReprovadaArtesp
            });
        }


        protected override void ChamarRequisicao(GeradorPassagemReprovadaArtespMessage mensagem)
        {
            try
            {
                Response = Handler.Execute(new GeradorPassagemReprovadaRequest
                {
                    PassagemPendenteArtesp = mensagem.PassagemPendenteArtesp,
                    MotivoNaoCompensado = mensagem.MotivoNaoCompensado
                });
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(Exception))
                    throw new PassagemInvalidaException(mensagem.PassagemPendenteArtesp.MensagemItemId, ex.Message, ex);
                throw;
            }

            Workers[ArtespActorsEnum.ProcessadorPassagemReprovadaActor].Tell(new ProcessadorPassagemReprovadaArtespMessage
            {
                PassagemReprovadaArtesp = Response.PassagemReprovadaArtesp
            });
        }
    }
}