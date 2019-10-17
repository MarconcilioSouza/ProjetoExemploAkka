using ConectCar.Framework.Infrastructure.Cqrs.Handlers;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Request;
using ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Responses;
using ProcessadorPassagensActors.CommandQuery.Validators.Validator;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Edi
{
    public class ValidadorSlaListaNelaEdiHandler : DataSourceHandlerBase, IAdoDataSourceProvider
    {

        #region [Properties]
        public DbConnectionDataSourceProvider AdoDataSourceProvider => GetAdoProvider();
        readonly DbConnectionDataSource _dataSourceConectSysReadOnly;
        readonly DbConnectionDataSource _dataSourceFallBack;
        private IValidator _validator;
        protected override void Init()
        {
            AddProvider(new DbConnectionDataSourceProvider());
        }
        #endregion

        #region [Ctor]
        public ValidadorSlaListaNelaEdiHandler()
        {
            _dataSourceConectSysReadOnly = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSysReadOnly);
            _dataSourceFallBack = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSys);
        }
        #endregion

        public ValidadorSlaListaNelaEdiResponse Execute(ValidadorSlaListaNelaEdiRequest request)
        {
            Log.Info($"Passagem DetalheTrnId: {request.PassagemPendenteEdi.DetalheTrnId} - Fluxo: ValidadorSlaListaNelaEdiHandler | Validar SlaListaNelaEdiValidator");
            _validator = new SlaListaNelaEdiValidator(_dataSourceConectSysReadOnly, _dataSourceFallBack, request.PassagemPendenteEdi);
            _validator.Validate();
            return new ValidadorSlaListaNelaEdiResponse { PassagemPendenteEdi = request.PassagemPendenteEdi };
        }
    }
}
