using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.Model;
using ProcessadorPassagensActors.CommandQuery.Queries;
using ProcessadorPassagensActors.Infrastructure;
using ConectCar.Framework.Infrastructure.Cqrs.Ado.Queries;
using ConectCar.Transacoes.Domain.Enum;
using ProcessadorPassagensActors.CommandQuery.Exceptions;

namespace ProcessadorPassagensActors.CommandQuery.Validators.Validator
{
    public class TransacaoConfirmacaoValidator : IValidator
    {
        private bool PossuiTransacaoProvisoria { get; }
        private readonly PassagemPendenteEDI _passagemPendenteEdi;
        public TransacaoConfirmacaoValidator(PassagemPendenteEDI passagemPendenteEdi)
        {
            _passagemPendenteEdi = passagemPendenteEdi;
            var query = new ObterCountPossuiTransacaoProvisoriaQuery();
            PossuiTransacaoProvisoria = query.Execute(passagemPendenteEdi);
        }


        public void Validate()
        {
            if (_passagemPendenteEdi.StatusCobranca != StatusCobranca.Confirmacao) return;
            if (!PossuiTransacaoProvisoria)
            {
                throw new EdiTransacaoException(CodigoRetornoTransacaoTRF.CATCobradaNaoCompativel, _passagemPendenteEdi);
            }
            ValidarTransacaoProvisoriaDentroTempoCorrecao();
        }


        private void ValidarTransacaoProvisoriaDentroTempoCorrecao()
        {

            var intervaloPassagem = DateTime.Now.Subtract(_passagemPendenteEdi.DataPassagem).TotalHours;

            if (intervaloPassagem > _passagemPendenteEdi.Conveniado.TempoDeCorrecaoDasTransacoesProvisorias)
            {
                throw new EdiTransacaoException(CodigoRetornoTransacaoTRF.PassagemForaDoPeriodo, _passagemPendenteEdi);
            }
        }

    }
}
