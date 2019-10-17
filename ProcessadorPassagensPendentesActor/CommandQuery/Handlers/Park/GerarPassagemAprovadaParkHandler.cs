using System.Linq;
using AutoMapper;
using ConectCar.Framework.Infrastructure.Cqrs.Handlers;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.Model;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Handlers.Park.Request;
using ProcessadorPassagensActors.CommandQuery.Handlers.Park.Response;
using ProcessadorPassagensActors.CommandQuery.Queries;
using System;
using ProcessadorPassagensActors.CommandQuery.Bo;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Park
{
    public class GerarPassagemAprovadaParkHandler : DataSourceHandlerBase,
        IAdoDataSourceProvider
    {
        #region [Properties]

        public DbConnectionDataSourceProvider AdoDataSourceProvider => GetAdoProvider();
        private readonly DbConnectionDataSource _dataSourceConectSysReadOnly;
        private readonly DbConnectionDataSource _dataSourceFallBack;

        protected override void Init()
        {
            AddProvider(new DbConnectionDataSourceProvider());
        }

        #endregion [Properties]

        #region [Ctor]

        public GerarPassagemAprovadaParkHandler()
        {
            _dataSourceConectSysReadOnly = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSysReadOnly);
            _dataSourceFallBack = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSys);
        }

        #endregion [Ctor]

        public GerarPassagemAprovadaParkResponse Execute(GerarPassagemAprovadaParkRequest request)
        {
            var response = new GerarPassagemAprovadaParkResponse
            {
                PassagemAprovadaEstacionamento = new PassagemAprovadaEstacionamento()
            };

            Mapper.Map(request.PassagemPendenteEstacionamento, response.PassagemAprovadaEstacionamento);
            response.PassagemAprovadaEstacionamento.TransacaoEstacionamento = new TransacaoEstacionamento
            {
                RegistroTransacaoId = request.PassagemPendenteEstacionamento.RegistroTransacaoId,
                AdesaoId = request.PassagemPendenteEstacionamento.Adesao.Id.TryToInt(),
                ConveniadoId = request.PassagemPendenteEstacionamento.Conveniado.Id.TryToInt(),
                Data = DateTime.Now,
                StatusId = (int)StatusAutorizacao.Ativa,
                DataHoraTransacao = request.PassagemPendenteEstacionamento.DataPassagem,
                DataHoraEntrada = request.PassagemPendenteEstacionamento.DataHoraEntrada,
                TipoTransacaoEstacionamentoId = (int)request.PassagemPendenteEstacionamento.TipoTransacaoEstacionamento,
                Mensalista = request.PassagemPendenteEstacionamento.Mensalista,
                PistaId = request.PassagemPendenteEstacionamento.Pista.Id.TryToInt(),
                PracaId = request.PassagemPendenteEstacionamento.Praca.Id.TryToInt(),
                MotivoAtrasoTransmissaoId = (int)request.PassagemPendenteEstacionamento.MotivoAtrasoTransmissao,
                MotivoDesconto = request.PassagemPendenteEstacionamento.MotivoDesconto,
                Credito = false,
                TipoOperacaoId = (int)TipoOperacaoMovimentoFinanceiro.Estacionamento,
                TagId = request.PassagemPendenteEstacionamento.Tag.Id.TryToInt(),
                Ticket = request.PassagemPendenteEstacionamento.Ticket,
                ValorDesconto = request.PassagemPendenteEstacionamento.ValorDesconto,
                Valor = request.PassagemPendenteEstacionamento.Valor,
                TempoPermanencia = request.PassagemPendenteEstacionamento.TempoPermanencia,
                DataRepasse = DateTime.Now, // Será preenchido no calculo de repasse
                RepasseId = 0, // Será preenchido no calculo de repasse
                ValorRepasse = 0, // Será preenchido no calculo de repasse
                DataReferencia = DateTime.Now, // Será preenchido no calculo de repasse,                 
                TarifaDeInterconexao = 0, // Será preenchido no calculo de repasse
                NumeroRPS = 0, //Preenchido pela query CriarPassagemAprovadaCompletaParkQuery
                SerieRPS = string.Empty, //Preenchido pela query CriarPassagemAprovadaCompletaParkQuery


                SurrogateKey = request.PassagemPendenteEstacionamento.RegistroTransacaoId,
                Detalhes = request.PassagemPendenteEstacionamento.Detalhes.Select(x => new DetalhePassagemEstacionamento
                {
                    PistaId = x.CodigoPista ?? 0,
                    DataHoraPassagem = x.Data ?? DateTime.Now,
                    SurrogateKey = request.PassagemPendenteEstacionamento.RegistroTransacaoId
                }).ToList()
            };

            PreencherRps(response);

            var calcularRepasse = new CalcularRepasseParkBo(_dataSourceConectSysReadOnly,
                                                            _dataSourceFallBack,
                                                            response.PassagemAprovadaEstacionamento);
            calcularRepasse.Calcular();

            return response;
        }

        private void PreencherRps(GerarPassagemAprovadaParkResponse response)
        {
            if (response.PassagemAprovadaEstacionamento.Conveniado.UtilizaRps && response.PassagemAprovadaEstacionamento.Valor > 0)
            {
                var qry = new CriarPassagemAprovadaCompletaParkQuery(_dataSourceConectSysReadOnly, _dataSourceFallBack);
                qry.Execute(response.PassagemAprovadaEstacionamento);
            }
        }
    }
}