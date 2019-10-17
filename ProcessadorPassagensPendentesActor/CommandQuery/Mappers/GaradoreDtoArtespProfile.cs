using AutoMapper;
using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Mappers
{
    public class GaradoreDtoArtespProfile : Profile
    {
        public GaradoreDtoArtespProfile()
        {
            CreateMap<PassagemPendenteArtesp, PassagemReprovadaArtesp>();

            CreateMap<PassagemPendenteArtesp, PassagemAprovadaArtesp>();

            //CreateMap<PassagemReprovada, PassagemReprovadaMessage>();

            //CreateMap<PassagemInvalida, PassagemInvalidaMessage>();

            



        }
    }
}
