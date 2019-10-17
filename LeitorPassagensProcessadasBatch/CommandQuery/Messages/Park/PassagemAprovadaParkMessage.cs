using ConectCar.Framework.Infrastructure.Data.ServiceBus;
using ConectCar.Transacoes.Domain.Dto;
using ConectCar.Transacoes.Domain.Model;
using ConectCar.Transacoes.Domain.ValueObject;

namespace LeitorPassagensProcessadasBatch.CommandQuery.Messages.Park
{
    public sealed class PassagemAprovadaParkMessage : PassagemAprovadaEstacionamentoDto, IMessage
    {
    }
}
