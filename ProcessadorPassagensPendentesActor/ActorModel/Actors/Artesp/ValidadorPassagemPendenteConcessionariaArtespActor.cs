using Akka.Actor;
using ConectCar.Transacoes.Domain.Enum;
using ProcessadorPassagensActors.ActorsMessages.Artesp;
using ProcessadorPassagensActors.ActorsUtils;
using ProcessadorPassagensActors.CommandQuery.Exceptions;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Responses;
using ProcessadorPassagensActors.Enums;
using System;

namespace ProcessadorPassagensActors.Actors.Artesp
{
    /// <summary>
    /// Validador responsável por verificar se a passagem possui informações de conveniado 
    /// e identificador da passagem no conveniado preenchidos.
    /// </summary>
    public class ValidadorPassagemPendenteConcessionariaArtespActor: 
        BaseArtespActor<ValidadorPassagemPendenteConcessionariaArtespMessage,
            ValidadorPassagemPendenteConcessionariaResponse,
            ValidadorPassagemPendenteConcessionariaArtespHandler>
    {


        protected override void PreStart()
        {
            Workers = ActorsArtespCreator.CreateValidadorPassagemPendenteConcessionariaChildrenActors(Context);
            FluxoAtual = ArtespActorsEnum.ValidadorPassagemPendenteConcessionariaActor;
        }

        protected override void ActorActivator(bool fake)
        {
            base.ActorActivator(fake);
            Workers[ArtespActorsEnum.IdentificadorPassagemAceiteManualReenvioActor].Tell(fake);
        }

        protected override void ChamarRequisicao(ValidadorPassagemPendenteConcessionariaArtespMessage mensagem)
        {
            try
            {
                Response = Handler.Execute(new ValidadorPassagemPendenteConcessionariaRequest { PassagemPendenteArtesp = mensagem.PassagemPendenteArtesp });
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(Exception))
                    throw new PassagemInvalidaException(mensagem.PassagemPendenteArtesp.MensagemItemId, ex.Message, ex);
                throw;
            }

            if(Response.MotivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
            {
                Workers[ArtespActorsEnum.IdentificadorPassagemAceiteManualReenvioActor].Tell(new IdentificadorPassagemAceiteManualReenvioArtespMessage
                {
                    PassagemPendenteArtesp = mensagem.PassagemPendenteArtesp
                });
            }
            else
            {
                var m = new GeradorPassagemPendenteReprovadaArtespMessage
                {
                    PassagemPendenteArtesp = Response.PassagemPendenteArtesp,
                    MotivoNaoCompensado = Response.MotivoNaoCompensado
                };

                Workers[ArtespActorsEnum.GeradorPassagemReprovadaActor].Tell(m);
            }

            
        }
    }
}