using AutoMapper;
using ConectCar.Transacoes.Domain.Dto;
using ProcessadorPassagensProcessadasApi.CommandQuery.Dtos;
using System;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Mappers
{
    public class TransacaoRecusadaParceiroProfile: Profile
    {
        public TransacaoRecusadaParceiroProfile()
        {
            CreateMap<TransacaoRecusadaParceiroEdiDto, TransacaoRecusadaParceiroLoteStaging>()
                   .ForMember(d => d.ExecucaoId, opt => opt.MapFrom(src => new Guid()))
                   .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                   .ForMember(d => d.DataEnvioAoParceiro, opt => opt.MapFrom(src => src.DataEnvioAoParceiro))
                   .ForMember(d => d.DataPassagemNaPraca, opt => opt.MapFrom(src => src.DataPassagemNaPraca))
                   .ForMember(d => d.ParceiroId, opt => opt.MapFrom(src => src.ParceiroId))
                   .ForMember(d => d.ViagemAgendadaId, opt => opt.MapFrom(src => src.ViagemAgendadaId))
                   .ForMember(d => d.Valor, opt => opt.MapFrom(src => src.Valor))
                   .ForMember(d => d.SurrogateKey, opt => opt.MapFrom(src => src.SurrogateKey))
                   .ForMember(d => d.DetalheTrnId, opt => opt.MapFrom(src => src.DetalheTRNId))
                   .ForMember(d => d.CodigoRetornoTRFId, opt => opt.MapFrom(src => src.CodigoRetornoTRFId))
                   .ForMember(d => d.StagingId, opt => opt.MapFrom(src => default(int?)))
                   ;

            CreateMap<TransacaoRecusadaParceiroDto, TransacaoRecusadaParceiroLoteStaging>()
                   .ForMember(d => d.ExecucaoId, opt => opt.MapFrom(src => new Guid()))
                   .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                   .ForMember(d => d.DataEnvioAoParceiro, opt => opt.MapFrom(src => src.DataEnvioAoParceiro))
                   .ForMember(d => d.DataPassagemNaPraca, opt => opt.MapFrom(src => src.DataPassagemNaPraca))                   
                   .ForMember(d => d.ParceiroId, opt => opt.MapFrom(src => src.ParceiroId))
                   .ForMember(d => d.ViagemAgendadaId, opt => opt.MapFrom(src => src.ViagemAgendadaId))
                   .ForMember(d => d.Valor, opt => opt.MapFrom(src => src.Valor))                   
                   .ForMember(d => d.SurrogateKey, opt => opt.MapFrom(src => src.SurrogateKey))
                   .ForMember(d => d.DetalheTrnId, opt => opt.MapFrom(src => default(int?)))
                   .ForMember(d => d.CodigoRetornoTRFId, opt => opt.MapFrom(src => default(int?)))
                   .ForMember(d => d.StagingId, opt => opt.MapFrom(src => default(int?)))
                   ;
        }
    }
}
