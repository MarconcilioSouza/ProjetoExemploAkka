using AutoMapper;
using ConectCar.Transacoes.Domain.Dto;
using ConectCar.Transacoes.Domain.Model;
using ProcessadorPassagensProcessadasApi.CommandQuery.Dtos;
using ProcessadorPassagensProcessadasApi.CommandQuery.Extension;
using System;
using System.Collections.Generic;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Mappers
{
    public class TransacaoPassagemEstacionamentoProfile : Profile
    {
        public TransacaoPassagemEstacionamentoProfile()
        {
            CreateMap<TransacaoEstacionamentoDto, TransacaoPassagemEstacionamentoLote>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.Data, opt => opt.MapFrom(src => src.Data))
                .ForMember(d => d.Credito, opt => opt.MapFrom(src => src.Credito))
                .ForMember(d => d.Valor, opt => opt.MapFrom(src => src.Valor))
                .ForMember(d => d.AdesaoId, opt => opt.MapFrom(src => src.AdesaoId))
                .ForMember(d => d.StatusId, opt => opt.MapFrom(src => (int)src.Status))
                .ForMember(d => d.TipoOperacaoId, opt => opt.MapFrom(src => src.TipoOperacaoId))
                .ForMember(d => d.DataHoraTransacao, opt => opt.MapFrom(src => src.DataHoraTransacao))
                .ForMember(d => d.DataHoraEntrada, opt => opt.MapFrom(src => src.DataHoraEntrada))
                .ForMember(d => d.ValorDesconto, opt => opt.MapFrom(src => src.ValorDesconto))
                .ForMember(d => d.MotivoDesconto, opt => opt.MapFrom(src => src.MotivoDesconto))
                .ForMember(d => d.TempoPermanencia, opt => opt.MapFrom(src => src.TempoPermanencia))
                .ForMember(d => d.ValorRepasse, opt => opt.MapFrom(src => src.ValorRepasse))
                .ForMember(d => d.TarifaDeInterconexao, opt => opt.MapFrom(src => src.TarifaDeInterconexao))
                .ForMember(d => d.DataRepasse, opt => opt.MapFrom(src => src.DataRepasse))
                .ForMember(d => d.ConveniadoId, opt => opt.MapFrom(src => src.ConveniadoId))
                .ForMember(d => d.TagId, opt => opt.MapFrom(src => src.TagId))
                .ForMember(d => d.PracaId, opt => opt.MapFrom(src => src.PracaId))
                .ForMember(d => d.PistaId, opt => opt.MapFrom(src => src.PistaId))
                .ForMember(d => d.MotivoAtrasoTransmissaoId, opt => opt.MapFrom(src => src.MotivoAtrasoTransmissaoId))
                .ForMember(d => d.RepasseId, opt => opt.MapFrom(src => src.RepasseId))
                .ForMember(d => d.SerieRPS, opt => opt.MapFrom(src => src.SerieRPS))
                .ForMember(d => d.NumeroRPS, opt => opt.MapFrom(src => src.NumeroRPS))
                .ForMember(d => d.DataReferencia, opt => opt.MapFrom(src => src.DataReferencia))
                .ForMember(d => (int)d.TipoTransacaoEstacionamentoId, opt => opt.MapFrom(src => src.TipoTransacaoEstacionamentoId))
                .ForMember(d => d.Ticket, opt => opt.MapFrom(src => src.Ticket))
                .ForMember(d => d.Mensalista, opt => opt.MapFrom(src => src.Mensalista))
                .ForMember(d => d.SurrogateKey, opt => opt.MapFrom(src => src.SurrogateKey))
                .ForMember(d => d.SaldoId, opt => opt.MapFrom(src => src.SaldoId));

            CreateMap<TransacaoEstacionamentoDto, TransacaoPassagemEstacionamentoLoteStaging>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id.TryToInt()))
                .ForMember(d => d.Data, opt => opt.MapFrom(src => src.Data))
                .ForMember(d => d.Credito, opt => opt.MapFrom(src => src.Credito))
                .ForMember(d => d.Valor, opt => opt.MapFrom(src => src.Valor))
                .ForMember(d => d.AdesaoId, opt => opt.MapFrom(src => src.AdesaoId))
                .ForMember(d => d.StatusId, opt => opt.MapFrom(src => (int)src.Status))
                .ForMember(d => d.TipoOperacaoId, opt => opt.MapFrom(src => src.TipoOperacaoId))
                .ForMember(d => d.DataHoraTransacao, opt => opt.MapFrom(src => src.DataHoraTransacao))
                .ForMember(d => d.DataHoraEntrada, opt => opt.MapFrom(src => src.DataHoraEntrada))
                .ForMember(d => d.ValorDesconto, opt => opt.MapFrom(src => src.ValorDesconto))
                .ForMember(d => d.MotivoDesconto, opt => opt.MapFrom(src => src.MotivoDesconto))
                .ForMember(d => d.TempoPermanencia, opt => opt.MapFrom(src => src.TempoPermanencia))
                .ForMember(d => d.ValorRepasse, opt => opt.MapFrom(src => src.ValorRepasse))
                .ForMember(d => d.TarifaDeInterconexao, opt => opt.MapFrom(src => src.TarifaDeInterconexao))
                .ForMember(d => d.DataRepasse, opt => opt.MapFrom(src => src.DataRepasse))
                .ForMember(d => d.ConveniadoId, opt => opt.MapFrom(src => src.ConveniadoId))
                .ForMember(d => d.TagId, opt => opt.MapFrom(src => src.TagId))
                .ForMember(d => d.PracaId, opt => opt.MapFrom(src => src.PracaId))
                .ForMember(d => d.PistaId, opt => opt.MapFrom(src => src.PistaId))
                .ForMember(d => d.MotivoAtrasoTransmissaoId, opt => opt.MapFrom(src => src.MotivoAtrasoTransmissaoId))
                .ForMember(d => d.RepasseId, opt => opt.MapFrom(src => src.RepasseId))
                .ForMember(d => d.SerieRPS, opt => opt.MapFrom(src => src.SerieRPS))
                .ForMember(d => d.NumeroRPS, opt => opt.MapFrom(src => src.NumeroRPS))
                .ForMember(d => d.DataReferencia, opt => opt.MapFrom(src => src.DataReferencia))
                .ForMember(d => d.TipoTransacaoEstacionamentoId, opt => opt.MapFrom(src => src.TipoTransacaoEstacionamentoId))
                .ForMember(d => d.Ticket, opt => opt.MapFrom(src => src.Ticket))
                .ForMember(d => d.Mensalista, opt => opt.MapFrom(src => src.Mensalista))
                .ForMember(d => d.SurrogateKey, opt => opt.MapFrom(src => src.SurrogateKey))
                .ForMember(d => d.SaldoId, opt => opt.MapFrom(src => src.SaldoId));
}
    }
}
