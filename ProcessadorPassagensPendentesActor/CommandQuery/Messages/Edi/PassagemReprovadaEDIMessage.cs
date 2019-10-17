using ConectCar.Transacoes.Domain.Dto;
using ConectCar.Framework.Infrastructure.Data.ServiceBus;

namespace ProcessadorPassagensActors.CommandQuery.Messages.Edi
{
    public class PassagemReprovadaEDIMessage : PassagemReprovadaEdiDto, IMessage
    {
    }
}
