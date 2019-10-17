using AutoMapper;
using ConectCar.Framework.Backend.CommonQuery.Query;
using ConectCar.Framework.Domain.Model;
using ConectCar.Framework.Infrastructure.Cqrs.Handlers;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.Model;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Request;
using ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Responses;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Edi
{
    public class GeradorPassagemReprovadaEdiHandler : DataSourceHandlerBase, IAdoDataSourceProvider
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

        #region [Ctor]
        public GeradorPassagemReprovadaEdiHandler()
        {
            _dataSourceConectSysReadOnly =
                AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSysReadOnly);
            _dataSourceFallBack = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSys);
        }
        #endregion

        public GeradorPassagemReprovadaEdiResponse Execute(GeradorPassagemReprovadaPorTransacaoExceptionEdiRequest request)
        {
            var response = new GeradorPassagemReprovadaEdiResponse { PassagemReprovadaEdi = new PassagemReprovadaEDI() };
            Mapper.Map(request.PassagemPendenteEdi, response.PassagemReprovadaEdi);

            response.PassagemReprovadaEdi.CodigoRetorno = request.CodigoRetornoTransacaoTrf;
            response.PassagemReprovadaEdi.DetalheTRFRecusado = new DetalheTrfRecusado
            {
                CodigoRetorno = request.CodigoRetornoTransacaoTrf,
                DetalheTRNId = request.PassagemPendenteEdi.DetalheTrnId
            };
            if (request.PassagemPendenteEdi.Adesao.Veiculo.Id != null && request.PassagemPendenteEdi.Adesao.Veiculo.Id > 0)
            {
                response.PassagemReprovadaEdi.Veiculo = request.PassagemPendenteEdi.Adesao.Veiculo;
            }
            return response;
        }

        public GeradorPassagemReprovadaEdiResponse Execute(GeradorPassagemReprovadaPorTransacaoParceiroExceptionEdiRequest request)
        {
            var response = new GeradorPassagemReprovadaEdiResponse { PassagemReprovadaEdi = new PassagemReprovadaEDI() };
            Mapper.Map(request.PassagemPendenteEdi, response.PassagemReprovadaEdi);

            response.PassagemReprovadaEdi.CodigoRetorno = request.CodigoRetornoTransacaoTrf;

            var queryConfigSistema =
                new ObterConfiguracaoSistemaQuery(true, _dataSourceConectSysReadOnly, _dataSourceFallBack);
            var parceiroId = queryConfigSistema.Execute(ConfiguracaoSistemaModel.CodigoParceiroRoadCard);

            response.PassagemReprovadaEdi.TransacaoRecusadaParceiro = new TransacaoRecusadaParceiroEdi
            {
                DetalheTRNId = request.PassagemPendenteEdi.DetalheTrnId,
                CodigoRetornoTransacaoTRF = request.CodigoRetornoTransacaoTrf,
                DataPassagemNaPraca = request.PassagemPendenteEdi.DataPassagem,
                ParceiroId = parceiroId.Valor.TryToInt(),
                Valor = request.PassagemPendenteEdi.Valor,
                ViagemAgendada = new DetalheViagem
                {
                    Id = request.DetalheViagemId
                },
                DataEnvioAoParceiro = null
            };

            response.PassagemReprovadaEdi.Veiculo = request.PassagemPendenteEdi.Adesao.Veiculo;

            return response;
        }
    }
}