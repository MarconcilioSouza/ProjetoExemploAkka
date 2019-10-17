using ConectCar.Framework.Infrastructure.Cqrs.Commands;
using LeitorPassagensProcessadasBatch.CommandQuery.Commands;
using LeitorPassagensProcessadasBatch.CommandQuery.Enum;
using LeitorPassagensProcessadasBatch.CommandQuery.Handlers.Request;
using LeitorPassagensProcessadasBatch.CommandQuery.Messages;
using LeitorPassagensProcessadasBatch.CommandQuery.Queries;
using LeitorPassagensProcessadasBatch.CommandQuery.Resources;
using LeitorPassagensProcessadasBatch.CommandQuery.Util;
using System.Collections.Generic;
using LeitorPassagensProcessadasBatch.CommandQuery.Commands.Artesp;
using LeitorPassagensProcessadasBatch.CommandQuery.Handlers.Request.Artesp;
using LeitorPassagensProcessadasBatch.CommandQuery.Messages.Artesp;
using LeitorPassagensProcessadasBatch.CommandQuery.Queries.Artesp;

namespace LeitorPassagensProcessadasBatch.CommandQuery.Handlers
{
    public sealed class TransacaoArtespHandler : TransacaoHandler,
        ICommand<ProcessarAprovadasArtespRequest>,
        ICommand<ProcessarReprovadasArtespRequest>,
        ICommand<ProcessarInvalidasArtespRequest>,
        ICommand<ObterMensagensAprovadasArtespRequest, IList<PassagemAprovadaArtespMessage>>,
        ICommand<ObterMensagensReprovadasArtespRequest, IList<PassagemReprovadaArtespMessage>>,
        ICommand<ObterMensagensInvalidasArtespRequest, IList<PassagemInvalidaArtespMessage>>
    {

        private readonly ObterPassagensAprovadasArtespQuery _obterPassagensAprovadasArtespQuery;
        private readonly ObterPassagensReprovadasArtespQuery _obterPassagensReprovadasArtespQuery;
        private readonly ObterPassagensInvalidasArtespQuery _obterPassagensInvalidasArtespQuery;


        public TransacaoArtespHandler()
        {
            var topicAprovada = ServiceBusUtil.ObterNome(TypeTransacao.AprovadaArtesp);
            _obterPassagensAprovadasArtespQuery = new ObterPassagensAprovadasArtespQuery(ServiceBusDataSource, true, ServiceBusUtil.BatchSize, topicAprovada);

            var topicReprovada = ServiceBusUtil.ObterNome(TypeTransacao.ReprovadaArtesp);
            _obterPassagensReprovadasArtespQuery = new ObterPassagensReprovadasArtespQuery(ServiceBusDataSource, true, ServiceBusUtil.BatchSize, topicReprovada);

            var topicInvalida = ServiceBusUtil.ObterNome(TypeTransacao.InvalidaArtesp);
            _obterPassagensInvalidasArtespQuery = new ObterPassagensInvalidasArtespQuery(ServiceBusDataSource, true, ServiceBusUtil.BatchSize, topicInvalida);
        }

        public void Execute(ProcessarAprovadasArtespRequest args)
        {
            var transacaoCommand = new ProcessadorPassagemAprovadaArtespCommand(RestDataSource);
            transacaoCommand.Execute(args);
        }

        public void Execute(ProcessarReprovadasArtespRequest args)
        {
            var transacaoCommand = new ProcessadorPassagemReprovadaArtespCommand(RestDataSource);
            transacaoCommand.Execute(args);
        }

        public void Execute(ProcessarInvalidasArtespRequest args)
        {
            var transacaoCommand = new ProcessadorPassagemInvalidaArtespCommand(RestDataSource);
            transacaoCommand.Execute(args);
        }

        public IList<PassagemAprovadaArtespMessage> Execute(ObterMensagensAprovadasArtespRequest args)
        {
            try
            {
                var transacoes = _obterPassagensAprovadasArtespQuery.Execute();
                return transacoes;
            }
            catch (System.Exception ex)
            {
                Log.Error(string.Format(LeitorPassagensProcessadasBatchResource.Error, ex.Message));
            }

            return new List<PassagemAprovadaArtespMessage>();
        }

        public IList<PassagemReprovadaArtespMessage> Execute(ObterMensagensReprovadasArtespRequest args)
        {
            try
            {
                var transacoes = _obterPassagensReprovadasArtespQuery.Execute();
                return transacoes;
            }
            catch (System.Exception ex)
            {
                Log.Error(string.Format(LeitorPassagensProcessadasBatchResource.Error, ex.Message));
            }

            return new List<PassagemReprovadaArtespMessage>();
        }

        public IList<PassagemInvalidaArtespMessage> Execute(ObterMensagensInvalidasArtespRequest args)
        {
            try
            {
                var transacoes = _obterPassagensInvalidasArtespQuery.Execute();
                return transacoes;
            }
            catch (System.Exception ex)
            {
                Log.Error(string.Format(LeitorPassagensProcessadasBatchResource.Error, ex.Message));
            }
            return new List<PassagemInvalidaArtespMessage>();
        }
    }
}
