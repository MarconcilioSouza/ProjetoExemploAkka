using AutoMapper;
using ConectCar.Transacoes.Domain.Dto;
using ProcessadorPassagensProcessadasApi.CommandQuery.Dtos;
using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Mappers
{
    public class TransacaoRecusadaProfile: Profile
    {
        public TransacaoRecusadaProfile()
        {

            CreateMap<TransacaoRecusadaDto, TransacaoRecusadaLoteStaging>()
                   .ForMember(d => d.ExecucaoId, opt => opt.MapFrom(src => src.DataProcessamento))
                   .ForMember(d => d.DataProcessamento, opt => opt.MapFrom(src => src.DataProcessamento))
                   .ForMember(d => d.MotivoRecusadoId, opt => opt.MapFrom(src => src.MotivoRecusadoId))
                   .ForMember(d => d.PassagemId, opt => opt.MapFrom(src => src.PassagemId))
                   .ForMember(d => d.SurrogateKey, opt => opt.MapFrom(src => src.SurrogateKey));

            CreateMap<TransacaoRecusadaDto, TransacaoRecusadaLote>()
                   .ForMember(d => d.ExecucaoId, opt => opt.MapFrom(src => src.DataProcessamento))
                   .ForMember(d => d.DataProcessamento, opt => opt.MapFrom(src => src.DataProcessamento))
                   .ForMember(d => d.MotivoRecusadoId, opt => opt.MapFrom(src => src.MotivoRecusadoId))
                   .ForMember(d => d.PassagemId, opt => opt.MapFrom(src => src.PassagemId))
                   .ForMember(d => d.SurrogateKey, opt => opt.MapFrom(src => src.SurrogateKey));

        }
    }
}
