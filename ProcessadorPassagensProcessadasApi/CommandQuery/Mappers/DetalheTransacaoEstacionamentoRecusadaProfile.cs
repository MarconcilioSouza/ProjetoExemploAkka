using AutoMapper;
using ConectCar.Transacoes.Domain.Dto;
using ConectCar.Transacoes.Domain.Model;
using ProcessadorPassagensProcessadasApi.CommandQuery.Dtos;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Mappers
{
    public class DetalheTransacaoEstacionamentoRecusadaProfile : AceiteManualReenvioPassagemProfile
    {
        public DetalheTransacaoEstacionamentoRecusadaProfile()
        {
          
            CreateMap<DetalheTransacaoEstacionamentoRecusada, DetalheTransacaoEstacionamentoRecusadaLoteStaging>()
                .ForMember(d => d.DataHoraPassagem, opt => opt.MapFrom(src => src.DataHoraPassagem))
                .ForMember(d => d.PistaId, opt => opt.MapFrom(src => src.Pista))
                .ForMember(d => d.PracaId, opt => opt.MapFrom(src => src.Praca))
                .ForMember(d => d.DataHoraPassagem, opt => opt.MapFrom(src => src.DataHoraPassagem))
                .ForMember(d => d.SurrogateKey, opt => opt.MapFrom(src => src.SurrogateKey));
        }
        
    }
}
