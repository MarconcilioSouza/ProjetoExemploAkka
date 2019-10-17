using ConectCar.Transacoes.Domain.Model;
using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests
{
  public  class GeradorPassagemPendenteReprovadaTransacaoReprovadaExistenteRequest
    {
        public PassagemPendenteArtesp PassagemPendenteArtesp { get; set; }
        public TransacaoRecusada TransacaoRecusada { get; set; }
    }
}
