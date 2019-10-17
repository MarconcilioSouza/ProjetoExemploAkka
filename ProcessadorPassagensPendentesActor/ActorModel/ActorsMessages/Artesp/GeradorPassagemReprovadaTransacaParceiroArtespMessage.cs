using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.Model;
using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.ActorsMessages.Artesp
{
    public class GeradorPassagemReprovadaTransacaParceiroArtespMessage: PassagemArtespMessageBase
    {       
        public MotivoNaoCompensado MotivoNaoCompensado { get; set; }
        public int DetalheViagemId { get; set; }
    }
}