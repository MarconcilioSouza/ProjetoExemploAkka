using ConectCar.Framework.Infrastructure.Data.ServiceBus;
using ConectCar.Transacoes.Domain.Dto;

namespace LeitorPassagensProcessadasBatch.CommandQuery.Messages.EDI
{
    public sealed class PassagemAprovadaEdiMessage : PassagemAprovadaEDIDto, IMessage
    {
    }
}
