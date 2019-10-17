using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.ValueObject;

namespace ProcessadorPassagensActors.CommandQuery.Exceptions
{
    public class PassagemReprovadaException : PassagemException
    {
        public PassagemPendenteArtesp PassagemPendenteArtesp { get; set; }

        public PassagemReprovadaException(MotivoNaoCompensado motivo, PassagemPendenteArtesp passagemPendenteArtesp) : base(motivo)
        {
            PassagemPendenteArtesp = passagemPendenteArtesp;
            MotivoNaoCompensado = motivo;
        }
    }
}
