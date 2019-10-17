using AutoMapper;
using ConectCar.Transacoes.Domain.Dto;
using ConectCar.Transacoes.Domain.Model;
using ProcessadorPassagensProcessadasApi.CommandQuery.Dtos;
using ProcessadorPassagensProcessadasApi.CommandQuery.Extension;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Mappers
{
    public class DetalhePassagemEstacionamentoProfile : Profile
    {
        public DetalhePassagemEstacionamentoProfile()
        {
            CreateMap<DetalhePassagemEstacionamento, DetalhePassagemEstacionamentoLote>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id.TryToInt()))
                .ForMember(d => d.DataHoraPassagem, opt => opt.MapFrom(src => src.DataHoraPassagem))
                .ForMember(d => d.PistaId, opt => opt.MapFrom(src => src.PistaId.TryToInt()))
                .ForMember(d => d.TransacaoEstacionamentoId, opt => opt.MapFrom(src => src.TransacaoEstacionamentoId.TryToInt()))
                .ForMember(d => d.SurrogateKey, opt => opt.MapFrom(src => src.SurrogateKey));

            CreateMap<DetalhePassagemEstacionamento, DetalhePassagemEstacionamentoLoteStaging>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id.TryToInt()))
                .ForMember(d => d.DataHoraPassagem, opt => opt.MapFrom(src => src.DataHoraPassagem))
                .ForMember(d => d.PistaId, opt => opt.MapFrom(src => src.PistaId))
                .ForMember(d => d.TransacaoEstacionamentoId, opt => opt.MapFrom(src => src.TransacaoEstacionamentoId.TryToInt()))
                .ForMember(d => d.SurrogateKey, opt => opt.MapFrom(src => src.SurrogateKey));
        }
    }
}
