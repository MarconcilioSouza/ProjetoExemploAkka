using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConectCar.Framework.Infrastructure.Cqrs.Commands;
using ConectCar.Framework.Infrastructure.Log;
using ProcessadorPassagensActors.CommandQuery.Handlers.Park.Request;
using ProcessadorPassagensActors.CommandQuery.Handlers.Park.Response;
using ConectCar.Framework.Infrastructure.Data.ServiceBus.DataProviders;
using ConectCar.Transacoes.Domain.Dto;
using ProcessadorPassagensActors.CommandQuery.Util;
using AutoMapper;
using ConectCar.Transacoes.Domain.Model;
using Newtonsoft.Json;
using ProcessadorPassagensActors.CommandQuery.Enums;
using ProcessadorPassagensActors.CommandQuery.Commands.Park;
using ProcessadorPassagensActors.CommandQuery.Messages.Park;
using ProcessadorPassagensActors.Infrastructure;
using ProcessadorPassagensActors.Infrastructure.Util;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Park
{
    public class ProcessadorPassagemReprovadaParkHandler : Loggable,
            ICommand<ProcessarPassagemReprovadaParkRequest, ProcessarPassagemReprovadaParkResponse>
    {
        #region [Properties]

        private readonly ServiceBusDataSourceBase _dataSource;
        private PassagemReprovadaEstacionamentoDto _passagemReprovadaDto;

        #endregion [Properties]

        #region [Ctor]

        public ProcessadorPassagemReprovadaParkHandler()
        {
            _dataSource = new ServiceBusDataSourceBase("TransacoesServiceBus", ServiceBusUtil.FactoriesCount);
        }

        #endregion [Ctor]

        public ProcessarPassagemReprovadaParkResponse Execute(ProcessarPassagemReprovadaParkRequest request)
        {
            EnviarParaBarramento(request);

            return new ProcessarPassagemReprovadaParkResponse() { Processado = true };
        }

        private void EnviarParaBarramento(ProcessarPassagemReprovadaParkRequest request)
        {
            var topicName = ServiceBusUtil.ObterNomeTopicReprovada(EnumInfra.ProtocolosEnum.PassagensReprovadasPark);
            var passagemCommand = new PassagemReprovadaParkTopicCommand(_dataSource, true, topicName);

            var mensagem = Mapper.Map(request.PassagemReprovadaEstacionamento, new PassagemReprovadaParkMessage());
            passagemCommand.Execute(new List<PassagemReprovadaParkMessage> { mensagem });
            Log.Info($"Json saida RegistroTransacaoId - {request.PassagemReprovadaEstacionamento.TransacaoEstacionamentoRecusada.RegistroTransacaoId} (reprovado): {JsonConvert.SerializeObject(new List<PassagemReprovadaParkMessage> { mensagem })}");
        }
    }
}
