using LeitorPassagensProcessadasBatch.CommandQuery.Mapper.Profiles;

namespace LeitorPassagensProcessadasBatch.CommandQuery.Mapper
{
    public static class MapperConfig
    {
        public static void RegisterMappings()
        {
            AutoMapper.Mapper.Initialize(cfg =>
             {
                 cfg.AddProfile<MapperProfiles>();
             });
        }
    }
}
