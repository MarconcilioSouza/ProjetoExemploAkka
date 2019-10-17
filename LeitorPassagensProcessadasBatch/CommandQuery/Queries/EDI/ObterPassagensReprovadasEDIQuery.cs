using ConectCar.Framework.Infrastructure.Cqrs.ServiceBus.Queries;
using ConectCar.Framework.Infrastructure.Data.ServiceBus.DataProviders;
using LeitorPassagensProcessadasBatch.CommandQuery.Messages.EDI;

namespace LeitorPassagensProcessadasBatch.CommandQuery.Queries.EDI
{
    public class ObterPassagensReprovadasEdiQuery : ServiceBusTopicQueryBase2<PassagemReprovadaEdiMessage>
    {
        public ObterPassagensReprovadasEdiQuery(ServiceBusDataSourceBase dataSource,
            bool keepMessageOrder,
            int batchSize,
            string topic) : base(dataSource, keepMessageOrder, batchSize, topic, $"sb_{topic}")
        {
        }
    }
}
