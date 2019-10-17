using ConectCar.Framework.Infrastructure.Cqrs.Commands;
using LeitorPassagensProcessadasBatch.CommandQuery.Commands.Park;
using LeitorPassagensProcessadasBatch.CommandQuery.Handlers.Request.Park;
using LeitorPassagensProcessadasBatch.CommandQuery.Messages.Park;
using System.Collections.Generic;
using System;
using LeitorPassagensProcessadasBatch.CommandQuery.Util;
using LeitorPassagensProcessadasBatch.CommandQuery.Enum;
using LeitorPassagensProcessadasBatch.CommandQuery.Queries.Park;
using LeitorPassagensProcessadasBatch.CommandQuery.Resources;

namespace LeitorPassagensProcessadasBatch.CommandQuery.Handlers
{
    public sealed class TransacaoParkHandler : TransacaoHandler,
        ICommand<ProcessarAprovadasParkRequest>,
        ICommand<ProcessarReprovadasParkRequest>,
        ICommand<ObterMensagensAprovadasParkRequest, IList<PassagemAprovadaParkMessage>>,
        ICommand<ObterMensagensReprovadasParkRequest, IList<PassagemReprovadaParkMessage>>
    {
        public void Execute(ProcessarAprovadasParkRequest args)
        {
            var transacaoCommand = new ProcessadorPassagemAprovadaParkCommand(RestDataSource);
            transacaoCommand.Execute(args);
        }

        public void Execute(ProcessarReprovadasParkRequest args)
        {
            var transacaoCommand = new ProcessadorPassagemReprovadaParkCommand(RestDataSource);
            transacaoCommand.Execute(args);
        }

        public IList<PassagemAprovadaParkMessage> Execute(ObterMensagensAprovadasParkRequest args)
        {
            try
            {
                var nomeQueue = ServiceBusUtil.ObterNome(TypeTransacao.AprovadaPark);

                var transacoesQuery = new ObterPassagensAprovadasParkQuery(ServiceBusDataSource, true, ServiceBusUtil.BatchSize, nomeQueue);

                var transacoes = transacoesQuery.Execute();
                return transacoes;
            }
            catch (System.Exception ex)
            {
                Log.Error(string.Format(LeitorPassagensProcessadasBatchResource.Error, ex.Message));
            }

            return new List<PassagemAprovadaParkMessage>();
        }

        public IList<PassagemReprovadaParkMessage> Execute(ObterMensagensReprovadasParkRequest args)
        {
            try
            {
                var nomeQueue = ServiceBusUtil.ObterNome(TypeTransacao.ReprovadaPark);

                var transacoesQuery = new ObterPassagensReprovadasParkQuery(ServiceBusDataSource, true, ServiceBusUtil.BatchSize, nomeQueue);

                var transacoes = transacoesQuery.Execute();
                return transacoes;
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(LeitorPassagensProcessadasBatchResource.Error, ex.Message));
            }

            return new List<PassagemReprovadaParkMessage>();
        }
    }
}
