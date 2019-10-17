using System;
using AutoMapper;
using ConectCar.Framework.Infrastructure.Cqrs.Handlers;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.Dto;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.Model;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Bo;
using ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Request;
using ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Responses;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Edi
{
    public class GeradorPassagemAprovadaEdiHandler : DataSourceHandlerBase, IAdoDataSourceProvider
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
        public GeradorPassagemAprovadaEdiHandler()
        {
            _dataSourceConectSysReadOnly = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSysReadOnly);
            _dataSourceFallBack = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSys);
        } 
        #endregion


        public GeradorPassagemAprovadaEdiResponse Execute(GeradorPassagemAprovadaEdiRequest request)
        {
            var response = new GeradorPassagemAprovadaEdiResponse { PassagemAprovadaEdi = new PassagemAprovadaEDI() };

            Mapper.Map(request.PassagemPendenteEdi, response.PassagemAprovadaEdi);

            if (request.PassagemPendenteEdi.StatusCobranca == StatusCobranca.Provisoria)
            {
                response.PassagemAprovadaEdi.TransacaoProvisoria = new TransacaoProvisoriaEDI
                {
                    AdesaoId = request.PassagemPendenteEdi.Adesao.Id.TryToInt(),
                    CategoriaUtilizadaId = request.PassagemPendenteEdi.CategoriaUtilizada.Id.TryToInt(),
                    DataDePassagem = request.PassagemPendenteEdi.DataPassagem,
                    ItemListaDeParaUtilizado = request.PassagemPendenteEdi.ItemListaDeParaUtilizado,
                    PistaId = request.PassagemPendenteEdi.Pista.Id.TryToInt(),
                    StatusId = (int)request.PassagemPendenteEdi.StatusPassagem,
                    DetalheTrnId = request.PassagemPendenteEdi.DetalheTrnId,
                    TipoOperacaoId = (int)TipoOperacaoMovimentoFinanceiro.Passagem,
                    Data = request.PassagemPendenteEdi.DataPassagem,
                    Valor = request.PassagemPendenteEdi.Valor,
                    DataRepasse = DateTime.Now, // será preenchido no calculo de repasse
                    TarifaDeInterconexao = 0,// será preenchido no calculo de repasse
                    ValorRepasse = 0,// será preenchido no calculo de repasse
                    RepasseId = 0,// será preenchido no calculo de repasse
                };

                if (request.PassagemPendenteEdi.DetalheTrfAprovadoManualmenteId != null && request.PassagemPendenteEdi.DetalheTrfAprovadoManualmenteId > 0)
                {
                    response.PassagemAprovadaEdi.TransacaoProvisoria.DetalheTRFAprovadoManualmente = new DetalheTRFAprovadoManualmenteDto
                    {
                        Id = request.PassagemPendenteEdi.DetalheTrfAprovadoManualmenteId
                    };
                }

                if (request.PassagemPendenteEdi.PossuiDivergenciaCategoriaVeiculo)
                {
                    response.PassagemAprovadaEdi.TransacaoProvisoria.DivergenciaCategoriaConfirmada =
                        new DivergenciaCategoriaConfirmada
                        {
                            CategoriaVeiculo = request.PassagemPendenteEdi.Adesao.Veiculo.Categoria
                        };
                }
            }
            else
            {
                response.PassagemAprovadaEdi.Transacao = new TransacaoPassagemEDI
                {
                    AdesaoId = request.PassagemPendenteEdi.Adesao.Id.TryToInt(),
                    CategoriaUtilizadaId = request.PassagemPendenteEdi.CategoriaUtilizada.Id.TryToInt(),
                    DataDePassagem = request.PassagemPendenteEdi.DataPassagem,
                    ItemListaDeParaUtilizado = request.PassagemPendenteEdi.ItemListaDeParaUtilizado,
                    PistaId = request.PassagemPendenteEdi.Pista.Id.TryToInt(),
                    StatusId = (int)request.PassagemPendenteEdi.StatusPassagem,
                    DetalheTrnId = request.PassagemPendenteEdi.DetalheTrnId,
                    Data = request.PassagemPendenteEdi.DataPassagem,                    
                    TipoOperacaoId = (int)TipoOperacaoMovimentoFinanceiro.Passagem,
                    Valor = request.PassagemPendenteEdi.Valor,
                    DataRepasse = DateTime.Now, // será preenchido no calculo de repasse
                    TarifaDeInterconexao = 0,// será preenchido no calculo de repasse
                    ValorRepasse = 0,// será preenchido no calculo de repasse
                    RepasseId = 0,// será preenchido no calculo de repasse
                };

                if (request.PassagemPendenteEdi.DetalheTrfAprovadoManualmenteId != null && request.PassagemPendenteEdi.DetalheTrfAprovadoManualmenteId > 0)
                {
                    response.PassagemAprovadaEdi.Transacao.DetalheTRFAprovadoManualmente = new DetalheTRFAprovadoManualmenteDto
                    {
                        Id = request.PassagemPendenteEdi.DetalheTrfAprovadoManualmenteId
                    };
                }

                if (request.PassagemPendenteEdi.PossuiDivergenciaCategoriaVeiculo)
                {
                    response.PassagemAprovadaEdi.Transacao.DivergenciaCategoriaConfirmada =
                        new DivergenciaCategoriaConfirmada
                        {
                            CategoriaVeiculo = request.PassagemPendenteEdi.Adesao.Veiculo.Categoria
                        };
                }
            }

            var calcularRepasse =
                new CalcularRepasseEdiBo(response.PassagemAprovadaEdi, _dataSourceConectSysReadOnly, _dataSourceFallBack);
            calcularRepasse.Calcular();

            if (request.PassagemPendenteEdi.PossuiEvasaoAceita)
            {
                response.DetalheTrfRecusado = new DetalheTrfRecusado
                {
                    CodigoRetorno = CodigoRetornoTransacaoTRF.EvasaoAceitaPassagemValida,
                    DetalheTRNId = request.PassagemPendenteEdi.DetalheTrnId
                };
            }

            if (request.PassagemPendenteEdi.PossuiEventoPrimeiraPassagemManual)
            {
                response.Evento = new Evento
                {
                    DataCriacao = DateTime.Now,
                    IdRegistro = request.PassagemPendenteEdi.Adesao.Id ?? 0,
                    TipoEvento = TipoEvento.PrimeiraPassagemManual,
                    Processado = true
                };
            }
            return response;
        }
    }
}
