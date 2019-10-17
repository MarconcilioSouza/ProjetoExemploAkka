using ConectCar.Framework.Infrastructure.Cqrs.Handlers;
using ConectCar.Framework.Infrastructure.Data.Rest.DataProviders;
using ConectCar.Framework.Infrastructure.Data.ServiceBus.DataProviders;
using LeitorPassagensProcessadasBatch.CommandQuery.Util;

namespace LeitorPassagensProcessadasBatch.CommandQuery.Handlers
{
    public class TransacaoHandler : DataSourceHandlerBase
    {
        protected readonly ServiceBusDataSourceBase ServiceBusDataSource;
        protected readonly RestDataSource RestDataSource;

        public TransacaoHandler()
        {
            ServiceBusDataSource = new ServiceBusDataSourceBase("TransacoesServiceBus", ServiceBusUtil.FactoriesCount);
            RestDataSource = new RestDataSource("ProcessadorPassagensProcessadasApi");
        }
        protected override void Init()
        {
            AddProvider(new RestDataSourceProvider());
        }
    }
}
