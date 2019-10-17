﻿using System;
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
    public class ValidadorPassagemValePedagioArtespActor : BaseArtespActor<ValidadorPassagemValePedagioArtespMessage, ValidadorPassagemValePedagioResponse, ValidadorPassagemValePedagioArtespHandler>
    {
        protected override void PreStart()
        {
            Workers = ActorsArtespCreator.CreateValidadorPassagemValePedagioChildrenActors(Context);
            FluxoAtual = ArtespActorsEnum.ValidadorPassagemValePedagioActor;
        }

        protected override void ActorActivator(bool fake)
        {
            base.ActorActivator(fake);
            Workers[ArtespActorsEnum.ValidadorDivergenciaCategoriaPassagemActor].Tell(fake);
        }

        protected override void ChamarRequisicao(ValidadorPassagemValePedagioArtespMessage mensagem)
        {
            try
            {
                Response = Handler.Execute(new ValidadorPassagemValePedagioRequest { PassagemPendenteArtesp = mensagem.PassagemPendenteArtesp });
                
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(Exception) || ex.GetType() == typeof(DomainException))
                    throw new PassagemInvalidaException(mensagem.PassagemPendenteArtesp.MensagemItemId, ex.Message, ex);
                throw;
            }

            if(Response.MotivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
            {
                Workers[ArtespActorsEnum.ValidadorDivergenciaCategoriaPassagemActor].Tell(new ValidadorDivergenciaCategoriaPassagemArtespMessage
                {
                    PassagemPendenteArtesp = Response.PassagemPendenteArtesp,
                    ViagensAgendadas = Response.ViagensAgendadas
                });
            }
            else
            {
                var m = new GeradorPassagemReprovadaTransacaParceiroArtespMessage
                {
                    DetalheViagemId = Response.ViagemNaoCompensadaId ?? 0,
                    MotivoNaoCompensado = Response.MotivoNaoCompensado,
                    PassagemPendenteArtesp = Response.PassagemPendenteArtesp
                };

                Workers[ArtespActorsEnum.GeradorPassagemReprovadaActor].Tell(m);
            }           
        }
    }
}