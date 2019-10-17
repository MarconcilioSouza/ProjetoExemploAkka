﻿using System;
using Akka.Actor;
using ProcessadorPassagensActors.ActorsMessages.Edi;
using ProcessadorPassagensActors.ActorsUtils;
using ProcessadorPassagensActors.CommandQuery.Exceptions;
using ProcessadorPassagensActors.CommandQuery.Handlers.Edi;
using ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Request;
using ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Responses;
using ProcessadorPassagensActors.Enums;

namespace ProcessadorPassagensActors.Actors.Edi
{
    public class ValidadorPassagemValePedagioEdiActor : BaseEdiActor<ValidadorPassagemValePedagioEdiMessage, ValidadorPassagemValePedagioEdiResponse, ValidadorPassagemValePedagioEdiHandler>
    {
        protected override void PreStart()
        {
            Workers = ActorsEdiCreator.CreateValidadorPassagemValePedadgioEdiChildrenActors(Context);
            FluxoAtual = EdiActorsEnum.ValidadorPassagemValePedagioEdiActor;
        }

        protected override void ChamarRequisicao(ValidadorPassagemValePedagioEdiMessage mensagem)
        {
            try
            {
                Response = Handler.Execute(
                       new ValidadorPassagemValePedagioEdiRequest { PassagemPendenteEdi = mensagem.PassagemPendenteEdi });
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(Exception))
                    throw new PassagemInvalidaException(mensagem.PassagemPendenteEdi.DetalheTrnId, ex.Message, ex);
                throw;
            }

            Workers[EdiActorsEnum.GeradorPassagemAprovadaEdiActor].Tell(new GeradorPassagemAprovadaEdiMessage
            {
                PassagemPendenteEdi = mensagem.PassagemPendenteEdi,
                DetalheViagens = Response.DetalheViagens,
            });
        }
    }
}