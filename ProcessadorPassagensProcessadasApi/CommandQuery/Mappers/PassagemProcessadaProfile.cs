using AutoMapper;
using ConectCar.Transacoes.Domain.Dto;
using ProcessadorPassagensProcessadasApi.CommandQuery.Dtos;
using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Mappers
{
    public class PassagemProcessadaProfile: Profile
    {
        public PassagemProcessadaProfile()
        {

            CreateMap<PassagemProcessadaArtespDto, PassagemProcessadaLoteStaging>()
                   .ForMember(d => d.ExecucaoId, opt => opt.MapFrom(src => new Guid()))
                   .ForMember(d => d.DataPagamento, opt => opt.MapFrom(src => src.DataPagamento))
                   .ForMember(d => d.MensagemId, opt => opt.Ignore())
                   .ForMember(d => d.MensagemItemId, opt => opt.MapFrom(src => src.MensagemItemId))
                   .ForMember(d => d.MotivoNaoCompensado, opt => opt.MapFrom(src => src.MotivoNaoCompensado))
                   .ForMember(d => d.MotivoOutroValor, opt => opt.MapFrom(src => src.MotivoOutroValor))
                   .ForMember(d => d.MotivoProvisionado, opt => opt.MapFrom(src => src.MotivoProvisionado))
                   .ForMember(d => d.PassagemId, opt => opt.MapFrom(src => src.MensagemItemId))
                   .ForMember(d => d.Reenvio, opt => opt.MapFrom(src => src.Reenvio))
                   .ForMember(d => d.Resultado, opt => opt.MapFrom(src => src.Resultado))
                   .ForMember(d => d.TransacaoId, opt => opt.MapFrom(src => src.TransacaoId))
                   .ForMember(d => d.ValePedagio, opt => opt.MapFrom(src => src.ValePedagio))
                   .ForMember(d => d.Valor, opt => opt.MapFrom(src => src.Valor))
                   ;
        }
    }
}
