using System.Collections.Generic;
using System.Linq;
using ConectCar.Cadastros.Conveniados.Backend.CommonQuery.Query;
using ConectCar.Cadastros.Domain.Model;
using ConectCar.Framework.Infrastructure.Cqrs.Handlers;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Framework.Infrastructure.Data.Cache.DataProviders;
using LeitorPassagensProcessadasBatch.CommandQuery.Queries;

namespace LeitorPassagensProcessadasBatch.CommandQuery.Handlers
{
    public class ConcessionariaHandler : DataSourceHandlerBase,
        IAdoDataSourceProvider,
        ICacheDataSourceProvider
    {
        protected override void Init()
        {
            AddProvider(new DbConnectionDataSourceProvider());
            AddProvider(new CacheDataSourceProvider(new CommonCacheDataSourceProvider()));
        }

        public DbConnectionDataSourceProvider AdoDataSourceProvider => GetAdoProvider();

        public CacheDataSourceProvider CacheDataSourceProvider => GetCacheProvider();

        private readonly DbConnectionDataSource _dbConnectionDataSourceSys;
        private readonly DbConnectionDataSource _dbConnectionDataSourceFallBack;

        public ConcessionariaHandler()
        {
            _dbConnectionDataSourceSys = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSysReadOnly);
            _dbConnectionDataSourceFallBack = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSys);
        }

        public IEnumerable<ConcessionariaModel> Execute()
        {
            var query = new ObterConcessionariasQuery(true, _dbConnectionDataSourceSys, _dbConnectionDataSourceFallBack);
            var concessionarias = query.Execute();
            concessionarias = concessionarias.Where(c => c.AtivoProtocoloArtesp);

            return concessionarias;
        }
    }
}
