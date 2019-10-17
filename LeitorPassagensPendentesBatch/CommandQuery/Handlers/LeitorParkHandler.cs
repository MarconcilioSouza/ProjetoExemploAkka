using ConectCar.Framework.Infrastructure.Cqrs.Commands;
using LeitorPassagensPendentesBatch.CommandQuery.Commands;
using LeitorPassagensPendentesBatch.CommandQuery.Enum;
using LeitorPassagensPendentesBatch.CommandQuery.Handlers.Request;
using LeitorPassagensPendentesBatch.CommandQuery.Messages;
using LeitorPassagensPendentesBatch.CommandQuery.Queries;
using LeitorPassagensPendentesBatch.CommandQuery.Resources;
using LeitorPassagensPendentesBatch.CommandQuery.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace LeitorPassagensPendentesBatch.CommandQuery.Handlers
{
    public class LeitorParkHandler : TransacaoHandler,
          ICommand<LerPassagemPendenteParkRequest, List<PassagemPendenteMessagePark>>,
          ICommand<EnviarPassagemParkRequest>
    {
        #region Properties
        private ObterPassagensTopicQueryPark _obterPassagensQueryPark;
        private EnviarPassagemParkParaAkkaCommand _enviarPassagemParkParaAkkaCommand;
        #endregion

        #region Ctor
        public LeitorParkHandler()
        {
            var nomeTopicPadrao = ServiceBusUtil.ObterNome(ProtocoloEnum.PassagensPendentesPark);
            _obterPassagensQueryPark = new ObterPassagensTopicQueryPark(_serviceBusDataSource, true, ServiceBusUtil.BatchSize, nomeTopicPadrao);
            _enviarPassagemParkParaAkkaCommand = new EnviarPassagemParkParaAkkaCommand(_restDataSource);
        }
        #endregion

        #region Methods
        public List<PassagemPendenteMessagePark> Execute(LerPassagemPendenteParkRequest args)
        {
            var passagensPendentes = new List<PassagemPendenteMessagePark>();
            try
            {
                var nomeTopic = ServiceBusUtil.ObterNome(ProtocoloEnum.PassagensPendentesPark);

                Log.Debug(String.Format(LeitorPassagensPendentesBatchResource.ObterPassagensPendentesBarramento, nomeTopic));
                passagensPendentes = _obterPassagensQueryPark.Execute(nomeTopic, $"sb_{nomeTopic}").ToList();
            }
            catch (Exception e)
            {
                Log.Error(string.Format(LeitorPassagensPendentesBatchResource.Error, e.Message), e);
            }
            return passagensPendentes;
        }

        public void Execute(EnviarPassagemParkRequest args)
        {
            if (args.passagemPendenteMessagePark.Any())
            {
                var request = new EnviarPassagensParkFilter(args.passagemPendenteMessagePark);

                foreach (var passagemPendenteMessagePark in args.passagemPendenteMessagePark)
                {
                    Log.Info($"Json PARK {passagemPendenteMessagePark.Ticket} - {JsonConvert.SerializeObject(passagemPendenteMessagePark)}");
                }

                _enviarPassagemParkParaAkkaCommand.Execute(request);
            }
        }
        #endregion
    }
}
