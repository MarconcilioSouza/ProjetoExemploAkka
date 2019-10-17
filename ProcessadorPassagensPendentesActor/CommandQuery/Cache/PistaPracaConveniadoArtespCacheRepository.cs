using ConectCar.Cadastros.Conveniados.Backend.CommonQuery.Filter;
using ConectCar.Cadastros.Conveniados.Backend.CommonQuery.Query;
using ConectCar.Cadastros.Domain.Dto;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ProcessadorPassagensActors.CommandQuery.Queries;
using ProcessadorPassagensActors.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using ProcessadorPassagensActors.CommandQuery.Connections;

namespace ProcessadorPassagensActors.CommandQuery.Cache
{
    public static class PistaPracaConveniadoArtespCacheRepository
    {
        
        private static MemoryCache _memoryCache;
        private static ObterPistaPracaConveniadoArtespCacheOriginQuery2 _pistaPracaConveniadoArtespQuery;

        static PistaPracaConveniadoArtespCacheRepository()
        {
            _memoryCache = new MemoryCache("PistaPracaConveniadoArtespMemoryCache");            
            _pistaPracaConveniadoArtespQuery = new ObterPistaPracaConveniadoArtespCacheOriginQuery2();
        }        

        /// <summary>
        /// Carrega as informações de pista, praça e conveniado.
        /// </summary>        
        /// <returns>PistaPracaConveniadoDto[]</returns>
        public static List<PistaPracaConveniadoDto> Listar()
        {
            var pistaPracaConveniado = Carregar1StLevelCache();
            if (pistaPracaConveniado == null || pistaPracaConveniado.Count() == 0)
            {
                pistaPracaConveniado = CarregarQuery();
                if (pistaPracaConveniado == null)
                    pistaPracaConveniado = new List<PistaPracaConveniadoDto>();
                else
                {
                    var cacheItem = new CacheItem("pistapracaconveniadoartesp", pistaPracaConveniado);
                    var cacheItemPolicy = new CacheItemPolicy
                    {
                        SlidingExpiration = new TimeSpan(0, 0, TimeHelper.LagSeconds)
                    };
                    _memoryCache.Add(cacheItem, cacheItemPolicy);
                }
            }

            return pistaPracaConveniado.ToList();
        }

        /// <summary>
        /// Carrega o cache de pista, praça e conveniado.
        /// </summary>        
        /// <returns>PistaPracaConveniadoDto[]</returns>
        private static IEnumerable<PistaPracaConveniadoDto> CarregarQuery()
        {           
            var ret = DataBaseConnection.HandleExecution(_pistaPracaConveniadoArtespQuery.Execute);
            return ret;
        }

        /// <summary>
        /// Carrega a lista de pista, praça e conveniado a partir do cache de 1o nivel.
        /// </summary>
        /// <returns>PistaPracaConveniadoDto[]</returns>
        private static IEnumerable<PistaPracaConveniadoDto> Carregar1StLevelCache()
        {
            var item = _memoryCache.Get("pistapracaconveniadoartesp");
            return item as IEnumerable<PistaPracaConveniadoDto>;
        }

    }
}
