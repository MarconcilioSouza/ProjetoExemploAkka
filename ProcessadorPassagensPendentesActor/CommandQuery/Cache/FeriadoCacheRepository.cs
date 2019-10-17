using ProcessadorPassagensActors.CommandQuery.Dtos;
using ProcessadorPassagensActors.CommandQuery.Queries;
using ProcessadorPassagensActors.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using ProcessadorPassagensActors.CommandQuery.Connections;

namespace ProcessadorPassagensActors.CommandQuery.Cache
{
    public static class FeriadoCacheRepository
    {

        
        private static MemoryCache _memoryCache;
        private static ObterFeriadosQuery _feriadoQuery;


        static FeriadoCacheRepository()
        {
            _memoryCache = new MemoryCache("FeriadosMemoryCache");
            _feriadoQuery = new ObterFeriadosQuery();
        }

        public static bool EhFeriado(DateTime data)
        {
            var feriados = ListarFeriadosAnoPassadoAnoQueVem();
            return feriados.Any(x => x.Data.Date == data.Date);
        }

        public static List<FeriadoDto> ListarFeriadosAnoPassadoAnoQueVem()
        {
            var feriados = Listar();
            var dateInit = DateTime.Now.AddYears(-1);
            var dateFim = DateTime.Now.AddYears(1);
            return feriados.Where(x => x.Data >= dateInit.Date && x.Data <= dateFim.Date).ToList();
        }

        public static List<FeriadoDto> ListarFeriadosProximos2Anos(DateTime data)
        {
            var feriados = Listar();
            var dateInit = data;
            var dateFim = data.AddYears(2);
            return feriados.Where(x => x.Data >= dateInit.Date && x.Data <= dateFim.Date).ToList();
        }

        public static List<FeriadoDto> Listar()
        {
            var feriados = Carregar1StLevelCache();
            if (feriados == null)
            {
                feriados = CarregarQuery();
                if (feriados == null)
                    feriados = new List<FeriadoDto>();
                else
                {
                    var cacheItem = new CacheItem("feriados", feriados);
                    var cacheItemPolicy = new CacheItemPolicy
                    {
                        SlidingExpiration = new TimeSpan(0, 0, TimeHelper.LagSeconds)
                    };
                    _memoryCache.Add(cacheItem, cacheItemPolicy);
                }
            }

            return feriados.ToList();
        }

        private static IEnumerable<FeriadoDto> CarregarQuery()
        {            
            var ret = DataBaseConnection.HandleExecution(_feriadoQuery.Execute);
            return ret;
        }

        private static IEnumerable<FeriadoDto> Carregar1StLevelCache()
        {
            var item = _memoryCache.Get("feriados");
            return item as IEnumerable<FeriadoDto>;
        }
    }
}
