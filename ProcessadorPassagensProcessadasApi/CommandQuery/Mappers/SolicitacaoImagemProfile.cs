using AutoMapper;
using ConectCar.Transacoes.Domain.Dto;
using ProcessadorPassagensProcessadasApi.CommandQuery.Dtos;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Mappers
{
    public class SolicitacaoImagemProfile : Profile
    {
        public SolicitacaoImagemProfile()
        {
            CreateMap<SolicitacaoImagemDto, SolicitacaoImagemLote>()
                   .ForMember(d => d.tagId, opt => opt.MapFrom(src => src.TagId))                                     
                   ;

            CreateMap<SolicitacaoImagemDto, SolicitacaoImagemLoteStaging>()
                   .ForMember(d => d.tagId, opt => opt.MapFrom(src => src.TagId))
                   ;
        }
    }
}
