using ConectCar.Framework.Infrastructure.Data.ServiceBus;
using ConectCar.Transacoes.Domain.Dto;

namespace ProcessadorPassagensActors.CommandQuery.Messages.Artesp
{
    public class PassagemReprovadaMessage : PassagemReprovadaArtespDto, IMessage
    {
    }
}
