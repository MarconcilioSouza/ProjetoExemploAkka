using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.CommandQuery.Queries;

namespace ProcessadorPassagensActors.CommandQuery.Validators.Validator
{
    public class TransacaoRepetidaValidator
    {
        private readonly ObterCountTransacaoPassagemRepetida _countTransacaoPassagemRepetida;
        private readonly ObterCountTransacaoPassagemRepetidaOrigemTrn _countTransacaoPassagemRepetidaOrigemTrn;
        private readonly ObterCountTransacaoPassagemRepetidaPorConveniado _countTransacaoPassagemRepetidaPorConveniado;

        public TransacaoRepetidaValidator()
        {
            _countTransacaoPassagemRepetida = new ObterCountTransacaoPassagemRepetida();
            _countTransacaoPassagemRepetidaOrigemTrn = new ObterCountTransacaoPassagemRepetidaOrigemTrn();
            _countTransacaoPassagemRepetidaPorConveniado = new ObterCountTransacaoPassagemRepetidaPorConveniado();
        }

        public MotivoNaoCompensado Validate(PassagemPendenteArtesp passagemPendenteArtesp)
        {
            var motivoNaoCompensado = MotivoNaoCompensado.SemMotivoNaoCompensado;

            var retorno = DataBaseConnection.HandleExecution(_countTransacaoPassagemRepetida.Execute, passagemPendenteArtesp);
            if (retorno)
            {
                retorno = DataBaseConnection.HandleExecution(_countTransacaoPassagemRepetidaOrigemTrn.Execute, passagemPendenteArtesp);
                if (retorno)
                    return MotivoNaoCompensado.TransacaoRepetida;

                retorno = DataBaseConnection.HandleExecution(_countTransacaoPassagemRepetidaPorConveniado.Execute, passagemPendenteArtesp);
                if (!retorno)
                    return MotivoNaoCompensado.TransacaoRepetida;
            }

            return motivoNaoCompensado;
        }
    }
}
