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
    /// <summary>
    /// Responsável por verificar se uma passagem já existe ou se uma transação já existe anterior.
    /// Caso não exista, redireciona para a ValidadorPassagemPendenteConcessionaria.
    /// </summary>
    public class ValidadorPassagemExistenteArtespActor : BaseArtespActor<ValidadorPassagemExistenteArtespMessage, ValidadorPassagemExistenteResponse, ValidadorPassagemExistenteArtespHandler>
    {

        protected override void PreStart()
        {
            Workers = ActorsArtespCreator.CreateValidadorPassagemExistenteChildrenActors(Context);
            FluxoAtual = ArtespActorsEnum.ValidadorPassagemExistenteActor;
        }

        protected override void ActorActivator(bool fake)
        {
            base.ActorActivator(fake);
            Workers[ArtespActorsEnum.ValidadorPassagemPendenteConcessionariaActor].Tell(fake);
        }

        protected override void ChamarRequisicao(ValidadorPassagemExistenteArtespMessage mensagem)
        {
            try
            {
                Response = Handler.Execute(new ValidadorPassagemExistenteRequest
                {
                    PassagemPendenteArtesp = mensagem.PassagemPendenteArtesp,
                    PassagemId = mensagem.PassagemId
                });
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(Exception))
                    throw new PassagemInvalidaException(mensagem.PassagemPendenteArtesp.MensagemItemId, ex.Message, ex);
                throw;
            }


            if(Response is ValidadorPassagemExistenteTransacaoRecusadaResponse)
            {
                var transacaoRecusadaResponse = Response as ValidadorPassagemExistenteTransacaoRecusadaResponse;

                var m = new GeradorPassagemReprovadaTransacaoReprovadaExistenteArtespMessage
                {
                    PassagemPendenteArtesp = transacaoRecusadaResponse.PassagemPendenteArtesp,
                    TransacaoRecusada = transacaoRecusadaResponse.TransacaoRecusada
                };

                Workers[ArtespActorsEnum.GeradorPassagemReprovadaActor].Tell(m);
            }
            else if (Response is ValidadorPassagemExistenteTransacaoPassagemResponse)
            {
                var transacaoPassagemResponse = Response as ValidadorPassagemExistenteTransacaoPassagemResponse;

                var m = new GeradorPassagemAprovadaComTransacaoExistenteArtespMessage
                {
                    TransacaoId = transacaoPassagemResponse.TransacaoId,
                    MensagemItemId = transacaoPassagemResponse.MensagemItemId,
                    CodigoProtocoloArtesp = transacaoPassagemResponse.CodigoProtocoloArtesp
                };

                Workers[ArtespActorsEnum.GeradorPassagemReprovadaActor].Tell(m);
            }
            else
            {
                Workers[ArtespActorsEnum.ValidadorPassagemPendenteConcessionariaActor].Tell(new ValidadorPassagemPendenteConcessionariaArtespMessage
                {
                    PassagemPendenteArtesp = Response.PassagemPendenteArtesp
                });
            }                            
        }
    }
}