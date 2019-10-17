using ConectCar.Transacoes.Domain.Model;

namespace ProcessadorPassagensActors.ActorsMessages.Artesp
{
    public class GeradorPassagemReprovadaTransacaoReprovadaExistenteArtespMessage: PassagemArtespMessageBase
    {        
        public TransacaoRecusada TransacaoRecusada { get; set; }
    }
}