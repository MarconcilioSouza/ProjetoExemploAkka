using ConectCar.Framework.Infrastructure.Cqrs.Ado.Queries;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.Model;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Exceptions;
using ProcessadorPassagensActors.CommandQuery.Queries;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Validators.Validator
{
    public class TransacaoRepetidaEdiValidator : IValidator
    {
        private readonly PassagemPendenteEDI _passagem;

        public TransacaoRepetidaEdiValidator(PassagemPendenteEDI passagem)
        {
            this._passagem = passagem;
        }

        public void Validate()
        {
            var query = new ObterCountTransacaoPassagemRepetidaOrigemTrn();
            var retorno = query.Execute(_passagem);
            if (retorno)
            {
                throw new EdiTransacaoException(CodigoRetornoTransacaoTRF.TransacaoRepetida, _passagem);
            }
        }
    }
}
