using ConectCar.Framework.Infrastructure.Cqrs.ServiceBus.Queries;
using ConectCar.Framework.Infrastructure.Data.ServiceBus.DataProviders;
using LeitorPassagensProcessadasBatch.CommandQuery.Messages.Artesp;

namespace LeitorPassagensProcessadasBatch.CommandQuery.Queries.Artesp
{
    public class ObterPassagensAprovadasArtespQuery : ServiceBusTopicQueryBase2<PassagemAprovadaArtespMessage>
    {
        public ObterPassagensAprovadasArtespQuery(ServiceBusDataSourceBase dataSource,
            bool keepMessageOrder,
            int batchSize,
            string topic) : base(dataSource, keepMessageOrder, batchSize, topic, $"sb_{topic}")
        {
        }
    }
}
