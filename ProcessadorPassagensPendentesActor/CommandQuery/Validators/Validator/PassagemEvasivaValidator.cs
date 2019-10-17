using ConectCar.Framework.Backend.CommonQuery.Query;
using ConectCar.Framework.Domain.Model;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.Model;
using ProcessadorPassagensActors.CommandQuery.Exceptions;
using ProcessadorPassagensActors.CommandQuery.Queries;

namespace ProcessadorPassagensActors.CommandQuery.Validators.Validator
{
    public class PassagemEvasivaValidator : IValidator
    {
        private readonly PassagemPendenteEDI _passagemPendenteEdi;

        public PassagemEvasivaValidator(PassagemPendenteEDI passagemPendenteEdi)
        {
            _passagemPendenteEdi = passagemPendenteEdi;
        }

        public void Validate()
        {
            if (_passagemPendenteEdi.StatusPassagem.Equals(StatusPassagem.Evasao))
            {
                var obterCountHistoricoListaNela = new ObterCountHistoricoListaNelaQuery();
                var countHistoricoListaNela = obterCountHistoricoListaNela.Execute(_passagemPendenteEdi);

                var validarSaldoSuficiente = new PossuiSaldoSuficienteValidator(_passagemPendenteEdi);
                if (countHistoricoListaNela && !validarSaldoSuficiente.Validate())
                    throw new EdiTransacaoException(CodigoRetornoTransacaoTRF.EvasaoParaAutuacao, _passagemPendenteEdi);

                _passagemPendenteEdi.PossuiEvasaoAceita = true;
            }
        }

    }
}
