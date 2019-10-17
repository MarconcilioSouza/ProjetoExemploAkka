using AutoMapper;
using ConectCar.Transacoes.Domain.Dto;
using ProcessadorPassagensProcessadasApi.CommandQuery.Dtos;
using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Mappers
{
    public class EventoProfile: Profile
    {
        public EventoProfile()
        {
            CreateMap<EventoDto, EventoLoteStaging>()
                   .ForMember(d => d.ExecucaoId, opt => opt.MapFrom(src => new Guid()))
                   .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                   .ForMember(d => d.DataCriacao, opt => opt.MapFrom(src => src.DataCriacao))
                   .ForMember(d => d.IdRegistro, opt => opt.MapFrom(src => src.IdRegistro))                   
                   .ForMember(d => d.Processado, opt => opt.MapFrom(src => src.Processado))
                   .ForMember(d => d.TipoEventoId, opt => opt.MapFrom(src => src.TipoEventoId))
                   .ForMember(d => d.Falha, opt => opt.MapFrom(src => default(bool?)))
                   .ForMember(d => d.StagingId, opt => opt.MapFrom(src => default(int?)))
                   ;
        }
    }
}
