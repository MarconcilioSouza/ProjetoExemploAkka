using System;
using ConectCar.Cadastros.Conveniados.Backend.CommonQuery.Cache;
using ConectCar.Cadastros.Domain.Enum;
using ConectCar.Framework.Infrastructure.Data.Cache.DataProviders;
using ConectCar.Framework.Infrastructure.Data.DataProviders;

namespace ProcessadorPassagensActors.CommandQuery.Cache
{
    public class CommonCacheDataSourceProvider : ICommonCacheDataSourceProvider
    {
        public IDataSource GetDataSourceTyped(Enum typeEntityCacheble)
        {
            switch (typeEntityCacheble)
            {
                case TypeCadastrosCacheble.Pracas:
                    return new PracaCacheDataSource();
                default:
                    return new PracaCacheDataSource();
            }
        }
    }
}
