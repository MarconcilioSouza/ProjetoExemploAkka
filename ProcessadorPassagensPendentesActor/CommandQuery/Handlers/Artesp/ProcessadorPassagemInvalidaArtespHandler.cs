using System.Collections.Generic;
using AutoMapper;
using ConectCar.Framework.Infrastructure.Data.ServiceBus.DataProviders;
using ProcessadorPassagensActors.CommandQuery.Commands;
using ProcessadorPassagensActors.CommandQuery.Commands.Artesp;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Responses;
using ProcessadorPassagensActors.CommandQuery.Messages.Artesp;
using ProcessadorPassagensActors.CommandQuery.Util;
using ProcessadorPassagensActors.CommandQuery.Enums;
using ConectCar.Framework.Infrastructure.Log;
using ConectCar.Transacoes.Domain.Dto;
using ProcessadorPassagensActors.Infrastructure;
using ProcessadorPassagensActors.Infrastructure.Util;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp
{
    public class ProcessadorPassagemInvalidaArtespHandler : Loggable
    {
        private readonly ServiceBusDataSourceBase _dataSource;
        public ProcessadorPassagemInvalidaArtespHandler()
        {
            _dataSource = new ServiceBusDataSourceBase("TransacoesServiceBus", ServiceBusUtil.FactoriesCount);
        }
        /// <summary>
        /// Envia passagens inválidas para o barramento...
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public EndResponse Execute(ProcessadorPassagemInvalidaRequest request)
        {

            var topicName = ServiceBusUtil.ObterNomeTopicInvalida(EnumInfra.ProtocolosEnum.PassagensInvalidasArtesp);
            var passagemCommand = new PassagemInvalidaArtespTopicCommand(_dataSource, true, topicName);

            var dto = new PassagemInvalidaArtespDto
            {
                MensagemItemId = request.PassagemInvalidaArtesp.MensagemItemId,
                QuantidadeTentativas = request.PassagemInvalidaArtesp.QtdTentativas
            };
            var mensagem = Mapper.Map<PassagemInvalidaMessage>(dto);            

            //Enviando para o barramento...
            passagemCommand.Execute(new List<PassagemInvalidaMessage> { mensagem });

            return new EndResponse();
        }
    }
}
