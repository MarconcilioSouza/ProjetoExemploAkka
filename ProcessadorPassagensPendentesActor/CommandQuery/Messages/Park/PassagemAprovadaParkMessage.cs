using ConectCar.Framework.Infrastructure.Data.ServiceBus;
using ConectCar.Transacoes.Domain.Dto;

namespace ProcessadorPassagensActors.CommandQuery.Messages.Park
{
    public class PassagemAprovadaParkMessage : PassagemAprovadaEstacionamentoDto, IMessage
    {
    }
}