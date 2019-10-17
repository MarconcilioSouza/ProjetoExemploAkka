using ConectCar.Framework.Backend.CommonQuery.Query;
using ConectCar.Framework.Domain.Model;
using ConectCar.Framework.Infrastructure.Cqrs.Handlers;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Enums;
using ProcessadorPassagensActors.CommandQuery.Exceptions;
using ProcessadorPassagensActors.CommandQuery.Handlers.Park.Request;
using ProcessadorPassagensActors.CommandQuery.Handlers.Park.Response;
using ProcessadorPassagensActors.CommandQuery.Validators.Validator;
using System;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Park
{
    public class ValidarPassagemParkHandler : DataSourceHandlerBase, IAdoDataSourceProvider
    {
        #region [Properties]

        public DbConnectionDataSourceProvider AdoDataSourceProvider => GetAdoProvider();
        private readonly DbConnectionDataSource _dataSourceConectSysReadOnly;
        private readonly DbConnectionDataSource _dataSourceFallBack;
        private readonly GenericValidator<PassagemPendenteEstacionamento> _validator;
        protected PassagemPendenteEstacionamento PassagemPendenteEstacionamento;

        protected override void Init()
        {
            AddProvider(new DbConnectionDataSourceProvider());
        }

        #endregion [Properties]

        #region [Ctor]

        public ValidarPassagemParkHandler()
        {
            _validator = new GenericValidator<PassagemPendenteEstacionamento>();
            _dataSourceConectSysReadOnly = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSysReadOnly);
            _dataSourceFallBack = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSys);
        }

        #endregion [Ctor]

        public ValidarPassagemParkResponse Execute(ValidarPassagemParkRequest request)
        {
            PassagemPendenteEstacionamento = request.PassagemPendenteEstacionamento;

            Log.Info($"Passagem RegistroTransacaoId: {request.PassagemPendenteEstacionamento.RegistroTransacaoId} - Fluxo: ValidarPassagemParkResponse | ValidarConveniado");
            if (!_validator.Validate(request.PassagemPendenteEstacionamento, PassagemPendenteParkValidatorEnum.ValidarConveniado.ToString()))
                throw new ParkException(request.PassagemPendenteEstacionamento, EstacionamentoErros.ConveniadoNaoEstacionamento);

            Log.Info($"Passagem RegistroTransacaoId: {request.PassagemPendenteEstacionamento.RegistroTransacaoId} - Fluxo: ValidarPassagemParkResponse | ValidarTag");
            if (!_validator.Validate(request.PassagemPendenteEstacionamento, PassagemPendenteParkValidatorEnum.ValidarTag.ToString()))
                throw new ParkException(request.PassagemPendenteEstacionamento, EstacionamentoErros.TagInvalida);

            Log.Info($"Passagem RegistroTransacaoId: {request.PassagemPendenteEstacionamento.RegistroTransacaoId} - Fluxo: ValidarPassagemParkResponse | ValidarPraca");
            if (!_validator.Validate(request.PassagemPendenteEstacionamento, PassagemPendenteParkValidatorEnum.ValidarPraca.ToString()))
                throw new ParkException(request.PassagemPendenteEstacionamento, EstacionamentoErros.PracaInvalida);

            Log.Info($"Passagem RegistroTransacaoId: {request.PassagemPendenteEstacionamento.RegistroTransacaoId} - Fluxo: ValidarPassagemParkResponse | ValidarPista");
            if (!_validator.Validate(request.PassagemPendenteEstacionamento, PassagemPendenteParkValidatorEnum.ValidarPista.ToString()))
                throw new ParkException(request.PassagemPendenteEstacionamento, EstacionamentoErros.PistaInvalida);

            Log.Info($"Passagem RegistroTransacaoId: {request.PassagemPendenteEstacionamento.RegistroTransacaoId} - Fluxo: ValidarPassagemParkResponse | ValidarDataTransacaoUltrapassaLimitePermitido");
            ValidarDataTransacaoUltrapassaLimitePermitido();

            Log.Info($"Passagem RegistroTransacaoId: {request.PassagemPendenteEstacionamento.RegistroTransacaoId} - Fluxo: ValidarPassagemParkResponse | ValorTransacaoUltrapassaLimitePermitido");
            ValorTransacaoUltrapassaLimitePermitido();

            return new ValidarPassagemParkResponse { PassagemPendenteEstacionamento = request.PassagemPendenteEstacionamento };
        }

        private void ValidarDataTransacaoUltrapassaLimitePermitido()
        {
            var obterConfiguracaoSistemaQuery = new ObterConfiguracaoSistemaQuery(true, _dataSourceConectSysReadOnly, _dataSourceFallBack);
            var configuracao =
                obterConfiguracaoSistemaQuery.Execute(ConfiguracaoSistemaModel.TransacaoEstacionamentoLimiteDias);

            if (configuracao != null)
            {
                var quantidadeDias = configuracao.Valor.TryToInt();
                if (PassagemPendenteEstacionamento.DataPassagem > DateTime.Now.AddDays(quantidadeDias) ||
                    PassagemPendenteEstacionamento.DataPassagem < DateTime.Now.AddDays(-quantidadeDias))
                    throw new ParkException(PassagemPendenteEstacionamento, EstacionamentoErros.DataTransacaoUltrapassaLimitePermitido);
            }
        }

        private void ValorTransacaoUltrapassaLimitePermitido()
        {
            var obterConfiguracaoSistemaQuery = new ObterConfiguracaoSistemaQuery(true, _dataSourceConectSysReadOnly, _dataSourceFallBack);
            var configuracao =
                obterConfiguracaoSistemaQuery.Execute(ConfiguracaoSistemaModel.TransacaoEstacionamentoLimiteValor);

            if (configuracao != null)
            {
                var valorLimite = configuracao.Valor.TryToDecimal();
                if (PassagemPendenteEstacionamento.Valor > valorLimite)
                    throw new ParkException(PassagemPendenteEstacionamento, EstacionamentoErros.DataTransacaoUltrapassaLimitePermitido);
            }
        }
    }
}