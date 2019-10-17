using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Queries;
using ProcessadorPassagensActors.CommandQuery.Queries.Filter;
using ProcessadorPassagensActors.Infrastructure;
using ConectCar.Framework.Infrastructure.Cqrs.Ado.Queries;
using ProcessadorPassagensActors.CommandQuery.Enums;
using ProcessadorPassagensActors.CommandQuery.Exceptions;

namespace ProcessadorPassagensActors.CommandQuery.Validators.Validator
{
    public class ValidarTransacaoRepetidaParkQuery : IValidator
    {
        private readonly PassagemPendenteEstacionamento _passagemPendenteEstacionamento;

        public ValidarTransacaoRepetidaParkQuery(PassagemPendenteEstacionamento passagemPendenteEstacionamento)
        {
            _passagemPendenteEstacionamento = passagemPendenteEstacionamento;
        }

        public void Validate()
        {
            var filter = new ObterTransacaoEstacionamentoIdRepetidaFilter
            {
                TagId = _passagemPendenteEstacionamento.Tag.Id.TryToInt(),
                ConveniadoId = _passagemPendenteEstacionamento.Conveniado.Id.TryToInt(),
                PistaId = _passagemPendenteEstacionamento.Pista.Id.TryToInt(),
                PracaId = _passagemPendenteEstacionamento.Praca.Id.TryToInt(),
                DataHoraEntrada = _passagemPendenteEstacionamento.DataHoraEntrada,
                DataHoraTransacao = _passagemPendenteEstacionamento.DataPassagem,
                TempoPermanencia = _passagemPendenteEstacionamento.TempoPermanencia
            };

            var obterTransacaoIdRepetidaQuery = new ObterTransacaoEstacionamentoIdRepetidaQuery();
            var idTransacao = obterTransacaoIdRepetidaQuery.Execute(filter);

            if (idTransacao > 0)
            {
                throw new ParkException(_passagemPendenteEstacionamento, EstacionamentoErros.TransacaoRepetida);
            }

            filter.TempoPermanencia = 0;
            idTransacao = obterTransacaoIdRepetidaQuery.Execute(filter);
            if (idTransacao > 0)
            {
                throw new ParkException(_passagemPendenteEstacionamento, EstacionamentoErros.TransacaoRepetida);
            }
        }
    }
}
