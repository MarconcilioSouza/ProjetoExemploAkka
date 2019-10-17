using System;
using Akka.Actor;
using ProcessadorPassagensActors.ActorsMessages.Artesp;
using ProcessadorPassagensActors.ActorsUtils;
using ProcessadorPassagensActors.CommandQuery.Exceptions;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Responses;
using ProcessadorPassagensActors.Enums;
using ConectCar.Transacoes.Domain.Enum;

namespace ProcessadorPassagensActors.Actors.Artesp
{
    public class ValidadorPassagemAceiteManualReenvioArtespActor : BaseArtespActor<ValidadorPassagemAceiteManualReenvioArtespMessage, ValidadorPassagemAceiteManualReenvioResponse, ValidadorPassagemAceiteManualReenvioArtespHandler>
    {
        protected override void PreStart()
        {
            Workers = ActorsArtespCreator.CreateValidadorPassagemAceiteManualReenvioChildrenActors(Context);
            FluxoAtual = ArtespActorsEnum.ValidadorPassagemAceiteManualReenvioActor;
        }

        protected override void ActorActivator(bool fake)
        {
            base.ActorActivator(fake);
            Workers[ArtespActorsEnum.ValidadorPassagemSistemaAceiteManualReenvioActor].Tell(fake);
        }

        protected override void ChamarRequisicao(ValidadorPassagemAceiteManualReenvioArtespMessage mensagem)
        {
            try
            {                
                Response = Handler.Execute(new ValidadorPassagemAceiteManualReenvioRequest { PassagemPendenteArtesp = mensagem.PassagemPendenteArtesp });
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(Exception))
                    throw new PassagemInvalidaException(mensagem.PassagemPendenteArtesp.MensagemItemId, ex.Message, ex);
                throw;
            }

            if(Response.MotivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
            {
                Workers[ArtespActorsEnum.ValidadorPassagemSistemaAceiteManualReenvioActor].Tell(new ValidadorPassagemSistemaAceiteManualReenvioArtespMessage
                {
                    PassagemPendenteArtesp = Response.PassagemPendenteArtesp
                });
            }
            else
            {
                var m = new GeradorPassagemReprovadaArtespMessage
                {
                    PassagemPendenteArtesp = Response.PassagemPendenteArtesp,
                    MotivoNaoCompensado = Response.MotivoNaoCompensado
                };

                Workers[ArtespActorsEnum.GeradorPassagemReprovadaActor].Tell(m);
            }

            
        }
    }
}