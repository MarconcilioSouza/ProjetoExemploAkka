using AutoMapper;
using ConectCar.Transacoes.Domain.Dto;
using ProcessadorPassagensProcessadasApi.CommandQuery.Dtos;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Mappers
{
    public class AceiteManualReenvioPassagemProfile: Profile
    {
        public AceiteManualReenvioPassagemProfile()
        {
            
            CreateMap<AceiteManualReenvioPassagemDto, AceiteManualReenvioPassagemLote>()
                    .ForMember(d => d.DataProcessamento, opt => opt.MapFrom(src => src.DataProcessamento ))
                    .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(d => d.Processado, opt => opt.MapFrom(src => src.Processado));

            CreateMap<AceiteManualReenvioPassagemDto, AceiteManualReenvioPassagemLoteStaging>()
                    .ForMember(d => d.DataProcessamento, opt => opt.MapFrom(src => src.DataProcessamento))
                    .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(d => d.Processado, opt => opt.MapFrom(src => src.Processado));
        }

    }
}
