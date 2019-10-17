using ConectCar.Framework.Infrastructure.Cqrs.Commands;
using LeitorPassagensProcessadasBatch.CommandQuery.Commands.EDI;
using LeitorPassagensProcessadasBatch.CommandQuery.Enum;
using LeitorPassagensProcessadasBatch.CommandQuery.Handlers.Request.EDI;
using LeitorPassagensProcessadasBatch.CommandQuery.Messages.EDI;
using LeitorPassagensProcessadasBatch.CommandQuery.Queries.EDI;
using LeitorPassagensProcessadasBatch.CommandQuery.Resources;
using LeitorPassagensProcessadasBatch.CommandQuery.Util;
using System.Collections.Generic;

namespace LeitorPassagensProcessadasBatch.CommandQuery.Handlers
{
    public sealed class TransacaoEdiHandler : TransacaoHandler,
        ICommand<ProcessarAprovadasEdiRequest>,
        ICommand<ProcessarReprovadasEdiRequest>,
        ICommand<ObterMensagensAprovadasEdiRequest, IList<PassagemAprovadaEdiMessage>>,
        ICommand<ObterMensagensReprovadasEdiRequest, IList<PassagemReprovadaEdiMessage>>
    {
        public void Execute(ProcessarAprovadasEdiRequest args)
        {
            var transacaoCommand = new ProcessadorPassagemAprovadaEdiCommand(RestDataSource);
            transacaoCommand.Execute(args);
        }

        public void Execute(ProcessarReprovadasEdiRequest args)
        {
            var transacaoCommand = new ProcessadorPassagemReprovadaEdiCommand(RestDataSource);
            transacaoCommand.Execute(args);
        }

        public IList<PassagemAprovadaEdiMessage> Execute(ObterMensagensAprovadasEdiRequest args)
        {
            try
            {
                var nomeQueue = ServiceBusUtil.ObterNome(TypeTransacao.AprovadaEdi);
                var transacoesQuery = new ObterPassagensAprovadasEdiQuery(ServiceBusDataSource,
                    true, ServiceBusUtil.BatchSize, nomeQueue);

                var transacoes = transacoesQuery.Execute();
                return transacoes;
            }
            catch (System.Exception ex)
            {
                Log.Error(string.Format(LeitorPassagensProcessadasBatchResource.Error, ex.Message));
            }

            return new List<PassagemAprovadaEdiMessage>();
        }

        public IList<PassagemReprovadaEdiMessage> Execute(ObterMensagensReprovadasEdiRequest args)
        {
            try
            {
                var nomeQueue = ServiceBusUtil.ObterNome(TypeTransacao.ReprovadaEdi);
                var transacoesQuery = new ObterPassagensReprovadasEdiQuery(ServiceBusDataSource,
                    true, ServiceBusUtil.BatchSize, nomeQueue);

                var transacoes = transacoesQuery.Execute();
                return transacoes;
            }
            catch (System.Exception ex)
            {
                Log.Error(string.Format(LeitorPassagensProcessadasBatchResource.Error, ex.Message));
            }

            return new List<PassagemReprovadaEdiMessage>();
        }
    }
}
