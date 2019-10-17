using System;
using AutoMapper;
using ProcessadorPassagensActors.ActorsMessages.Artesp;
using ProcessadorPassagensActors.CommandQuery.Exceptions;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Responses;
using ProcessadorPassagensActors.Enums;

namespace ProcessadorPassagensActors.Actors.Artesp
{
    public class ProcessadorPassagemAprovadaArtespActor : BaseArtespActor<ProcessadorPassagemAprovadaArtespMessage, EndResponse, ProcessadorPassagemAprovadaArtespHandler>
    {
        protected override void PreStart()
        {
            FluxoAtual = ArtespActorsEnum.ProcessadorPassagemAprovadaActor;
        }

        public ProcessadorPassagemAprovadaArtespActor()
        {
            Receive<GeradorPassagemProcessadaMensageriaMessage>(item =>
            {
                Processar(item, ChamarProcessadorPassagemAprovadaCompensadaPreviamente);
            });
        }        

        protected override void ChamarRequisicao(ProcessadorPassagemAprovadaArtespMessage mensagem)
        {
            try
            {
                Handler.Execute(new ProcessadorPassagemAprovadaRequest
                {
                    PassagemAprovadaArtesp = mensagem.PassagemAprovadaArtesp,
                    CodigoProtocoloArtesp = mensagem.CodigoProtocoloArtesp,
                });
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(Exception))
                    throw new PassagemInvalidaException(mensagem.PassagemAprovadaArtesp.MensagemItemId, ex.Message, ex);
                throw;
            }
           

           
        }


        private void ChamarProcessadorPassagemAprovadaCompensadaPreviamente(GeradorPassagemProcessadaMensageriaMessage mensagem)
        {
            try
            {
                Handler.Execute(new GeradorPassagemProcessadaMensageriaRequest(
                    mensagem.ResultadoPassagem,
                    mensagem.PassagemPendenteArtesp,
                    mensagem.TransacaoIdOriginal,
                    mensagem.ValorRepasse,
                    mensagem.DataPagamento,
                    mensagem.MotivoNaoCompensado,
                    mensagem.PassagemId
                ));
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(Exception))
                    throw new PassagemInvalidaException(mensagem.PassagemPendenteArtesp.MensagemItemId, ex.Message, ex);
                throw;
            }
        }

    }
}