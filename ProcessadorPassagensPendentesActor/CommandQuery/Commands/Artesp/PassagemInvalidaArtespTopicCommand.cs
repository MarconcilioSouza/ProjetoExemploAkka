using ConectCar.Framework.Infrastructure.Cqrs.ServiceBus.Commands;
using ConectCar.Framework.Infrastructure.Data.ServiceBus.DataProviders;
using ProcessadorPassagensActors.CommandQuery.Messages.Artesp;

namespace ProcessadorPassagensActors.CommandQuery.Commands.Artesp
{
    public class PassagemInvalidaArtespTopicCommand : ServiceBusTopicCommandBase2<PassagemInvalidaMessage>
    {
        public PassagemInvalidaArtespTopicCommand(ServiceBusDataSourceBase dataSource,
            bool keepMessageOrder,
            string topic)
            : base(dataSource, keepMessageOrder, topic, $"sb_{topic}")
        {
        }
    }
}
