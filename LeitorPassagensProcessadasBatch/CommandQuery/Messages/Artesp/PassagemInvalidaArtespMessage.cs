using ConectCar.Framework.Infrastructure.Data.ServiceBus;
using ConectCar.Transacoes.Domain.Dto;

namespace LeitorPassagensProcessadasBatch.CommandQuery.Messages.Artesp
{
    public class PassagemInvalidaArtespMessage : PassagemInvalidaArtespDto, IMessage
    {
    }
}
