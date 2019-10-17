using AutoMapper;
using ConectCar.Transacoes.Domain.Dto;
using ProcessadorPassagensProcessadasApi.CommandQuery.Dtos;
using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Mappers
{
    public class DivergenciaCategoriaConfirmadaProfile: Profile
    {
        public DivergenciaCategoriaConfirmadaProfile()
        {
            CreateMap<DivergenciaCategoriaConfirmadaDto, DivergenciaCategoriaConfirmadaLoteStaging>()
                    .ForMember(d => d.ExecucaoId, opt => opt.MapFrom(src => new Guid()))
                    .ForMember(d => d.CategoriaVeiculoId, opt => opt.MapFrom(src => src.CategoriaVeiculoId))
                    .ForMember(d => d.Data, opt => opt.MapFrom(src => DateTime.Now))
                    .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(d => d.Surrogatekey, opt => opt.MapFrom(src => src.SurrogateKey))
                    .ForMember(d => d.TransacaoPassagemId, opt => opt.MapFrom(src => src.TransacaoPassagemId))
                    .ForMember(d => d.StagingId, opt => opt.MapFrom(src => default(int?)))
                    ;
        }
    }
}
