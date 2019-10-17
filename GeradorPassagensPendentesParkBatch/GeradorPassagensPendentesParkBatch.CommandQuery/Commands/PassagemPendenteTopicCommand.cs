using ConectCar.Framework.Infrastructure.Cqrs.ServiceBus.Commands;
using ConectCar.Framework.Infrastructure.Data.ServiceBus.DataProviders;
using GeradorPassagensPendentesParkBatch.CommandQuery.Messages;

namespace GeradorPassagensPendentesParkBatch.CommandQuery.Commands
{
    public class PassagemPendenteTopicCommand : ServiceBusTopicCommandBase2<PassagemPendenteParkMessage>
    {
        public PassagemPendenteTopicCommand(ServiceBusDataSourceBase dataSource,
            bool keepMessageOrder,
            string topic)
            : base(dataSource, keepMessageOrder, topic, $"sb_{topic}")
        {
        }
    }
}