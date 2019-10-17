using ConectCar.Framework.Infrastructure.Data.ServiceBus;
using ConectCar.Transacoes.Domain.Dto;
using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Messages.Park
{
    public class PassagemReprovadaParkMessage : PassagemReprovadaEstacionamento, IMessage
    {
    }
}