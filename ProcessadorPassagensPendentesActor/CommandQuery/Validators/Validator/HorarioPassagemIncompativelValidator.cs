using ConectCar.Framework.Backend.CommonQuery.Query;
using ConectCar.Framework.Domain.Model;
using ConectCar.Framework.Infrastructure.Cqrs.Ado.Queries;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.Model;
using ProcessadorPassagensActors.CommandQuery.Exceptions;
using ProcessadorPassagensActors.CommandQuery.Queries;
using ProcessadorPassagensActors.CommandQuery.Queries.Filter;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Validators.Validator
{
    public class HorarioPassagemIncompativelValidator : IValidator
    {
        readonly DbConnectionDataSource _dataSourceConectSysReadOnly;
        readonly DbConnectionDataSource _dataSourceFallBack;
        readonly PassagemPendenteEDI _passagemPendenteEdi;

        public HorarioPassagemIncompativelValidator(DbConnectionDataSource dbSysReadOnly, DbConnectionDataSource dbSysFallBack, PassagemPendenteEDI passagemPendenteEdi)
        {
            _dataSourceConectSysReadOnly = dbSysReadOnly;
            _dataSourceFallBack = dbSysFallBack;
            _passagemPendenteEdi = passagemPendenteEdi;
        }

        public void Validate()
        {
            if (HorarioDePassagemEIncompativel())
            {
                throw new EdiTransacaoException(CodigoRetornoTransacaoTRF.HorarioPassagemIncompativel, _passagemPendenteEdi);
            }
        }

        private bool HorarioDePassagemEIncompativel()
        {
            var queryConfiguracaoSistema = new ObterConfiguracaoSistemaQuery(true, _dataSourceConectSysReadOnly, _dataSourceFallBack);
            var configuracaoHorarioPassagem = queryConfiguracaoSistema.Execute(ConfiguracaoSistemaModel.HorarioDePassagem).Valor.TryToInt();

            var dataReferencia = _passagemPendenteEdi.DataPassagem.AddSeconds(-_passagemPendenteEdi.Praca.TempoRetornoPraca).AddMinutes(-(configuracaoHorarioPassagem + 5));

            var obterIdUltimaTransacaoPassagem = new ObterIdUltimaTransacaoPassagemQuery();
            var idUltimaTransacaoPassagem = obterIdUltimaTransacaoPassagem.Execute(new DetalheTrnFilter
            {
                OBUId = _passagemPendenteEdi.Tag.OBUId,
                CodigoPraca = _passagemPendenteEdi.Praca.CodigoPraca,
                Data = dataReferencia
            });

            if ((idUltimaTransacaoPassagem ?? 0) > 0)
            {
                var obterDetalheTrnQuery = new ObterDetalheTrnPorTransacaoIdQuery();
                var detalheTrn = obterDetalheTrnQuery.Execute(idUltimaTransacaoPassagem ?? 0);

                var diferencaMinutosAceito = _passagemPendenteEdi.DataPassagem.Subtract(detalheTrn.Data).TotalMinutes;

                if (diferencaMinutosAceito < 0)
                    diferencaMinutosAceito = diferencaMinutosAceito * -1;

                if (diferencaMinutosAceito < _passagemPendenteEdi.Praca.TempoRetornoPraca)
                {
                    if (detalheTrn.StatusCobrancaId == (int)StatusCobranca.Provisoria && _passagemPendenteEdi.StatusCobranca == StatusCobranca.Confirmacao)
                        return false;

                    return true;
                }
            }

            var verificarSeTempoDePassagemEhInValido = new TempoDePassagemEhInValidoQuery(_dataSourceConectSysReadOnly);
            var tempoDePassagemEhInValido = verificarSeTempoDePassagemEhInValido.Execute(_passagemPendenteEdi);


            if (_passagemPendenteEdi.StatusCobranca != StatusCobranca.Confirmacao)
                return tempoDePassagemEhInValido;

            return false;
        }


    }
}

