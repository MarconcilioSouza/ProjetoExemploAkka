using AutoMapper;
using ConectCar.Transacoes.Domain.Dto;
using ConectCar.Transacoes.Domain.Model;
using ProcessadorPassagensProcessadasApi.CommandQuery.Dtos;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Mappers
{
    public class ConveniadoInformacoesRpsProfile : Profile
    {
        public ConveniadoInformacoesRpsProfile()
        {
            CreateMap<ConveniadoInformacoesRpsDto, ConveniadoInformacoesRpsLote>()
                .ForMember(d => d.SerieRPS, opt => opt.MapFrom(src => src.SerieRPS))
                .ForMember(d => d.NumeroRPS, opt => opt.MapFrom(src => src.NumeroRPS))
                .ForMember(d => d.TipoRps, opt => opt.MapFrom(src => src.TipoRps));

            CreateMap<ConveniadoInformacoesRpsDto, ConveniadoInformacoesRpsLoteStaging>()
                .ForMember(d => d.SerieRPS, opt => opt.MapFrom(src => src.SerieRPS))
                .ForMember(d => d.NumeroRPS, opt => opt.MapFrom(src => src.NumeroRPS))
                .ForMember(d => d.TipoRps, opt => opt.MapFrom(src => src.TipoRps));

        }
    }
}
