using AutoMapper;
using ConectCar.Transacoes.Domain.Dto;
using ProcessadorPassagensProcessadasApi.CommandQuery.Dtos;
using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Mappers
{
    public class DetalheTRFRecusadoProfile : Profile
    {
        public DetalheTRFRecusadoProfile()
        {
            CreateMap<DetalheTRFRecusadoDto, DetalheTRFRecusadoLoteStaging>()
                .ForMember(d => d.ExecucaoId, opt => opt.MapFrom(src => new Guid()))
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.ArquivoTRFId, opt => opt.MapFrom(src => src.ArquivoTRFId))
                .ForMember(d => d.CodigoRetornoId, opt => opt.MapFrom(src => src.CodigoRetornoId))
                .ForMember(d => d.SurrogateKey, opt => opt.MapFrom(src => src.SurrogateKey))
                .ForMember(d => d.DetalheTRNId, opt => opt.MapFrom(src => src.DetalheTRNId))
                .ForMember(d => d.StagingId, opt => opt.MapFrom(src => default(int?)))
                .ForMember(d => d.Falha, opt => opt.MapFrom(src => default(bool?)))
                ;
        }
    }
}
