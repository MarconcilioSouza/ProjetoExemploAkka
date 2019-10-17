using ConectCar.Framework.Infrastructure.Data.ServiceBus;
using ConectCar.Transacoes.Domain.Dto;

namespace GeradorPassagensPendentesParkBatch.CommandQuery.Messages
{
    public class PassagemPendenteParkMessage : PassagemPendenteEstacionamentoDto, IMessage
    {
    }
}