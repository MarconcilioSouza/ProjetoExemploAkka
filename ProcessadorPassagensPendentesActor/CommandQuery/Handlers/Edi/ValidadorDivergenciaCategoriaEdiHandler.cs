using ConectCar.Framework.Infrastructure.Cqrs.Handlers;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Request;
using ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Responses;
using ProcessadorPassagensActors.CommandQuery.Validators.Validator;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Edi
{
    public class ValidadorDivergenciaCategoriaEdiHandler : DataSourceHandlerBase, IAdoDataSourceProvider
    {
        #region [Properties]
        public DbConnectionDataSourceProvider AdoDataSourceProvider => GetAdoProvider();
        private readonly DbConnectionDataSource _dataSourceConectSysReadOnly;
        private readonly DbConnectionDataSource _dataSourceFallBack;
        private IValidator _validator;
        protected override void Init()
        {
            AddProvider(new DbConnectionDataSourceProvider());
        }
        #endregion

        #region [Ctor]
        public ValidadorDivergenciaCategoriaEdiHandler()
        {
            _dataSourceConectSysReadOnly = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSysReadOnly);
            _dataSourceFallBack = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSys);
        } 
        #endregion

        public ValidadorDivergenciaCategoriaEdiResponse Execute(ValidadorDivergenciaCategoriaEdiRequest request)
        {
            #region CategoriaVeiculoValidator
            Log.Info($"Passagem ID: {request.PassagemPendenteEdi.DetalheTrnId} - Fluxo: CategoriaVeiculoValidator | CategoriaVeiculo");
            if (request.PassagemPendenteEdi.Conveniado.HabilitarConfirmacaoCategoria)
            {
                _validator = new CategoriaVeiculoValidator(request.PassagemPendenteEdi);
                _validator.Validate();
            }

            #endregion
            return new ValidadorDivergenciaCategoriaEdiResponse { PassagemPendenteEdi = request.PassagemPendenteEdi };
        }
    }
}
