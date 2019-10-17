using ConectCar.Framework.Infrastructure.Cqrs.ServiceBus.Commands;
using ConectCar.Framework.Infrastructure.Data.ServiceBus.DataProviders;
using ProcessadorPassagensActors.CommandQuery.Messages.Artesp;

namespace ProcessadorPassagensActors.CommandQuery.Commands.Artesp
{
    public class PassagemAprovadaArtespTopicCommand : ServiceBusTopicCommandBase2<PassagemAprovadaMessage>
    {
        public PassagemAprovadaArtespTopicCommand(ServiceBusDataSourceBase dataSource,
            bool keepMessageOrder,
            string topic)
            : base(dataSource, keepMessageOrder, topic, $"sb_{topic}")
        {
        }
    }
}
