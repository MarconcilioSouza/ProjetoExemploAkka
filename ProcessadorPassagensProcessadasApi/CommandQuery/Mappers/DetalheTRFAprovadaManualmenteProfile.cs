using AutoMapper;
using ConectCar.Transacoes.Domain.Dto;
using ProcessadorPassagensProcessadasApi.CommandQuery.Dtos;
using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Mappers
{
    public class DetalheTRFAprovadaManualmenteProfile : Profile
    {
        public DetalheTRFAprovadaManualmenteProfile()
        {
            CreateMap<DetalheTRFAprovadoManualmenteDto, DetalheTRFAprovadaManualmenteLoteStaging>()
                .ForMember(d => d.ExecucaoId, opt => opt.MapFrom(src => new Guid()))
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.StagingId, opt => opt.MapFrom(src => default(int?)))
                .ForMember(d => d.TransacaoPassagemId, opt => opt.MapFrom(src => src.TransacaoPassagemId))
                .ForMember(d => d.SurrogateKey, opt => opt.MapFrom(src => src.SurrogateKey));
        }
    }
}
