using AutoMapper;
using ConectCar.Transacoes.Domain.Dto;
using ProcessadorPassagensProcessadasApi.CommandQuery.Dtos;
using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Mappers
{
    public class VeiculoProfile: Profile
    {
        public VeiculoProfile()
        {
            CreateMap<VeiculoDto, VeiculoLoteStaging>()
                   .ForMember(d => d.ExecucaoId, opt => opt.MapFrom(src => new Guid()))
                   .ForMember(d => d.CategoriaConfirmada, opt => opt.MapFrom(src => src.CategoriaConfirmada))
                   .ForMember(d => d.CategoriaVeiculoId, opt => opt.MapFrom(src => src.CategoriaVeiculoId))
                   .ForMember(d => d.ContagemConfirmadaCategoria, opt => opt.MapFrom(src => src.ContagemConfirmacaoCategoria))
                   .ForMember(d => d.ContagemDivergenciaConfirmada, opt => opt.MapFrom(src => src.ContagemDivergenciaCategoriaConfirmada))
                   .ForMember(d => d.DataConfirmacaoCategoria, opt => opt.MapFrom(src => src.DataConfirmacaoCategoria))
                   .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                   .ForMember(d => d.StagingId, opt => opt.MapFrom(src => default(int?)))                 
                   .ForMember(d => d.Placa, opt => opt.MapFrom(src => src.Placa))                   
                   .ForMember(d => d.CategoriaId, opt => opt.MapFrom(src => src.CategoriaVeiculoId))
                   .ForMember(d => d.Falha, opt => opt.MapFrom(src => false))
                   .ForMember(d => d.Motivo, opt => opt.MapFrom(src => default(string)))
                   ;
        }
    }
}
