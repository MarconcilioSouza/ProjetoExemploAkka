using ConectCar.Framework.Infrastructure.Cqrs.ServiceBus.Commands;
using ConectCar.Framework.Infrastructure.Data.ServiceBus.DataProviders;
using ProcessadorPassagensActors.CommandQuery.Messages.Edi;

namespace ProcessadorPassagensActors.CommandQuery.Commands.Edi
{
    public class PassagemReprovadaEDITopicCommand : ServiceBusTopicCommandBase2<PassagemReprovadaEDIMessage>
    {
        public PassagemReprovadaEDITopicCommand(ServiceBusDataSourceBase dataSource,
            bool keepMessageOrder,
            string topic)
            : base(dataSource, keepMessageOrder, topic, $"sb_{topic}")
        {
        }
    }
}
