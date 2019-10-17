using AutoMapper;
using ConectCar.Transacoes.Domain.Dto;
using ProcessadorPassagensProcessadasApi.CommandQuery.Dtos;
using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Mappers
{
    public class TransacaoPassagemProfile : Profile
    {
        public TransacaoPassagemProfile()
        {
            CreateMap<TransacaoPassagemDto, TransacaoPassagemLoteStaging>()
                   .ForMember(d => d.ExecucaoId, opt => opt.MapFrom(src => new Guid()))
                   .ForMember(d => d.AdesaoId, opt => opt.MapFrom(src => src.AdesaoId))
                   .ForMember(d => d.CategoriaUtilizadaId, opt => opt.MapFrom(src => src.CategoriaUtilizadaId))
                   .ForMember(d => d.Credito, opt => opt.MapFrom(src => src.Credito))
                   .ForMember(d => d.Data, opt => opt.MapFrom(src => DateTime.Now))
                   .ForMember(d => d.DataDePassagem, opt => opt.MapFrom(src => src.DataDePassagem))
                   .ForMember(d => d.DataRepasse, opt => opt.MapFrom(src => DateTime.Now))
                   .ForMember(d => d.EstornoId, opt => opt.Ignore())
                   .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                   .ForMember(d => d.ItemListaDeParaUtilizado, opt => opt.MapFrom(src => src.ItemListaDeParaUtilizado))
                   .ForMember(d => d.OrigemTransacaoId, opt => opt.MapFrom(src => src.OrigemTransacaoId))
                   .ForMember(d => d.PassagemId, opt => opt.Ignore())
                   .ForMember(d => d.RepasseId, opt => opt.MapFrom(src => src.RepasseId))
                   .ForMember(d => d.StatusId, opt => opt.MapFrom(src => src.StatusId))
                   .ForMember(d => d.SaldoId, opt => opt.MapFrom(src => src.SaldoId))
                   .ForMember(d => d.SurrogateKey, opt => opt.MapFrom(src => src.SurroGateKey))
                   .ForMember(d => d.TarifaDeInterconexao, opt => opt.MapFrom(src => src.TarifaDeinterconexao))
                   .ForMember(d => d.TipoOperacaoId, opt => opt.MapFrom(src => src.TipoOperacaoId))
                   .ForMember(d => d.Valor, opt => opt.MapFrom(src => src.Valor))
                   .ForMember(d => d.ValorRepasse, opt => opt.MapFrom(src => src.ValorRepasse));
        }
    }
}
