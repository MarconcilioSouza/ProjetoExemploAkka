using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Exceptions
{
    public class PassagemPendenteReprovadaException : PassagemException
    {
        public PassagemPendenteArtesp PassagemPendenteArtesp { get; set; }

        public PassagemPendenteReprovadaException(PassagemPendenteArtesp passagemPendenteArtesp, MotivoNaoCompensado motivoNaoCompensado) : base(motivoNaoCompensado)
        {
            PassagemPendenteArtesp = passagemPendenteArtesp;
            MotivoNaoCompensado = motivoNaoCompensado;
        }
    }

}
