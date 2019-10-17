using AutoMapper;
using ConectCar.Transacoes.Domain.Dto;
using ProcessadorPassagensProcessadasApi.CommandQuery.Dtos;
using ProcessadorPassagensProcessadasApi.CommandQuery.Extension;
using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Mappers
{
    public class TransacaoPassagemEdiProfile : Profile
    {
        public TransacaoPassagemEdiProfile()
        {
            CreateMap<TransacaoPassagemEDIDto, TransacaoPassagemLoteStaging>()
                .ForMember(d => d.ExecucaoId, opt => opt.MapFrom(src => new Guid()))
                .ForMember(d => d.Data, opt => opt.MapFrom(src => src.Data))
                .ForMember(d => d.AdesaoId, opt => opt.MapFrom(src => src.AdesaoId))
                .ForMember(d => d.CategoriaUtilizadaId, opt => opt.MapFrom(src => src.CategoriaUtilizadaId))
                .ForMember(d => d.ChaveCriptografiaBanco, opt => opt.MapFrom(src => src.ChaveCriptografiaBanco))
                .ForMember(d => d.Credito, opt => opt.MapFrom(src => src.Credito))
                .ForMember(d => d.DataDePassagem, opt => opt.MapFrom(src => src.DataDePassagem))
                .ForMember(d => d.DataRepasse, opt => opt.MapFrom(src => src.DataRepasse))
                .ForMember(d => d.DetalheTRNId, opt => opt.MapFrom(src => src.DetalheTRNId.TryToLong()))
                .ForMember(d => d.EvasaoAceita, opt => opt.MapFrom(src => src.EvasaoAceita))
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.ItemListaDeParaUtilizado, opt => opt.MapFrom(src => src.ItemListaDeParaUtilizado.TryToInt()))
                .ForMember(d => d.PistaId, opt => opt.MapFrom(src => src.PistaId))
                .ForMember(d => d.OrigemTransacaoId, opt => opt.MapFrom(src => src.OrigemTransacaoId))
                .ForMember(d => d.RepasseId, opt => opt.MapFrom(src => src.RepasseId))
                .ForMember(d => d.SaldoId, opt => opt.MapFrom(src => src.SaldoId))
                .ForMember(d => d.StatusId, opt => opt.MapFrom(src => src.StatusId))
                .ForMember(d => d.SurrogateKey, opt => opt.MapFrom(src => src.SurroGateKey))
                .ForMember(d => d.TarifaDeInterconexao, opt => opt.MapFrom(src => src.TarifaDeinterconexao))
                .ForMember(d => d.TransacaoDeCorrecaoId, opt => opt.MapFrom(src => src.TransacaoDeCorrecaoId))
                .ForMember(d => d.TipoOperacaoId, opt => opt.MapFrom(src => src.TipoOperacaoId))
                .ForMember(d => d.TransacaoProvisoria, opt => opt.MapFrom(src => false))
                .ForMember(d => d.Valor, opt => opt.MapFrom(src => src.Valor))
                .ForMember(d => d.Falha, opt => opt.MapFrom(src => false))
                .ForMember(d => d.ValorRepasse, opt => opt.MapFrom(src => src.ValorRepasse));
        }
    }
}
