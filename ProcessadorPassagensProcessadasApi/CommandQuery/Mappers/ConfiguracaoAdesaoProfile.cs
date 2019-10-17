using AutoMapper;
using ConectCar.Transacoes.Domain.Dto;
using ProcessadorPassagensProcessadasApi.CommandQuery.Dtos;
using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Mappers
{
    public class ConfiguracaoAdesaoProfile : Profile
    {
        public ConfiguracaoAdesaoProfile()
        {
            CreateMap<ConfiguracaoAdesaoDto, ConfiguracaoAdesaoLoteStaging>()
                    .ForMember(d => d.ExecucaoId, opt => opt.MapFrom(src => new Guid()))
                    .ForMember(d => d.CategoriaId, opt => opt.MapFrom(src => src.CategoriaId))
                    .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(d => d.Falha, opt => opt.MapFrom(src => default(bool?)))
                    .ForMember(d => d.StagingId, opt => opt.MapFrom(src => default(int?)))
                    ;
        }
    }
}