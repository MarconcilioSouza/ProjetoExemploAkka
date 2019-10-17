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

namespace LeitorPassagensPendentesBatch.CommandQuery.Handlers
{
    public class LeitorArtespHandler : TransacaoHandler,
        ICommand<LerPassagemPendenteArtespRequest, List<PassagemPendenteMessageArtesp>>,
        ICommand<EnviarPassagemArtespRequest>
    {
        #region Properties
        private ObterPassagensTopicQueryArtesp _obterPassagensQueryArtesp;
        private EnviarPassagemArtespParaAkkaCommand _enviarPassagemArtespParaAkkaCommand;
        #endregion

        #region Ctor
        public LeitorArtespHandler()
        {
            var nomeTopicPadrao = ServiceBusUtil.ObterNome(ProtocoloEnum.PassagensPendentesArtesp);
            _obterPassagensQueryArtesp = new ObterPassagensTopicQueryArtesp(_serviceBusDataSource, true, ServiceBusUtil.BatchSize, nomeTopicPadrao);
            _enviarPassagemArtespParaAkkaCommand = new EnviarPassagemArtespParaAkkaCommand(_restDataSource);
        }
        #endregion

        #region Methods
        public List<PassagemPendenteMessageArtesp> Execute(LerPassagemPendenteArtespRequest args)
        {
            var passagensPendentes = new List<PassagemPendenteMessageArtesp>();
            try
            {
                var nomeTopic = ServiceBusUtil.ObterNome(ProtocoloEnum.PassagensPendentesArtesp);

                Log.Debug(String.Format(LeitorPassagensPendentesBatchResource.ObterPassagensPendentesBarramento, nomeTopic));
                passagensPendentes = _obterPassagensQueryArtesp.Execute(nomeTopic, $"sb_{nomeTopic}").ToList();
           }
            catch (Exception e)
            {
                Log.Error(string.Format(LeitorPassagensPendentesBatchResource.Error, e.Message), e);
            }
            return passagensPendentes;
        }

        public void Execute(EnviarPassagemArtespRequest args)
        {
            if (args.passagemPendenteMessageArtesp.Any())
            {
                var request = new EnviarPassagensArtespFilter(args.passagemPendenteMessageArtesp);
                RastreamentoLogs(args.passagemPendenteMessageArtesp);
                _enviarPassagemArtespParaAkkaCommand.Execute(request);
            }
        }


        private void RastreamentoLogs(IEnumerable<PassagemPendenteMessageArtesp> passagensPendentes)
        {
            try
            {
                //Rastreamento dos itens na tabela de logs...
                var itensGerados = passagensPendentes.Select(x => x.MensagemItemId.ToString())
                    .Aggregate((a, b) => $"{a} - {b}");

                Log.Info($"Itens processados: {itensGerados}");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }

        #endregion
    }
}
