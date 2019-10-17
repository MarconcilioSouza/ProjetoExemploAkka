using AutoMapper;
using ConectCar.Transacoes.Domain.Dto;
using ConectCar.Transacoes.Domain.Model;
using ProcessadorPassagensProcessadasApi.CommandQuery.Dtos;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Mappers
{
    public class PistaInformacoesRpsProfile : Profile
    {
        public PistaInformacoesRpsProfile()
        {
            CreateMap<PistaInformacoesRPSDto, PistaInformacoesRPSLote>()
                .ForMember(d => d.ConveniadoInformacoesRPSId, opt => opt.MapFrom(src => src.ConveniadoInformacoesRPSId))
                .ForMember(d => d.SerieRPS, opt => opt.MapFrom(src => src.SerieRPS))
                .ForMember(d => d.NumeroRPS, opt => opt.MapFrom(src => src.NumeroRPS))
                .ForMember(d => d.PistaId, opt => opt.MapFrom(src => src.PistaId))
                .ForMember(d => d.DataCriacao, opt => opt.MapFrom(src => src.DataCriacao));

            CreateMap<PistaInformacoesRPSDto, PistaInformacoesRPSLoteStaging>()
                .ForMember(d => d.ConveniadoInformacoesRPSId, opt => opt.MapFrom(src => src.ConveniadoInformacoesRPSId))
                .ForMember(d => d.SerieRPS, opt => opt.MapFrom(src => src.SerieRPS))
                .ForMember(d => d.NumeroRPS, opt => opt.MapFrom(src => src.NumeroRPS))
                .ForMember(d => d.PistaId, opt => opt.MapFrom(src => src.PistaId))
                .ForMember(d => d.DataCriacao, opt => opt.MapFrom(src => src.DataCriacao));
        }
    }
}
