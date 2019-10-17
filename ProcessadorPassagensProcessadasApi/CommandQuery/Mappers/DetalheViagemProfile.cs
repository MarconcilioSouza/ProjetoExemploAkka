using AutoMapper;
using ConectCar.Transacoes.Domain.Dto;
using ProcessadorPassagensProcessadasApi.CommandQuery.Dtos;
using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Mappers
{
    public class DetalheViagemProfile: Profile
    {
        public DetalheViagemProfile()
        {
            CreateMap<DetalheViagemDto, DetalheViagemLoteStaging>()
                    .ForMember(d => d.ExecucaoId, opt => opt.MapFrom(src => new Guid()))
                    .ForMember(d => d.CodigoPracaRoadcard, opt => opt.MapFrom(src => src.CodigoPracaRoadCard))
                    .ForMember(d => d.DataCancelamento, opt => opt.MapFrom(src => src.DataCancelamento))
                    .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                    //.ForMember(d => d.StagingId, opt => opt.MapFrom(src => default(int?)))
                    .ForMember(d => d.PracaId, opt => opt.MapFrom(src => src.PracaId))
                    .ForMember(d => d.Sequencia, opt => opt.MapFrom(src => src.Sequencia))
                    .ForMember(d => d.StatusId, opt => opt.MapFrom(src => src.StatusId))
                    .ForMember(d => d.SurrogateKey, opt => opt.MapFrom(src => src.SurrogateKey))
                    .ForMember(d => d.ValorPassagem, opt => opt.MapFrom(src => src.ValorPassagem))
                    .ForMember(d => d.ViagemID, opt => opt.MapFrom(src => src.ViagemId))
                    ;
        }
    }
}
