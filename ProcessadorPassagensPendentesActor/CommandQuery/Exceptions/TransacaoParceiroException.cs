using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Exceptions
{
    public class TransacaoParceiroException : DomainException
    {
        public MotivoNaoCompensado MotivoNaoCompensado { get; protected set; }
        public int DetalheViagemId { get; protected set; }

        public PassagemPendenteArtesp PassagemPendente { get; set; }

        public TransacaoParceiroException(MotivoNaoCompensado motivoNaoCompensado, int detalheViagemId, PassagemPendenteArtesp passagemPendente) : base(motivoNaoCompensado.ToString())
        {
            MotivoNaoCompensado = motivoNaoCompensado;
            DetalheViagemId = detalheViagemId;
            PassagemPendente = passagemPendente;
        }
    }
}
