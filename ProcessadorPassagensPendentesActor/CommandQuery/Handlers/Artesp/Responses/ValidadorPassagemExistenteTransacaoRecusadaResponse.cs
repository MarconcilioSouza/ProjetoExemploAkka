using ConectCar.Transacoes.Domain.Model;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Responses
{
    public class ValidadorPassagemExistenteTransacaoRecusadaResponse: ValidadorPassagemExistenteResponse
    {
        public TransacaoRecusada TransacaoRecusada { get; set; }
    }
}
