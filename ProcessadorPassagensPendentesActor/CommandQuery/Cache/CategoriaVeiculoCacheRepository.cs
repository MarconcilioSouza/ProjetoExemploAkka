using ConectCar.Comercial.Cliente.Adesao.Backend.CommonQuery.Query;
using ConectCar.Comercial.Domain.Model;
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
    public static class CategoriaVeiculoCacheRepository
    {

        
        private static MemoryCache _memoryCache;
        private static ObterCategoriaVeiculoQuery _categoriaVeiculoQuery;


        static CategoriaVeiculoCacheRepository()
        {
            _memoryCache = new MemoryCache("CategoriaVeiculoMemoryCache");

            var dataSourceProvider = new DbConnectionDataSourceProvider();
            var dataSource = dataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSys);

            _categoriaVeiculoQuery = new ObterCategoriaVeiculoQuery(true, dataSource);
        }        

        public static List<CategoriaVeiculoModel> Listar()
        {
            var categoriasVeiculo = Carregar1StLevelCache();
            if (categoriasVeiculo == null)
            {
                categoriasVeiculo = CarregarQuery();
                if (categoriasVeiculo == null)
                    categoriasVeiculo = new List<CategoriaVeiculoModel>();
                else
                {
                    var cacheItem = new CacheItem("categoriaveiculo", categoriasVeiculo);
                    var cacheItemPolicy = new CacheItemPolicy
                    {
                        SlidingExpiration = new TimeSpan(0, 0, TimeHelper.CommandTimeOut)
                    };
                    _memoryCache.Add(cacheItem, cacheItemPolicy);
                }
            }

            return categoriasVeiculo.ToList();
        }

        private static IEnumerable<CategoriaVeiculoModel> CarregarQuery()
        {            
            var ret = DataBaseConnection.HandleExecution(_categoriaVeiculoQuery.Execute);
            return ret;
        }

        private static IEnumerable<CategoriaVeiculoModel> Carregar1StLevelCache()
        {
            var item = _memoryCache.Get("categoriaveiculo");
            return item as IEnumerable<CategoriaVeiculoModel>;
        }
    }
}
