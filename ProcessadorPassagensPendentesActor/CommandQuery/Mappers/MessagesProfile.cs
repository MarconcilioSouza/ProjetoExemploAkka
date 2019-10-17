using AutoMapper;
using ConectCar.Transacoes.Domain.Dto;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Messages.Artesp;
using ProcessadorPassagensActors.CommandQuery.Messages.Edi;
using ProcessadorPassagensActors.CommandQuery.Messages.Park;

namespace ProcessadorPassagensActors.CommandQuery.Mappers
{
    public class MessagesProfile : Profile
    {
        public MessagesProfile()
        {
            CreateMap<PassagemAprovadaArtespDto, PassagemAprovadaMessage>();
            CreateMap<PassagemReprovadaArtespDto, PassagemReprovadaMessage>();            
            CreateMap<PassagemInvalidaArtespDto, PassagemInvalidaMessage>();

            CreateMap<PassagemAprovadaEDIDto, PassagemAprovadaEDIMessage>();
            CreateMap<PassagemReprovadaEdiDto, PassagemReprovadaEDIMessage>();            

            CreateMap<PassagemAprovadaEstacionamentoDto, PassagemAprovadaParkMessage>();
            CreateMap<PassagemReprovadaEstacionamento, PassagemReprovadaParkMessage>();

        }
    }
}
