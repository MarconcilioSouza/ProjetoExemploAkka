using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.CommandQuery.Queries;

namespace ProcessadorPassagensActors.CommandQuery.Validators.Validator
{
    public class ValorTarifaValidator
    {
        private readonly ObterTarifaPorPracaECategoria _tarifaPorPracaECategoria;

        public ValorTarifaValidator()
        {
            _tarifaPorPracaECategoria = new ObterTarifaPorPracaECategoria();
        }

        public MotivoNaoCompensado Validate(PassagemPendenteArtesp passagemPendenteArtesp)
        {
            var motivoNaoCompensado = MotivoNaoCompensado.SemMotivoNaoCompensado;

            if (passagemPendenteArtesp.MotivoSemValor == MotivoSemValor.NaoSeAplica && passagemPendenteArtesp.Valor == 0)
                return MotivoNaoCompensado.ValorInvalido;

            if (passagemPendenteArtesp.Conveniado.HabilitarValidacaoTarifa)
            {
                decimal? valorTarifa = DataBaseConnection.HandleExecution(_tarifaPorPracaECategoria.Execute, passagemPendenteArtesp);

                if (passagemPendenteArtesp.Tag.Grupo == Grupo.Isento || passagemPendenteArtesp.Tag.Grupo == Grupo.IsentoPelaArtesp)
                    return motivoNaoCompensado;

                if (valorTarifa != passagemPendenteArtesp.Valor)
                    return MotivoNaoCompensado.ValorInvalido;
            }

            return motivoNaoCompensado;
        }
    }
}
