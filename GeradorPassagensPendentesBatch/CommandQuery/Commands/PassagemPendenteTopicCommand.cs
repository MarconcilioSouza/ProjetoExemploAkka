using ConectCar.Framework.Infrastructure.Cqrs.ServiceBus.Commands;
using ConectCar.Framework.Infrastructure.Data.ServiceBus.DataProviders;
using GeradorPassagensPendentesBatch.CommandQuery.Messages;

namespace GeradorPassagensPendentesBatch.CommandQuery.Commands
{
    public class PassagemPendenteTopicCommand : ServiceBusTopicCommandBase2<PassagemPendenteMessage>
    {
        public PassagemPendenteTopicCommand(ServiceBusDataSourceBase dataSource,
            bool keepMessageOrder,
            string topic)
            : base(dataSource, keepMessageOrder, topic, $"sb_{topic}")
        {
        }
    }
}
