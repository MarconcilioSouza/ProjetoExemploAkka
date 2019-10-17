using ConectCar.Transacoes.Domain.Enum;
using ProcessadorPassagensActors.CommandQuery.Enums;
using ProcessadorPassagensActors.CommandQuery.Queries;
using ProcessadorPassagensActors.CommandQuery.Queries.Filter;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Cache;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Validators.Validator
{
    public class HorarioPassagemManualValidator
    {
        private readonly ObterCountTransacaoPassagemManualHorarioIncompativelNaMesmaPracaQuery _countTransacaoPassagemManualHorarioIncompativelNaMesmaPracaQuery;
        private readonly ObterCountTransacaoPassagemPorHorarioDePassagemManualQuery _countTransacaoPassagemPorHorarioDePassagemManualQuery;
        private readonly int _quantidadeDiasLimiteEstornoPassagem;

        public HorarioPassagemManualValidator()
        {
            _countTransacaoPassagemManualHorarioIncompativelNaMesmaPracaQuery = new ObterCountTransacaoPassagemManualHorarioIncompativelNaMesmaPracaQuery();
            _countTransacaoPassagemPorHorarioDePassagemManualQuery = new ObterCountTransacaoPassagemPorHorarioDePassagemManualQuery();
            var configuracao = ConfiguracaoSistemaCacheRepository.Obter(NomeConfiguracaoSistema.QuantidadeDiasLimiteEstornoPassagem.ToString());
            _quantidadeDiasLimiteEstornoPassagem = configuracao.Valor.TryToInt();
        }

        public MotivoNaoCompensado Validate(PassagemPendenteArtesp passagemPendenteArtesp)
        {
            var motivoNaoCompensado = MotivoNaoCompensado.SemMotivoNaoCompensado;
            
            var filter = new ObterCountTransacaoPassagemPorHorarioDePassagemManualFilter { PassagemPendenteArtesp = passagemPendenteArtesp };
            filter.tempoLimite = _quantidadeDiasLimiteEstornoPassagem;

            var retorno = DataBaseConnection.HandleExecution(_countTransacaoPassagemManualHorarioIncompativelNaMesmaPracaQuery.Execute,filter);
            if (retorno) 
                return MotivoNaoCompensado.PassagemManualHorarioIncompativelNaMesmaPraca;


            retorno = DataBaseConnection.HandleExecution(_countTransacaoPassagemPorHorarioDePassagemManualQuery.Execute,filter);
            if (retorno)
                return MotivoNaoCompensado.PassagemManualHorarioIncompativelPracaDiferente;

            return motivoNaoCompensado;

        }
    }
}
