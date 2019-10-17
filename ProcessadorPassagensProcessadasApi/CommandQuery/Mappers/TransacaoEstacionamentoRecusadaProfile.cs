using AutoMapper;
using ConectCar.Transacoes.Domain.Dto;
using ConectCar.Transacoes.Domain.Model;
using ProcessadorPassagensProcessadasApi.CommandQuery.Extension;
using ProcessadorPassagensProcessadasApi.CommandQuery.Dtos;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Mappers
{
    public class TransacaoEstacionamentoRecusadaProfile : Profile
    {
        public TransacaoEstacionamentoRecusadaProfile()
        {
            CreateMap<TransacaoEstacionamentoRecusada, TransacaoEstacionamentoRecusadaLoteStaging>()
                .ForMember(d => d.DataCadastro, opt => opt.MapFrom(src => src.DataCadastro))
                .ForMember(d => d.ConveniadoId, opt => opt.MapFrom(src => src.ConveniadoId))
                .ForMember(d => d.TagId, opt => opt.MapFrom(src => src.TagId))
                .ForMember(d => d.PistaId, opt => opt.MapFrom(src => src.PistaId))
                .ForMember(d => d.PracaId, opt => opt.MapFrom(src => src.PracaId))
                .ForMember(d => d.DataHoraTransacao, opt => opt.MapFrom(src => src.DataHoraTransacao))
                .ForMember(d => d.DataHoraEntrada, opt => opt.MapFrom(src => src.DataHoraEntrada))
                .ForMember(d => d.ValorCobrado, opt => opt.MapFrom(src => src.ValorCobrado))
                .ForMember(d => d.ValorDesconto, opt => opt.MapFrom(src => src.ValorDesconto))
                .ForMember(d => d.MotivoDesconto, opt => opt.MapFrom(src => src.MotivoDesconto))
                .ForMember(d => d.TempoPermamencia, opt => opt.MapFrom(src => src.TempoPermamencia))
                .ForMember(d => d.MotivoAtrasoTransmissao, opt => opt.MapFrom(src => src.MotivoAtrasoTransmissaoEstacionamento))
                .ForMember(d => d.MotivoRecusa, opt => opt.MapFrom(src => src.MotivoRecusaId))
                .ForMember(d => d.TipoTransacaoEstacionamento, opt => opt.MapFrom(src => src.TipoTransacaoEstacionamento.TryToInt()))
                .ForMember(d => d.Ticket, opt => opt.MapFrom(src => src.Ticket))
                .ForMember(d => d.Mensalista, opt => opt.MapFrom(src => src.Mensalista))
                .ForMember(d => d.SurrogateKey, opt => opt.MapFrom(src => src.SurrogateKey));
        }
    }
}
