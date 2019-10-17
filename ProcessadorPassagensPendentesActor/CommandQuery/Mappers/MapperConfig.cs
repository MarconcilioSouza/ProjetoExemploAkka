using AutoMapper;

namespace ProcessadorPassagensActors.CommandQuery.Mappers
{
    public static class MapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<PassagemPendenteArtespProfile>();
                cfg.AddProfile<PassagemPendenteEdiProfile>();
                cfg.AddProfile<GaradoreDtoArtespProfile>();
                cfg.AddProfile<GeradorDtoEdiProfile>();
                cfg.AddProfile<MessagesProfile>();
                cfg.AddProfile<PassagemPendenteParkProfile>();
            });
        }
    }
}
