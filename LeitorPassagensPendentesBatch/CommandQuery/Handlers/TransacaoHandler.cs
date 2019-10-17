using ConectCar.Framework.Infrastructure.Cqrs.Handlers;
using ConectCar.Framework.Infrastructure.Data.Rest.DataProviders;
using ConectCar.Framework.Infrastructure.Data.ServiceBus.DataProviders;
using LeitorPassagensPendentesBatch.CommandQuery.Util;

namespace LeitorPassagensPendentesBatch.CommandQuery.Handlers
{
    public class TransacaoHandler : DataSourceHandlerBase
    {
        protected readonly ServiceBusDataSourceBase _serviceBusDataSource;
        protected readonly RestDataSource _restDataSource;

        public TransacaoHandler()
        {
            _serviceBusDataSource = new ServiceBusDataSourceBase("TransacoesServiceBus", ServiceBusUtil.FactoriesCount);
            _restDataSource = new RestDataSource("ProcessadorPassagensPendentesApi");
        }
        protected override void Init()
        {
            AddProvider(new RestDataSourceProvider());
        }
    }
}
