using ConectCar.Framework.Infrastructure.Data.ServiceBus;
using ConectCar.Transacoes.Domain.Dto;

namespace LeitorPassagensPendentesBatch.CommandQuery.Messages
{
    public class PassagemPendenteMessagePark : PassagemPendenteEstacionamentoDto, IMessage
    {
    }
}
