using AutoMapper;
using ConectCar.Transacoes.Domain.Dto;
using ConectCar.Transacoes.Domain.Model;
using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Mappers
{
    public class GeradorDtoEdiProfile : Profile
    {
        public GeradorDtoEdiProfile()
        {

            #region DetalheViagem => PassagemAprovadaEDIDto
            CreateMap<DetalheViagem, PassagemAprovadaEDIDto>()
                .ForMember(d => d.Viagens, o => o.MapFrom(s => new DetalheViagemDto
                {
                    CodigoPracaRoadCard = s.CodigoPracaRoadCard,
                    DataCancelamento = s.DataCancelamento,
                    PracaId = s.PracaId,
                    Id = s.Id.TryToInt(),
                    Sequencia = s.Sequencia,
                    StatusId = s.StatusDetalheViagemId,
                    ViagemId = s.Viagem.Id.TryToInt(),
                    ValorPassagem = s.ValorPassagem,
                }));
            #endregion

            #region Evento => PassagemAprovadaEDIDto
            CreateMap<Evento, PassagemAprovadaEDIDto>()
                .ForMember(d => d.EventoPrimeiraPassagemManual, o => o.MapFrom(s => new EventoDto
                {
                    Id = s.Id.TryToInt(),
                    DataCriacao = s.DataCriacao,
                    IdRegistro = s.IdRegistro,
                    Processado = s.Processado,
                    TipoEventoId = s.TipoEvento.TryToInt(),
                }));
            #endregion
        }
    }
}
