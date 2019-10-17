using ConectCar.Framework.Infrastructure.Cqrs.ServiceBus.Queries;
using ConectCar.Framework.Infrastructure.Data.ServiceBus.DataProviders;
using LeitorPassagensProcessadasBatch.CommandQuery.Messages.Park;

namespace LeitorPassagensProcessadasBatch.CommandQuery.Queries.Park
{
    public sealed class ObterPassagensAprovadasParkQuery : ServiceBusTopicQueryBase2<PassagemAprovadaParkMessage>
    {
        public ObterPassagensAprovadasParkQuery(ServiceBusDataSourceBase dataSource,
            bool keepMessageOrder,
            int batchSize,
            string topic) : base(dataSource, keepMessageOrder, batchSize, topic, $"sb_{topic}")
        {
        }
    }
}
