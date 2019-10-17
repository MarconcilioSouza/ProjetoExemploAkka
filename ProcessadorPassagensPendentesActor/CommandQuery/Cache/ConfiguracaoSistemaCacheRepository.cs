using ConectCar.Framework.Backend.CommonQuery.Query;
using ConectCar.Framework.Domain.Model;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ProcessadorPassagensActors.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.CommandQuery.Queries;

namespace ProcessadorPassagensActors.CommandQuery.Cache
{
    public static class ConfiguracaoSistemaCacheRepository
    {

        
        private static MemoryCache _memoryCache;
        private static ObterConfiguracaoSistemaQuery _configuracaoSistemaQuery;


        static ConfiguracaoSistemaCacheRepository()
        {
            _memoryCache = new MemoryCache("ConfiguracaoSistemaMemoryCache");

            var dataSourceProvider = new DbConnectionDataSourceProvider();
            var dataSource = dataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSysReadOnly);
            var dataSourceFallback = dataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSys);

            _configuracaoSistemaQuery = new ObterConfiguracaoSistemaQuery(true, dataSource, dataSourceFallback);
        }
        

        public static ConfiguracaoSistemaModel Obter(string nome)
        {            
            var configuracoesSistema = Listar();

            return configuracoesSistema.FirstOrDefault(x => x.Nome == nome);
        }

        public static List<ConfiguracaoSistemaModel> Listar()
        {
            var configuracoesSistema = Carregar1StLevelCache();
            if (configuracoesSistema == null)
            {
                configuracoesSistema = CarregarQuery();
                if (configuracoesSistema == null)
                    configuracoesSistema = new List<ConfiguracaoSistemaModel>();
                else
                {
                    var cacheItem = new CacheItem("configuracaosistema", configuracoesSistema);
                    var cacheItemPolicy = new CacheItemPolicy
                    {
                        SlidingExpiration = new TimeSpan(0, 0, TimeHelper.LagSeconds)
                    };
                    _memoryCache.Add(cacheItem, cacheItemPolicy);
                }
            }

            return configuracoesSistema.ToList();
        }

        private static IEnumerable<ConfiguracaoSistemaModel> CarregarQuery()
        {
            var ret = DataBaseConnection.HandleExecution(_configuracaoSistemaQuery.Execute);
            return ret;
        }

        private static IEnumerable<ConfiguracaoSistemaModel> Carregar1StLevelCache()
        {
            var item = _memoryCache.Get("configuracaosistema");
            return item as IEnumerable<ConfiguracaoSistemaModel>;
        }
    }
    
}
