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
    public class LeitorEdiHandler : TransacaoHandler,
         ICommand<LerPassagemPendenteEdiRequest, List<PassagemPendenteMessageEdi>>,
         ICommand<EnviarPassagemEdiRequest>
    {
        #region Properties
        private ObterPassagensTopicQueryEdi _obterPassagensEdiQuery;
        private EnviarPassagemEdiParaAkkaCommand _enviarPassagemEdiParaAkkaCommand;
        #endregion

        #region Ctor
        public LeitorEdiHandler()
        {
            var nomeTopicPadrao = ServiceBusUtil.ObterNome(ProtocoloEnum.PassagensPendentesEdi);
            _obterPassagensEdiQuery = new ObterPassagensTopicQueryEdi(_serviceBusDataSource, true, ServiceBusUtil.BatchSize, nomeTopicPadrao);
            _enviarPassagemEdiParaAkkaCommand = new EnviarPassagemEdiParaAkkaCommand(_restDataSource);
        }
        #endregion

        #region Methods
        public List<PassagemPendenteMessageEdi> Execute(LerPassagemPendenteEdiRequest args)
        {
            var passagensPendentes = new List<PassagemPendenteMessageEdi>();
            try
            {
                var nomeTopic = ServiceBusUtil.ObterNome(ProtocoloEnum.PassagensPendentesEdi);

                Log.Debug(String.Format(LeitorPassagensPendentesBatchResource.ObterPassagensPendentesBarramento, nomeTopic));
                passagensPendentes = _obterPassagensEdiQuery.Execute(nomeTopic, $"sb_{nomeTopic}").ToList();
            }
            catch (Exception e)
            {
                Log.Error(string.Format(LeitorPassagensPendentesBatchResource.Error, e.Message), e);
            }
            return passagensPendentes;
        }

        public void Execute(EnviarPassagemEdiRequest args)
        {
            if (args.passagemPendenteMessageEdi.Any())
            {
                foreach (var passagemPendenteMessageEdi in args.passagemPendenteMessageEdi)
                {
                    Log.Info($"Json TRN {passagemPendenteMessageEdi.DetalheTrnId} - {JsonConvert.SerializeObject(passagemPendenteMessageEdi)}");
                }

                var request = new EnviarPassagensEdiFilter(args.passagemPendenteMessageEdi);
                _enviarPassagemEdiParaAkkaCommand.Execute(request);
                
            }
        }
        #endregion
    }
}
