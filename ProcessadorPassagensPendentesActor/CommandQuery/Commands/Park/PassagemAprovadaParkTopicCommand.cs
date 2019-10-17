using ConectCar.Framework.Infrastructure.Cqrs.ServiceBus.Commands;
using ConectCar.Framework.Infrastructure.Data.ServiceBus.DataProviders;
using ProcessadorPassagensActors.CommandQuery.Messages.Park;

namespace ProcessadorPassagensActors.CommandQuery.Commands.Park
{
    public class PassagemAprovadaParkTopicCommand : ServiceBusTopicCommandBase2<PassagemAprovadaParkMessage>
    {
        public PassagemAprovadaParkTopicCommand(ServiceBusDataSourceBase dataSource,
            bool keepMessageOrder,
            string topic)
            : base(dataSource, keepMessageOrder, topic, $"sb_{topic}")
        {
        }
    }
}