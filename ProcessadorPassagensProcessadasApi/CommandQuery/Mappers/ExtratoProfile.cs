using AutoMapper;
using ConectCar.Transacoes.Domain.Dto;
using ProcessadorPassagensProcessadasApi.CommandQuery.Dtos;
using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Mappers
{
    public class ExtratoProfile: Profile
    {
        public ExtratoProfile()
        {
            CreateMap<ExtratoDto, ExtratoLoteStaging>()
                   .ForMember(d => d.ExecucaoId, opt => opt.MapFrom(src => new Guid()))
                   .ForMember(d => d.AdesaoId, opt => opt.MapFrom(src => src.AdesaoId))
                   .ForMember(d => d.ChaveCriptografiaBanco, opt => opt.MapFrom(src => src.ChaveCriptografiaBanco))
                   .ForMember(d => d.DataTransacao, opt => opt.MapFrom(src => DateTime.Now))
                   .ForMember(d => d.Descricao, opt => opt.MapFrom(src => src.Descricao))
                   .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                   .ForMember(d => d.Placa, opt => opt.MapFrom(src => src.Placa))
                   .ForMember(d => d.SubDescricao, opt => opt.MapFrom(src => src.SubDescricao))
                   .ForMember(d => d.SurrogateKey, opt => opt.MapFrom(src => src.SurrogateKey))
                   .ForMember(d => d.TipoOperacaoId, opt => opt.MapFrom(src => src.TipoOperacaoId))
                   .ForMember(d => d.TransacaoId, opt => opt.MapFrom(src => src.TransacaoId))
                   .ForMember(d => d.ValorD, opt => opt.MapFrom(src => src.ValorD)) 
                   .ForMember(d => d.StagingId, opt => opt.MapFrom(src => default(int?)))
                   ;
        }
    }
}
