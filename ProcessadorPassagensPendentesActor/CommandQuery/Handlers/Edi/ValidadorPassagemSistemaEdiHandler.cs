using ConectCar.Framework.Infrastructure.Cqrs.Handlers;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.Model;
using ProcessadorPassagensActors.CommandQuery.Enums;
using ProcessadorPassagensActors.CommandQuery.Exceptions;
using ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Request;
using ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Responses;
using ProcessadorPassagensActors.CommandQuery.Validators.Validator;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Edi
{
    public class ValidadorPassagemSistemaEdiHandler : DataSourceHandlerBase, IAdoDataSourceProvider
    {

        #region [Properties]
        public DbConnectionDataSourceProvider AdoDataSourceProvider => GetAdoProvider();
        readonly DbConnectionDataSource _dataSourceConectSysReadOnly;
        readonly DbConnectionDataSource _dataSourceFallBack;
        readonly GenericValidator<PassagemPendenteEDI> _validatorRuleSet;
        private IValidator _validator;
        protected override void Init()
        {
            AddProvider(new DbConnectionDataSourceProvider());
        }

        #endregion

        #region [Ctor]
        public ValidadorPassagemSistemaEdiHandler()
        {
            _dataSourceConectSysReadOnly = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSysReadOnly);
            _dataSourceFallBack = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSys);
            _validatorRuleSet = new GenericValidator<PassagemPendenteEDI>();

        }
        #endregion


        public ValidadorPassagemSistemaEdiActorResponse Execute(ValidadorPassagemSistemaEdiActorRequest request)
        {

            #region Validar transacao repetida
            _validator = new TransacaoRepetidaEdiValidator(request.PassagemPendenteEdi);
            _validator.Validate(); 
            #endregion


            var possuiTransacaoAprovadaManualmente = _validatorRuleSet.Validate(request.PassagemPendenteEdi, PassagemPendenteEdiValidatorEnum.PossuiTransacaoAprovadaManualmente.ToString());

            if (!possuiTransacaoAprovadaManualmente)
            {
                #region HorarioDePassagemEIncompativel
                Log.Info($"Passagem ID: {request.PassagemPendenteEdi.DetalheTrnId} - Fluxo: ValidadorPassagemSistemaEdiHandler | HorarioDePassagemEIncompativel");
                _validator = new HorarioPassagemIncompativelValidator(_dataSourceConectSysReadOnly, _dataSourceFallBack, request.PassagemPendenteEdi);
                _validator.Validate();
                #endregion

                #region PassagemEvasiva
                Log.Info($"Passagem ID: {request.PassagemPendenteEdi.DetalheTrnId} - Fluxo: ValidadorPassagemSistemaEdiHandler | PassagemEvasiva");
                _validator = new PassagemEvasivaValidator(request.PassagemPendenteEdi);
                _validator.Validate();
                #endregion
            }

            #region ValidarTransacaoConfirmacao  ( CATCobradaNaoCompativel - PassagemForaDoPeriodo )
            Log.Info($"Passagem DetalheTrnId: {request.PassagemPendenteEdi.DetalheTrnId} - Fluxo: ValidadorPassagemSistemaEdiHandler | TransacaoConfirmacaoValidator");
            _validator = new TransacaoConfirmacaoValidator(request.PassagemPendenteEdi);
            _validator.Validate();
            #endregion

            #region PrimeiraPassagemManualValidator
            Log.Info($"Passagem DetalheTrnId: {request.PassagemPendenteEdi.DetalheTrnId} - Fluxo: ValidadorPassagemSistemaEdiHandler | PrimeiraPassagemManualValidator");
            var primeiraPassagemManualValidator = new PrimeiraPassagemManualValidator();
            request.PassagemPendenteEdi.PossuiEventoPrimeiraPassagemManual = primeiraPassagemManualValidator.EhPrimeiraPassagemManual(request.PassagemPendenteEdi.Adesao.Id.TryToInt(), request.PassagemPendenteEdi.StatusPassagem);
            #endregion

            return new ValidadorPassagemSistemaEdiActorResponse { PassagemPendenteEdi = request.PassagemPendenteEdi };

        }
    }
}
