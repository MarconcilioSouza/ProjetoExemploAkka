using ConectCar.Framework.Infrastructure.Cqrs.Handlers;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ProcessadorPassagensActors.CommandQuery.Handlers.Park.Request;
using ProcessadorPassagensActors.CommandQuery.Handlers.Park.Response;
using ProcessadorPassagensActors.CommandQuery.Validators.Validator;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Park
{
    public class ValidarPassagemSistemaParkHandler : DataSourceHandlerBase,
         IAdoDataSourceProvider
    {
        #region [Properties]

        public DbConnectionDataSourceProvider AdoDataSourceProvider => GetAdoProvider();
        private readonly DbConnectionDataSource _dataSourceConectSysReadOnly;
        private readonly DbConnectionDataSource _dataSourceFallBack;
        private IValidator qryValidarTransacaoRepetida;

        protected override void Init()
        {
            AddProvider(new DbConnectionDataSourceProvider());
        }

        #endregion [Properties]

        #region [Ctor]

        public ValidarPassagemSistemaParkHandler()
        {
            _dataSourceConectSysReadOnly = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSysReadOnly);
            _dataSourceFallBack = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSys);
        }

        #endregion [Ctor]

        public ValidarPassagemSistemaParkResponse Execute(ValidarPassagemSistemaParkRequest request)
        {
            Log.Info($"Passagem RegistroTransacaoId: {request.PassagemPendenteEstacionamento.RegistroTransacaoId} - Fluxo: ValidarPassagemSistemaParkHandler | ValidarTransacaoRepetida");
            qryValidarTransacaoRepetida = new ValidarTransacaoRepetidaParkQuery(request.PassagemPendenteEstacionamento);
            qryValidarTransacaoRepetida.Validate();

            return new ValidarPassagemSistemaParkResponse { PassagemPendenteEstacionamento = request.PassagemPendenteEstacionamento };
        }
    }
}