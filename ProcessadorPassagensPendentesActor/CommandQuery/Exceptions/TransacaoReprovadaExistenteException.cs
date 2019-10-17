using ConectCar.Transacoes.Domain.Model;
using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Exceptions
{
    public class TransacaoReprovadaExistenteException : PassagemException
    {
        public PassagemPendenteArtesp PassagemPendenteArtesp { get; protected set; }
        public TransacaoRecusada TransacaoRecusada { get; set; }

        public TransacaoReprovadaExistenteException(PassagemPendenteArtesp passagemPendenteArtesp, TransacaoRecusada transacaoRecusada) : base(transacaoRecusada.MotivoRecusado)
        {
            PassagemPendenteArtesp = passagemPendenteArtesp;
            TransacaoRecusada = transacaoRecusada;
        }
    }
}
