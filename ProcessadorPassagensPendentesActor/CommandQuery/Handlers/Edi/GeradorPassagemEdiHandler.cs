using ConectCar.Framework.Infrastructure.Cqrs.Handlers;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.Model;
using ProcessadorPassagensActors.CommandQuery.Bo;
using ProcessadorPassagensActors.CommandQuery.Enums;
using ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Request;
using ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Responses;
using ProcessadorPassagensActors.CommandQuery.Queries;
using ProcessadorPassagensActors.CommandQuery.Validators.Validator;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Edi
{
    public class GeradorPassagemEdiHandler : DataSourceHandlerBase, IAdoDataSourceProvider
    {
        #region [Properties]
        public DbConnectionDataSourceProvider AdoDataSourceProvider => GetAdoProvider();
        readonly DbConnectionDataSource _dataSourceConectSysReadOnly;
        readonly DbConnectionDataSource _dataSourceFallBack;
        readonly GenericValidator<PassagemPendenteEDI> _validator;
        #endregion

        #region [Ctor]
        public GeradorPassagemEdiHandler()
        {
            _validator = new GenericValidator<PassagemPendenteEDI>();
            _dataSourceConectSysReadOnly = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSysReadOnly);
            _dataSourceFallBack = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSys);
        }
        #endregion
        protected override void Init()
        {
            AddProvider(new DbConnectionDataSourceProvider());
        }
        /// <summary>
        /// Executa o processamento de Passagens
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GeradorPassagemEdiResponse Execute(GeradorPassagemEdiRequest request)
        {
            var response = new GeradorPassagemEdiResponse { PassagemPendenteEdi = CarregarPassagemPendenteEdi(request.PassagemPendenteEdi) };
            return response;
        }

        public PassagemPendenteEDI CarregarPassagemPendenteEdi(PassagemPendenteEDI passagemPendenteEdi)
        {
            Log.Info($"Passagem DetalheTrnId: {passagemPendenteEdi.DetalheTrnId} Data Passagem {passagemPendenteEdi.DataPassagem.ToStringPtBr()}");

            // criar passagem baseado no passagem pendente
            Log.Debug($"Passagem DetalheTrnId: {passagemPendenteEdi.DetalheTrnId} - Fluxo: GeradorPassagemEdiHandler | Criando Passagem Completa.");
            var query = new CriarPassagemCompletaEdiQuery(_dataSourceConectSysReadOnly, _dataSourceFallBack);
            query.Execute(passagemPendenteEdi);


            Log.Debug($"Passagem DetalheTrnId: {passagemPendenteEdi.DetalheTrnId} - Fluxo: GeradorPassagemEdiHandler | DefinirCategoriaUtilizada");
            var defnirCategoriaUtilizadaBo = new DefinirCategoriaUtilizadaEdiBo();

            var request = new CategoriaUtilizadasRequest
            {
                Codigo = passagemPendenteEdi.CategoriaCobrada?.Codigo ?? passagemPendenteEdi.CategoriaTag.Codigo,
                ListaDeParaCategoriaVeiculoId = passagemPendenteEdi.Conveniado.ListaDeParaCategoriaVeiculoId,
            };
            var categoriaUtilizada = defnirCategoriaUtilizadaBo.Definir(request);
            passagemPendenteEdi.CategoriaUtilizada = categoriaUtilizada.CategoriaUtilizada;
            passagemPendenteEdi.ItemListaDeParaUtilizado = categoriaUtilizada.ItemListaDeParaUtilizado;


            return passagemPendenteEdi;
        }
    }
}
