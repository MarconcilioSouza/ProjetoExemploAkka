using ConectCar.Transacoes.Domain.Enum;

namespace ProcessadorPassagensActors.ActorsMessages.Artesp
{
    public class GeradorPassagemReprovadaArtespMessage: PassagemArtespMessageBase
    {        
        public MotivoNaoCompensado MotivoNaoCompensado { get; set; }
    }
}