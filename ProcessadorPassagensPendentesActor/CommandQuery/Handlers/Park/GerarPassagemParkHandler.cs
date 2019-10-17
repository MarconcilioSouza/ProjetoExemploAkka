using ConectCar.Framework.Infrastructure.Cqrs.Handlers;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Handlers.Park.Request;
using ProcessadorPassagensActors.CommandQuery.Handlers.Park.Response;
using ProcessadorPassagensActors.CommandQuery.Queries;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Park
{
    public class GerarPassagemParkHandler : DataSourceHandlerBase,
        IAdoDataSourceProvider
    {
        #region [Properties]
        public DbConnectionDataSourceProvider AdoDataSourceProvider => GetAdoProvider();
        readonly DbConnectionDataSource _dataSourceConectSysReadOnly;
        readonly DbConnectionDataSource _dataSourceFallBack;
        protected override void Init()
        {
            AddProvider(new DbConnectionDataSourceProvider());
        }

        #endregion

        #region  [Ctor]
        public GerarPassagemParkHandler()
        {
            _dataSourceConectSysReadOnly = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSysReadOnly);
            _dataSourceFallBack = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSys);
        }
        #endregion

        public GerarPassagemParkResponse Execute(GerarPassagemParkRequest request)
        {
            var response = new GerarPassagemParkResponse
            {
                PassagemPendenteEstacionamento = CarregarPassagemPendentePark(request.PassagemPendenteEstacionamento)
            };
            return response;
        }

        public PassagemPendenteEstacionamento CarregarPassagemPendentePark(PassagemPendenteEstacionamento passagemPendentePark)
        {
            Log.Info($"Passagem RegistroTransacaoId: {passagemPendentePark.RegistroTransacaoId} - Fluxo: GerarPassagemParkHandler | Criando Passagem Completa.");
            var query = new CriarPassagemCompletaParkQuery(_dataSourceConectSysReadOnly, _dataSourceFallBack);
             query.Execute(passagemPendentePark);

            return passagemPendentePark;
        }
    }
}
