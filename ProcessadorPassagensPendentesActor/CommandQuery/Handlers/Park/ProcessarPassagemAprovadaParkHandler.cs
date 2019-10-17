using AutoMapper;
using ConectCar.Framework.Backend.CommonQuery.Query;
using ConectCar.Framework.Domain.Model;
using ConectCar.Framework.Infrastructure.Cqrs.Handlers;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Framework.Infrastructure.Data.Cache.DataProviders;
using ConectCar.Framework.Infrastructure.Data.ServiceBus.DataProviders;
using ConectCar.Transacoes.Domain.Dto;
using ConectCar.Transacoes.Domain.Model;
using Newtonsoft.Json;
using ProcessadorPassagensActors.CommandQuery.Cache;
using ProcessadorPassagensActors.CommandQuery.Commands.Park;
using ProcessadorPassagensActors.CommandQuery.Enums;
using ProcessadorPassagensActors.CommandQuery.Handlers.Park.Request;
using ProcessadorPassagensActors.CommandQuery.Handlers.Park.Response;
using ProcessadorPassagensActors.CommandQuery.Messages.Park;
using ProcessadorPassagensActors.CommandQuery.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProcessadorPassagensActors.Infrastructure;
using ProcessadorPassagensActors.Infrastructure.Util;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Park
{
    public class ProcessadorPassagemAprovadaParkHandler : DataSourceHandlerBase,
        IAdoDataSourceProvider,
        ICacheDataSourceProvider
    {
        #region [Properties]

        private readonly ServiceBusDataSourceBase _dataSource;
        private PassagemAprovadaEstacionamentoDto _passagemAprovadaDto;
        private readonly string _chaveCriptografiaBanco;
        private ProcessarPassagemAprovadaParkRequest _request;
        private readonly DbConnectionDataSource _dataSourceConectSysReadOnly;
        private readonly DbConnectionDataSource _dataSourceFallBack;
        public DbConnectionDataSourceProvider AdoDataSourceProvider => GetAdoProvider();
        public CacheDataSourceProvider CacheDataSourceProvider => GetCacheProvider();

        protected override void Init()
        {
            AddProvider(new DbConnectionDataSourceProvider());
            AddProvider(new CacheDataSourceProvider(new CommonCacheDataSourceProvider()));
        }

        #endregion [Properties]

        #region [Ctor]

        public ProcessadorPassagemAprovadaParkHandler()
        {
            _dataSource = new ServiceBusDataSourceBase("TransacoesServiceBus", ServiceBusUtil.FactoriesCount);
            _dataSourceConectSysReadOnly = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSysReadOnly);
            _dataSourceFallBack = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSys);

            if (!string.IsNullOrEmpty(_chaveCriptografiaBanco)) return;
            var querySistema = new ObterConfiguracaoSistemaQuery(true, _dataSourceConectSysReadOnly, _dataSourceFallBack);
            _chaveCriptografiaBanco = querySistema.Execute(ConfiguracaoSistemaModel.ChaveCriptografiaBanco).Valor;
        }

        #endregion [Ctor]

        public ProcessarPassagemAprovadaParkResponse Execute(ProcessarPassagemAprovadaParkRequest request)
        {
            _request = request;

            _passagemAprovadaDto = new PassagemAprovadaEstacionamentoDto();

            PreencherTransacaoEstacionamento();
            PreencherPistaInformacoesRps();
            PreencherConveniadoInformacoesRps();
            PreencherDetalhePassagemEstacionamento();
            PreencherExtrato();
            EnviarParaBarramento();

            return new ProcessarPassagemAprovadaParkResponse { Processado = true };
        }

        private void PreencherDetalhePassagemEstacionamento()
        {
            _passagemAprovadaDto.TransacaoEstacionamento.Detalhes = _request.PassagemAprovadaEstacionamento.Detalhes.Select(x => new DetalhePassagemEstacionamento
            {
                DataHoraPassagem = x.Data ?? new DateTime(),
                PistaId = x.CodigoPista ?? 0,
                SurrogateKey = _passagemAprovadaDto.TransacaoEstacionamento.SurrogateKey
            }).ToList();
        }

        private void PreencherTransacaoEstacionamento()
        {
            _passagemAprovadaDto.TransacaoEstacionamento = new TransacaoEstacionamentoDto();
            _passagemAprovadaDto.TransacaoEstacionamento.RegistroTransacaoId =_request.PassagemAprovadaEstacionamento.RegistroTransacaoId;
            _passagemAprovadaDto.TransacaoEstacionamento.Credito = _request.PassagemAprovadaEstacionamento?.TransacaoEstacionamento?.Credito ?? false;
            _passagemAprovadaDto.TransacaoEstacionamento.Data = _request.PassagemAprovadaEstacionamento?.TransacaoEstacionamento?.Data ?? new DateTime();
            _passagemAprovadaDto.TransacaoEstacionamento.StatusId = _request.PassagemAprovadaEstacionamento?.TransacaoEstacionamento?.StatusId ?? 0;
            _passagemAprovadaDto.TransacaoEstacionamento.TipoOperacaoId = _request.PassagemAprovadaEstacionamento?.TransacaoEstacionamento?.TipoOperacaoId ?? 0;
            _passagemAprovadaDto.TransacaoEstacionamento.AdesaoId = _request.PassagemAprovadaEstacionamento?.TransacaoEstacionamento?.AdesaoId ?? 0;
            _passagemAprovadaDto.TransacaoEstacionamento.Valor = _request.PassagemAprovadaEstacionamento?.TransacaoEstacionamento?.Valor ?? 0;
            _passagemAprovadaDto.TransacaoEstacionamento.SaldoId = _request.PassagemAprovadaEstacionamento.Adesao.SaldoId;
            _passagemAprovadaDto.TransacaoEstacionamento.TarifaDeInterconexao = _request.PassagemAprovadaEstacionamento?.TransacaoEstacionamento?.TarifaDeInterconexao ?? 0;
            _passagemAprovadaDto.TransacaoEstacionamento.DataRepasse = _request.PassagemAprovadaEstacionamento?.TransacaoEstacionamento?.DataRepasse ?? new DateTime();
            _passagemAprovadaDto.TransacaoEstacionamento.ValorRepasse = _request.PassagemAprovadaEstacionamento?.TransacaoEstacionamento?.ValorRepasse ?? 0;
            _passagemAprovadaDto.TransacaoEstacionamento.Mensalista = _request.PassagemAprovadaEstacionamento?.TransacaoEstacionamento?.Mensalista ?? false;
            _passagemAprovadaDto.TransacaoEstacionamento.Ticket = _request.PassagemAprovadaEstacionamento?.TransacaoEstacionamento?.Ticket ?? string.Empty;
            _passagemAprovadaDto.TransacaoEstacionamento.DataReferencia = _request.PassagemAprovadaEstacionamento?.TransacaoEstacionamento?.DataReferencia ?? new DateTime();
            _passagemAprovadaDto.TransacaoEstacionamento.TipoTransacaoEstacionamentoId = _request.PassagemAprovadaEstacionamento?.TransacaoEstacionamento?.TipoTransacaoEstacionamentoId ?? 0;
            _passagemAprovadaDto.TransacaoEstacionamento.MotivoAtrasoTransmissaoId = _request.PassagemAprovadaEstacionamento?.TransacaoEstacionamento?.MotivoAtrasoTransmissaoId ?? 0;
            _passagemAprovadaDto.TransacaoEstacionamento.NumeroRPS = _request.PassagemAprovadaEstacionamento?.TransacaoEstacionamento?.NumeroRPS ?? 0;
            _passagemAprovadaDto.TransacaoEstacionamento.RepasseId = _request.PassagemAprovadaEstacionamento?.TransacaoEstacionamento?.RepasseId ?? 0;
            _passagemAprovadaDto.TransacaoEstacionamento.SerieRPS = _request.PassagemAprovadaEstacionamento?.TransacaoEstacionamento?.SerieRPS ?? string.Empty;
            _passagemAprovadaDto.TransacaoEstacionamento.MotivoDesconto = _request.PassagemAprovadaEstacionamento?.TransacaoEstacionamento?.MotivoDesconto ?? string.Empty;
            _passagemAprovadaDto.TransacaoEstacionamento.ValorDesconto = _request.PassagemAprovadaEstacionamento?.TransacaoEstacionamento?.ValorDesconto ?? 0;
            _passagemAprovadaDto.TransacaoEstacionamento.DataHoraEntrada = _request.PassagemAprovadaEstacionamento?.TransacaoEstacionamento?.DataHoraEntrada ?? new DateTime();
            _passagemAprovadaDto.TransacaoEstacionamento.DataHoraTransacao = _request.PassagemAprovadaEstacionamento?.TransacaoEstacionamento?.DataHoraTransacao ?? new DateTime();
            _passagemAprovadaDto.TransacaoEstacionamento.PracaId = _request.PassagemAprovadaEstacionamento?.TransacaoEstacionamento?.PracaId ?? 0;
            _passagemAprovadaDto.TransacaoEstacionamento.PistaId = _request.PassagemAprovadaEstacionamento?.TransacaoEstacionamento?.PistaId ?? 0;
            _passagemAprovadaDto.TransacaoEstacionamento.TagId = _request.PassagemAprovadaEstacionamento?.TransacaoEstacionamento?.TagId ?? 0;
            _passagemAprovadaDto.TransacaoEstacionamento.ConveniadoId = _request.PassagemAprovadaEstacionamento?.TransacaoEstacionamento?.ConveniadoId ?? 0;
            _passagemAprovadaDto.TransacaoEstacionamento.Detalhes = _request.PassagemAprovadaEstacionamento?.TransacaoEstacionamento?.Detalhes ?? new List<ConectCar.Transacoes.Domain.Model.DetalhePassagemEstacionamento>();
            _passagemAprovadaDto.TransacaoEstacionamento.TempoPermanencia = _request.PassagemAprovadaEstacionamento?.TransacaoEstacionamento?.TempoPermanencia ?? 0;
            _passagemAprovadaDto.TransacaoEstacionamento.SurrogateKey = _request.PassagemAprovadaEstacionamento.RegistroTransacaoId;
        }

        private void PreencherPistaInformacoesRps()
        {
            _passagemAprovadaDto.PistaInformacoesRPS = new PistaInformacoesRPSDto
            {
                ConveniadoInformacoesRPSId =
                    _request.PassagemAprovadaEstacionamento?.PistaInformacoesRPS?.ConveniadoInformacoesRPSId ?? 0,
                SerieRPS = _request.PassagemAprovadaEstacionamento?.PistaInformacoesRPS?.SerieRPS ?? string.Empty,
                NumeroRPS = _request.PassagemAprovadaEstacionamento?.PistaInformacoesRPS?.NumeroRPS ?? 0,
                PistaId = _request.PassagemAprovadaEstacionamento?.PistaInformacoesRPS?.PistaId ?? 0,
                DataCriacao = _request.PassagemAprovadaEstacionamento?.PistaInformacoesRPS?.DataCriacao ?? new DateTime(),
                Id = _request.PassagemAprovadaEstacionamento?.PistaInformacoesRPS?.Id,
            };
        }

        private void PreencherConveniadoInformacoesRps()
        {
            _passagemAprovadaDto.ConveniadoInformacoesRPS = new ConveniadoInformacoesRpsDto
            {
                SerieRPS = _request.PassagemAprovadaEstacionamento?.ConveniadoInformacoesRPS?.SerieRPS ?? string.Empty,
                NumeroRPS = _request.PassagemAprovadaEstacionamento?.ConveniadoInformacoesRPS?.NumeroRPS ?? 0,
                TipoRps = _request.PassagemAprovadaEstacionamento?.ConveniadoInformacoesRPS?.TipoRps ?? 0,
                Id = _request.PassagemAprovadaEstacionamento?.ConveniadoInformacoesRPS ?.Id
            };
        }

        private void PreencherExtrato()
        {
            var transacao = _request.PassagemAprovadaEstacionamento.TransacaoEstacionamento;

          
            var nomeFatura = _request.PassagemAprovadaEstacionamento.Conveniado.NomeFatura ?? _request.PassagemAprovadaEstacionamento.Conveniado.NomeFantasia;
            var nomeFantasia = _request.PassagemAprovadaEstacionamento.Conveniado.NomeFantasia;
            var identificacaoPraca = _request.PassagemAprovadaEstacionamento.Praca.IdentificacaoPraca;

            var sb = new StringBuilder();
          
            sb.Append(!string.IsNullOrEmpty(nomeFatura) ? nomeFatura.PadLeft(' ') : nomeFantasia.PadLeft(' '));
            sb.Append(identificacaoPraca.PadLeft(' '));

            _passagemAprovadaDto.Extrato = new ExtratoDto
            {
                AdesaoId = _request.PassagemAprovadaEstacionamento.Adesao.Id.TryToInt(),
                ChaveCriptografiaBanco = _chaveCriptografiaBanco,
                DataTransacao = transacao.Data,
                Placa = _request.PassagemAprovadaEstacionamento.Adesao.Veiculo.Placa,
                SubDescricao = _request.PassagemAprovadaEstacionamento.DataPassagem.ToStringPtBr(),
                SurrogateKey = transacao.RegistroTransacaoId,
                TipoOperacaoId = transacao.TipoOperacaoId,
                TransacaoId = 0,
                ValorD = transacao.Valor,
                Descricao = sb.ToString()
            };
        }

        private void EnviarParaBarramento()
        {
            var topicName = ServiceBusUtil.ObterNomeTopicAprovada(EnumInfra.ProtocolosEnum.PassagensAprovadasPark);

            var passagemCommand = new PassagemAprovadaParkTopicCommand(_dataSource, true, topicName);
            var mensagem = Mapper.Map(_passagemAprovadaDto, new PassagemAprovadaParkMessage());
            passagemCommand.Execute(new List<PassagemAprovadaParkMessage> { mensagem });
            Log.Info($"Json saida RegistroTransacaoId - {_passagemAprovadaDto.TransacaoEstacionamento.RegistroTransacaoId} (aprovado): {JsonConvert.SerializeObject(new List<PassagemAprovadaParkMessage> { mensagem })}");
        }
    }
}