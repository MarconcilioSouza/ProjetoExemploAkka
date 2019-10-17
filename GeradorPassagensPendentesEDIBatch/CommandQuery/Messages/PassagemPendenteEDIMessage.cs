using ConectCar.Framework.Infrastructure.Data.ServiceBus;
using ConectCar.Transacoes.Domain.Dto;

namespace GeradorPassagensPendentesEDIBatch.CommandQuery.Messages
{
    public class PassagemPendenteEDIMessage : PassagemPendenteEdiDto, IMessage
    {
    }
}