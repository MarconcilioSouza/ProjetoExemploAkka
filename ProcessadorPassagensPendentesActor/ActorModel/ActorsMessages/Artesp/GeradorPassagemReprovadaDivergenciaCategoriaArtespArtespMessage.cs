using ConectCar.Transacoes.Domain.Enum;

namespace ProcessadorPassagensActors.ActorsMessages.Artesp
{
    public class GeradorPassagemReprovadaDivergenciaCategoriaArtespArtespMessage: PassagemArtespMessageBase
    {        
        public MotivoNaoCompensado MotivoNaoCompensado { get; set; }
    }
}