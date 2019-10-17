using ConectCar.Transacoes.Domain.Enum;

namespace ProcessadorPassagensActors.ActorsMessages.Artesp
{
    public class GeradorPassagemPendenteReprovadaArtespMessage: PassagemArtespMessageBase
    {        
        public MotivoNaoCompensado MotivoNaoCompensado { get; set; }

    }
}