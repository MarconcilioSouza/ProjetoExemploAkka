using ConectCar.Framework.Infrastructure.Cqrs.ServiceBus.Commands;
using ConectCar.Framework.Infrastructure.Data.ServiceBus.DataProviders;
using GeradorPassagensPendentesEDIBatch.CommandQuery.Messages;

namespace GeradorPassagensPendentesEDIBatch.CommandQuery.Commands
{
    public class PassagemPendenteTopicCommand : ServiceBusTopicCommandBase2<PassagemPendenteEDIMessage>
    {
        public PassagemPendenteTopicCommand(ServiceBusDataSourceBase dataSource,
            bool keepMessageOrder,
            string topic)
            : base(dataSource, keepMessageOrder, topic, $"sb_{topic}")
        {
        }
    }
}