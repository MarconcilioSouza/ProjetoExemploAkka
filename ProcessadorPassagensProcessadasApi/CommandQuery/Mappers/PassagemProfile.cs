using AutoMapper;
using ConectCar.Transacoes.Domain.Dto;
using ProcessadorPassagensProcessadasApi.CommandQuery.Dtos;
using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Mappers
{
    public class PassagemProfile: Profile
    {
        public PassagemProfile()
        {
            CreateMap<PassagemDto, PassagemLoteStaging>()
                   .ForMember(d => d.ExecucaoId, opt => opt.MapFrom(src => new Guid()))
                   .ForMember(d => d.AdesaoId, opt => opt.MapFrom(src => src.AdesaoId))
                   .ForMember(d => d.CategoriaCobradaId, opt => opt.MapFrom(src => src.CategoriaCobradaId))
                   .ForMember(d => d.CategoriaDetectadaId, opt => opt.MapFrom(src => src.CategoriaDetectadaId))
                   .ForMember(d => d.CategoriaTagId, opt => opt.MapFrom(src => src.CategoriaTagId))
                   .ForMember(d => d.CodigoPassagemConveniado, opt => opt.MapFrom(src => src.CodigoPassagemConveniado))
                   .ForMember(d => d.CodigoPista, opt => opt.MapFrom(src => src.CodigoPista))
                   .ForMember(d => d.CodigoPraca, opt => opt.MapFrom(src => src.CodigoPraca))
                   .ForMember(d => d.ConveniadoId, opt => opt.MapFrom(src => src.ConveniadoId))
                   .ForMember(d => d.Data, opt => opt.MapFrom(src => src.Data))
                   .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                   .ForMember(d => d.MensagemItemId, opt => opt.MapFrom(src => src.MensagemItemId))
                   .ForMember(d => d.MotivoSemValorId, opt => opt.MapFrom(src => src.MotivoSemvalorId))
                   .ForMember(d => d.PassagemRecusadaMensageria, opt => opt.MapFrom(src => src.PassagemRecusadaMensageria))
                   .ForMember(d => d.Placa, opt => opt.MapFrom(src => src.Placa))
                   .ForMember(d => d.Reenvio, opt => opt.MapFrom(src => src.Reenvio))
                   .ForMember(d => d.SomenteoInformacoesAlteradas, opt => opt.MapFrom(src => src.SomenteInformacoesAlteradas))
                   .ForMember(d => d.StatusCobrancaId, opt => opt.MapFrom(src => src.StatusCobrancaId))
                   .ForMember(d => d.StatusPassagemId, opt => opt.MapFrom(src => src.StatusPassagemId))
                   .ForMember(d => d.SurrogateKey, opt => opt.MapFrom(src => src.SurrogateKey))
                   .ForMember(d => d.TagBloqueadaMomentoRecebimento, opt => opt.MapFrom(src => src.TagBloqueadaMomentoRecebimento))
                   .ForMember(d => d.TagId, opt => opt.MapFrom(src => src.TagId))
                   .ForMember(d => d.Valor, opt => opt.MapFrom(src => src.Valor))
                   ;
        }
    }
}
