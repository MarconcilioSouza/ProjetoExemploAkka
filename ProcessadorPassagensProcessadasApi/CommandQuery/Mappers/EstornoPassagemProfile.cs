using AutoMapper;
using ConectCar.Transacoes.Domain.Dto;
using ProcessadorPassagensProcessadasApi.CommandQuery.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Mappers
{
    public class EstornoPassagemProfile: Profile
    {
        public EstornoPassagemProfile()
        {
            CreateMap<EstornoPassagemDto, EstornoPassagemLoteStaging>()
                    .ForMember(d => d.ExecucaoId, opt => opt.MapFrom(src => new Guid()))
                    .ForMember(d => d.AdesaoId, opt => opt.MapFrom(src => src.AdesaoId))
                    .ForMember(d => d.ChaveCriptografiaBanco, opt => opt.MapFrom(src => src.ChaveCriptografiaBanco))
                    .ForMember(d => d.Data, opt => opt.MapFrom(src => DateTime.Now))
                    .ForMember(d => d.DataEstorno, opt => opt.MapFrom(src => src.DataEstorno))
                    .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(d => d.SaldoId, opt => opt.MapFrom(src => src.SaldoId))
                    .ForMember(d => d.SomenteInformacoesAlterada, opt => opt.MapFrom(src => src.SomenteInformacoesAlteradas))
                    .ForMember(d => d.StatusId, opt => opt.MapFrom(src => src.StatusId))
                    .ForMember(d => d.SurrogateKey, opt => opt.MapFrom(src => src.SurroGateKey))
                    .ForMember(d => d.TipoOperacaoId, opt => opt.MapFrom(src => src.TipoOperacaoId))
                    .ForMember(d => d.TransacaoPassagemOriginalId, opt => opt.MapFrom(src => src.TransacaoPassagemOrigemlId))
                    .ForMember(d => d.Valor, opt => opt.MapFrom(src => src.Valor))
                    .ForMember(d => d.StagingId, opt => opt.MapFrom(src => default(int?)))
                    ;
        }
    }
}
