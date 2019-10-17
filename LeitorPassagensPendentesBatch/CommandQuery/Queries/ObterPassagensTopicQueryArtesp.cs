using ConectCar.Framework.Infrastructure.Cqrs.ServiceBus.Queries;
using ConectCar.Framework.Infrastructure.Data.ServiceBus.DataProviders;
using LeitorPassagensPendentesBatch.CommandQuery.Messages;

namespace LeitorPassagensPendentesBatch.CommandQuery.Queries
{
    public class ObterPassagensTopicQueryArtesp : ServiceBusTopicQueryBase2<PassagemPendenteMessageArtesp>
    {
        public ObterPassagensTopicQueryArtesp(ServiceBusDataSourceBase dataSource,
            bool keepMessageOrder,
            int batchSize,
            string topic) : base(dataSource, keepMessageOrder, batchSize, topic, $"sb_{topic}")
        {
        }
    }
}
