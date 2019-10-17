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
    public class ValidadorPassagemArtespActor : BaseArtespActor<ValidadorPassagemArtespMessage, ValidadorPassagemResponse, ValidadorPassagemArtespHandler>
    {
        protected override void PreStart()
        {
            Workers = ActorsArtespCreator.CreateValidadorPassagemChildrenActors(Context);
            FluxoAtual = ArtespActorsEnum.ValidadorPassagemActor;
        }

        protected override void ActorActivator(bool fake)
        {
            base.ActorActivator(fake);
            Workers[ArtespActorsEnum.ValidadorPassagemSistemaActor].Tell(fake);
        }

        protected override void ChamarRequisicao(ValidadorPassagemArtespMessage mensagem)
        {
            try
            {                
                Response = Handler.Execute(new ValidadorPassagemRequest { PassagemPendenteArtesp = mensagem.PassagemPendenteArtesp });
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(Exception))
                    throw new PassagemInvalidaException(mensagem.PassagemPendenteArtesp.MensagemItemId, ex.Message, ex);
                throw;
            }


            if(Response.MotivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
            {
                Workers[ArtespActorsEnum.ValidadorPassagemSistemaActor].Tell(new ValidadorPassagemSistemaArtespMessage
                {
                    PassagemPendenteArtesp = Response.PassagemPendenteArtesp
                });
            }
            else
            {
                if (Response.PassagemInvalida)
                {
                    var m = new GeradorPassagemProcessadaMensageriaMessage(
                        ResultadoPassagem.NaoCompensado,
                        Response.PassagemPendenteArtesp,
                        Response.MotivoNaoCompensado);                    

                    Workers[ArtespActorsEnum.ProcessadorPassagemAprovadaActor].Tell(m);
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
}