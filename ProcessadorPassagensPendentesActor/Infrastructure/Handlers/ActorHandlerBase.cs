using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Framework.Infrastructure.Log;
using ProcessadorPassagensActors.Infrastructure.Util;

namespace ProcessadorPassagensActors.Infrastructure.Handlers
{
    public abstract class ActorHandlerBase: Loggable
    {
        private static DbConnectionDataSourceProvider DataSourceProvider = new DbConnectionDataSourceProvider();

        protected DbConnectionDataSource _dataSourceConectSysReadOnly;
        protected DbConnectionDataSource _dataSourceFallBack;
        protected DbConnectionDataSource _dataSourceMensageria;
        protected bool _testDataSourceHealthy;



        protected ActorHandlerBase()
        {
            _dataSourceConectSysReadOnly = DataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSysReadOnly);
            _dataSourceFallBack = DataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSys);
            _dataSourceMensageria = DataSourceProvider.GetDataSource(DbConnectionDataSourceType.Mensageria);
            _testDataSourceHealthy = ServiceBusUtil.DataBaseHealthyCheck();
        }
        
    }
}
