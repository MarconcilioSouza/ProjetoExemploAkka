using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Exceptions
{
    public class PassagemDivergenciaCategoriaException : PassagemException
    {
        public PassagemPendenteArtesp PassagemPendenteArtesp { get; }

        public PassagemDivergenciaCategoriaException(MotivoNaoCompensado motivoNaoCompensado, PassagemPendenteArtesp passagem) : base(motivoNaoCompensado)
        {
            PassagemPendenteArtesp = passagem;
            MotivoNaoCompensado = motivoNaoCompensado;
        }
    }
}
