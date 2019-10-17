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
    public class ValidadorPassagemSistemaArtespActor : BaseArtespActor<ValidadorPassagemSistemaArtespMessage, ValidadorPassagemSistemaResponse, ValidadorPassagemSistemaArtespHandler>
    {
        protected override void PreStart()
        {
            Workers = ActorsArtespCreator.CreateValidadorPassagemSistemaChildrenActors(Context);
            FluxoAtual = ArtespActorsEnum.ValidadorPassagemSistemaActor;
        }

        protected override void ActorActivator(bool fake)
        {
            base.ActorActivator(fake);
            Workers[ArtespActorsEnum.ValidadorPassagemValePedagioActor].Tell(fake);
        }

        protected override void ChamarRequisicao(ValidadorPassagemSistemaArtespMessage mensagem)
        {
            try
            {
                Response = Handler.Execute(new ValidadorPassagemSistemaRequest { PassagemPendenteArtesp = mensagem.PassagemPendenteArtesp });
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(Exception))
                    throw new PassagemInvalidaException(mensagem.PassagemPendenteArtesp.MensagemItemId, ex.Message, ex);
                throw;
            }


            if(Response.MotivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
            {
                Workers[ArtespActorsEnum.ValidadorPassagemValePedagioActor].Tell(new ValidadorPassagemValePedagioArtespMessage
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