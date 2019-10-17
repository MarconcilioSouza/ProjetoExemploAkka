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
    public class GeradorPassagemAprovadaArtespActor : BaseArtespActor<GeradorPassagemAprovadaArtespMessage, GeradorPassagemAprovadaResponse, GeradorPassagemAprovadaArtespHandler>
    {
        protected override void PreStart()
        {
            Workers = ActorsArtespCreator.CreateGeradorPassagemAprovadaChildrenActors(Context);
            FluxoAtual = ArtespActorsEnum.GeradorPassagemAprovadaActor;
        }

        public GeradorPassagemAprovadaArtespActor()
        {
            Receive<GeradorPassagemAprovadaComTransacaoExistenteArtespMessage>(item =>
            {
                Processar(item, ChamarGeradorPassagemAprovadaComTransacaoExistente);
            });
        }

        protected override void ActorActivator(bool fake)
        {
            base.ActorActivator(fake);
            Workers[ArtespActorsEnum.ProcessadorPassagemAprovadaActor].Tell(fake);
        }

        protected override void ChamarRequisicao(GeradorPassagemAprovadaArtespMessage mensagem)
        {
            try
            {
                Response = Handler.Execute(new GeradorPassagemAprovadaRequest
                {
                    PassagemPendenteArtesp = mensagem.PassagemPendenteArtesp,
                    ViagensAgendadas = mensagem.ViagensAgendadas

                });
            }
            catch (Exception ex)
            {
                if(ex.GetType() == typeof(Exception))
                    throw new PassagemInvalidaException(mensagem.PassagemPendenteArtesp.MensagemItemId, ex.Message, ex);
                throw;
            }

            Workers[ArtespActorsEnum.ProcessadorPassagemAprovadaActor].Tell(new ProcessadorPassagemAprovadaArtespMessage
            {
                PassagemAprovadaArtesp = Response.PassagemAprovadaArtesp,
                CodigoProtocoloArtesp = Response.CodigoProtocoloArtesp,
            });
        }

        private void ChamarGeradorPassagemAprovadaComTransacaoExistente(GeradorPassagemAprovadaComTransacaoExistenteArtespMessage mensagem)
        {
            try
            {
                Response = Handler.Execute(new GeradorPassagemAprovadaComTransacaoExistenteRequest(mensagem.TransacaoId, mensagem.CodigoProtocoloArtesp));
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(Exception))
                    throw new PassagemInvalidaException(mensagem.MensagemItemId, ex.Message, ex);
                throw;
            }

            Workers[ArtespActorsEnum.ProcessadorPassagemAprovadaActor].Tell(new ProcessadorPassagemAprovadaArtespMessage
            {
                PassagemAprovadaArtesp = Response.PassagemAprovadaArtesp,
                CodigoProtocoloArtesp = Response.CodigoProtocoloArtesp
            });

        }




    }
}