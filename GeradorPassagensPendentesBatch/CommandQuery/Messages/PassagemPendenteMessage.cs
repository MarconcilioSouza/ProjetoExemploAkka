using ConectCar.Framework.Infrastructure.Data.ServiceBus;
using ConectCar.Transacoes.Domain.Dto;
using System.Runtime.Serialization;

namespace GeradorPassagensPendentesBatch.CommandQuery.Messages
{
    public class PassagemPendenteMessage : PassagemPendenteArtespDto, IMessage
    {
        [IgnoreDataMember]
        public int? TempoSLAEnvioPassagem { get; set; }
    }
}