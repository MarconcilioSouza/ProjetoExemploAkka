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
    public class ValidadorPassagemPendenteArtespActor : BaseArtespActor<ValidadorPassagemPendenteArtespMessage
        , ValidadorPassagemPendenteResponse
        , ValidadorPassagemPendenteArtespHandler>
    {
        protected override void PreStart()
        {
            Workers = ActorsArtespCreator.CreateValidadorPassagemPendenteChildrenActors(Context);
            FluxoAtual = ArtespActorsEnum.ValidadorPassagemPendenteActor;
        }

        protected override void ActorActivator(bool fake)
        {
            base.ActorActivator(fake);
            Workers[ArtespActorsEnum.GeradorPassagemActor].Tell(fake);
        }

        protected override void ChamarRequisicao(ValidadorPassagemPendenteArtespMessage mensagem)
        {
            try
            {
                Response = Handler.Execute(new ValidadorPassagemPendenteRequest { PassagemPendenteArtesp = mensagem.PassagemPendenteArtesp });
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(Exception))
                    throw new PassagemInvalidaException(mensagem.PassagemPendenteArtesp.MensagemItemId, ex.Message, ex);
                throw;
            }
            

            if(Response.MotivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
            {
                Workers[ArtespActorsEnum.GeradorPassagemActor].Tell(new GeradorPassagemArtespMessage
                {
                    PassagemPendenteArtesp = Response.PassagemPendenteArtesp
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