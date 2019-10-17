using System;
using AutoMapper;
using ConectCar.Framework.Backend.CommonQuery.Query;
using ConectCar.Framework.Domain.Model;
using ConectCar.Framework.Infrastructure.Cqrs.Handlers;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Framework.Infrastructure.Data.Cache.DataProviders;
using ConectCar.Framework.Infrastructure.Data.ServiceBus.DataProviders;
using ConectCar.Transacoes.Domain.Dto;
using ConectCar.Transacoes.Domain.Enum;
using Newtonsoft.Json;
using ProcessadorPassagensActors.CommandQuery.Cache;
using ProcessadorPassagensActors.CommandQuery.Commands.Edi;
using ProcessadorPassagensActors.CommandQuery.Enums;
using ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Request;
using ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Responses;
using ProcessadorPassagensActors.CommandQuery.Messages.Edi;
using ProcessadorPassagensActors.CommandQuery.Util;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using ConectCar.Transacoes.Domain.Model;
using ProcessadorPassagensActors.CommandQuery.Queries;
using ProcessadorPassagensActors.Infrastructure;
using ConectCar.Framework.Infrastructure.Cqrs.Ado.Queries;
using ProcessadorPassagensActors.Infrastructure.Util;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Edi
{
    public class ProcessadorPassagemAprovadaEdiHandler : DataSourceHandlerBase,
        IAdoDataSourceProvider,
        ICacheDataSourceProvider
    {
        #region [Properties]

        private readonly ServiceBusDataSourceBase _dataSource;
        private PassagemAprovadaEDIDto _passagemAprovadaEdiDto;
        private readonly string _chaveCriptografiaBanco;
        private ProcessadorPassagemAprovadaEdiRequest _request;
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

        public ProcessadorPassagemAprovadaEdiHandler()
        {
            _dataSource = new ServiceBusDataSourceBase("TransacoesServiceBus", ServiceBusUtil.FactoriesCount);
            _dataSourceConectSysReadOnly = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSysReadOnly);
            _dataSourceFallBack = AdoDataSourceProvider.GetDataSource(DbConnectionDataSourceType.ConectSys);

            if (!string.IsNullOrEmpty(_chaveCriptografiaBanco)) return;
            var querySistema = new ObterConfiguracaoSistemaQuery(true, _dataSourceConectSysReadOnly, _dataSourceFallBack);
            _chaveCriptografiaBanco = querySistema.Execute(ConfiguracaoSistemaModel.ChaveCriptografiaBanco).Valor;
        }

        #endregion [Ctor]

        public ProcessadorPassagemAprovadaEdiResponse Execute(ProcessadorPassagemAprovadaEdiRequest request)
        {
            _request = request;
            _passagemAprovadaEdiDto = new PassagemAprovadaEDIDto();

            Log.Info($"Passagem DetalheTrnId: {request.PassagemAprovadaEdi.DetalheTrnId} Data Passagem {request.PassagemAprovadaEdi.DataPassagem.ToStringPtBr()}");

            PreencherPassagemAprovadaDto();

            PreencherDetalheTrfRecusado();

            //PreencherDetalheTrfManualmenteDto();

            PreencherDetalheViagens();

            PreencherEventoPrimeiraPassagemManual();

            PreencherExtrato();

            EnviarParaBarramento(request.PassagemAprovadaEdi?.DetalheTrnId ?? 0);

            return new ProcessadorPassagemAprovadaEdiResponse { Processado = true };
        }

        //private void PreencherDetalheTrfManualmenteDto()
        //{
        //    if (_request.DetalheTrfRecusado != null)
        //    {
        //        _passagemAprovadaEdiDto.DetalheTRFAprovadoManualmente = new DetalheTRFAprovadoManualmenteDto
        //        {
        //            Id = _request.PassagemAprovadaEdi.DetalheTrfAprovadoManualmenteId,
        //            SurrogateKey = _request.PassagemAprovadaEdi.DetalheTrnId
        //        };
        //    }
        //}

        private void PreencherPassagemAprovadaDto()
        {
            if (_request.PassagemAprovadaEdi?.Transacao != null)
            {
                _passagemAprovadaEdiDto.TransacaoPassagemEDI = new TransacaoPassagemEDIDto
                {
                    Id = _request.PassagemAprovadaEdi.Transacao.Id.TryToInt(),
                    AdesaoId = _request.PassagemAprovadaEdi.Transacao.AdesaoId,
                    CategoriaUtilizadaId = _request.PassagemAprovadaEdi.Transacao.CategoriaUtilizadaId,
                    ItemListaDeParaUtilizado =
                        _request.PassagemAprovadaEdi.Transacao.ItemListaDeParaUtilizado.TryToInt(),
                    OrigemTransacaoId = _request.PassagemAprovadaEdi.Transacao.OrigemTransacaoId,
                    Credito = _request.PassagemAprovadaEdi.Transacao.Credito,
                    Data = _request.PassagemAprovadaEdi.Transacao.Data,
                    DataDePassagem = _request.PassagemAprovadaEdi.Transacao.DataDePassagem,
                    DataRepasse = _request.PassagemAprovadaEdi.Transacao.DataRepasse,
                    Valor = _request.PassagemAprovadaEdi.Transacao.Valor,
                    ValorRepasse = _request.PassagemAprovadaEdi.Transacao.ValorRepasse,
                    TipoOperacaoId = _request.PassagemAprovadaEdi.Transacao.TipoOperacaoId,
                    PistaId = _request.PassagemAprovadaEdi.Transacao.PistaId,
                    RepasseId = _request.PassagemAprovadaEdi.Transacao.RepasseId,
                    StatusId = (int)StatusAutorizacao.Ativa,
                    TarifaDeinterconexao = _request.PassagemAprovadaEdi.Transacao.TarifaDeInterconexao,
                    SurroGateKey = _request.PassagemAprovadaEdi.DetalheTrnId,
                    DetalheTRNId = _request.PassagemAprovadaEdi.DetalheTrnId,
                    EvasaoAceita = _request.PassagemAprovadaEdi.PossuiEvasaoAceita,
                    ChaveCriptografiaBanco = _chaveCriptografiaBanco,
                    SaldoId = _request.PassagemAprovadaEdi.Adesao.SaldoId,
                    TransacaoDeCorrecaoId = null
                };

                if (_request.PassagemAprovadaEdi.StatusCobranca == StatusCobranca.Confirmacao)
                {
                    var obterTransacaoProvisoriaIdQuery = new ObterCountPossuiTransacaoProvisoriaQuery();
                    var possuiProvisoria = obterTransacaoProvisoriaIdQuery.Execute(_request.PassagemAprovadaEdi);

                    if (possuiProvisoria)
                    {
                        var obterProvisoriaQuery = new ObterTransacaoProvisoriaQuery();
                        var transacaoProvisoria = obterProvisoriaQuery.Execute(_request.PassagemAprovadaEdi);

                        _passagemAprovadaEdiDto.TransacaoPassagemEDI.TransacaoDeCorrecaoId = transacaoProvisoria.Id.TryToInt();

                    }
                }

                if (_request.PassagemAprovadaEdi.PossuiDivergenciaCategoriaVeiculo)
                {
                    _passagemAprovadaEdiDto.DivergenciaCategoriaConfirmada = new DivergenciaCategoriaConfirmadaDto
                    {
                        Id = _request.PassagemAprovadaEdi.Transacao.DivergenciaCategoriaConfirmada?.Id.TryToInt() ?? 0,
                        CategoriaVeiculoId =
                            _request.PassagemAprovadaEdi.Transacao.DivergenciaCategoriaConfirmada?.CategoriaVeiculo?.Id
                                .TryToInt() ?? 0,
                        Data = _request.PassagemAprovadaEdi.Transacao.Data,
                        TransacaoPassagemId = _request.PassagemAprovadaEdi.Transacao.Id.TryToInt(),
                        SurrogateKey = _request.PassagemAprovadaEdi.DetalheTrnId
                    };
                }
            }
            else if (_request.PassagemAprovadaEdi?.TransacaoProvisoria != null)
            {
                _passagemAprovadaEdiDto.TransacaoProvisoriaEDI = new TransacaoProvisoriaEDIDto
                {
                    Id = _request.PassagemAprovadaEdi.TransacaoProvisoria.Id.TryToInt(),
                    AdesaoId = _request.PassagemAprovadaEdi.TransacaoProvisoria.AdesaoId,
                    CategoriaUtilizadaId = _request.PassagemAprovadaEdi.TransacaoProvisoria.CategoriaUtilizadaId,
                    ItemListaDeParaUtilizado = _request.PassagemAprovadaEdi.TransacaoProvisoria.ItemListaDeParaUtilizado.TryToInt(),
                    OrigemTransacaoId = _request.PassagemAprovadaEdi.TransacaoProvisoria.OrigemTransacaoId,
                    Credito = _request.PassagemAprovadaEdi.TransacaoProvisoria.Credito,
                    Data = _request.PassagemAprovadaEdi.TransacaoProvisoria.Data,
                    DataDePassagem = _request.PassagemAprovadaEdi.TransacaoProvisoria.DataDePassagem,
                    DataRepasse = _request.PassagemAprovadaEdi.TransacaoProvisoria.DataRepasse,
                    Valor = _request.PassagemAprovadaEdi.TransacaoProvisoria.Valor,
                    ValorRepasse = _request.PassagemAprovadaEdi.TransacaoProvisoria.ValorRepasse,
                    TipoOperacaoId = _request.PassagemAprovadaEdi.TransacaoProvisoria.TipoOperacaoId,
                    PistaId = _request.PassagemAprovadaEdi.TransacaoProvisoria.PistaId,
                    RepasseId = _request.PassagemAprovadaEdi.TransacaoProvisoria.RepasseId,
                    StatusId = (int)StatusAutorizacao.Ativa,
                    DetalheTRNId = _request.PassagemAprovadaEdi.TransacaoProvisoria.DetalheTrnId,
                    TarifaDeinterconexao = _request.PassagemAprovadaEdi.TransacaoProvisoria.TarifaDeInterconexao,
                    SurroGateKey = _request.PassagemAprovadaEdi.TransacaoProvisoria.DetalheTrnId,
                    EvasaoAceita = _request.PassagemAprovadaEdi.PossuiEvasaoAceita,
                    ChaveCriptografiaBanco = _chaveCriptografiaBanco,
                    SaldoId = _request.PassagemAprovadaEdi.Adesao.SaldoId, //request.PassagemAprovadaEdi.Transacao.
                    TransacaoDeCorrecaoId = null
                };


                if (_request.PassagemAprovadaEdi.PossuiDivergenciaCategoriaVeiculo)
                    _passagemAprovadaEdiDto.DivergenciaCategoriaConfirmada = new DivergenciaCategoriaConfirmadaDto
                    {
                        Id = _request.PassagemAprovadaEdi.TransacaoProvisoria.DivergenciaCategoriaConfirmada?.Id
                                 .TryToInt() ?? 0,
                        CategoriaVeiculoId =
                            _request.PassagemAprovadaEdi.TransacaoProvisoria.DivergenciaCategoriaConfirmada
                                ?.CategoriaVeiculo?.Id.TryToInt() ?? 0,
                        Data = _request.PassagemAprovadaEdi.TransacaoProvisoria.Data,
                        TransacaoPassagemId = _request.PassagemAprovadaEdi.TransacaoProvisoria.Id.TryToInt(),
                        SurrogateKey = _request.PassagemAprovadaEdi.DetalheTrnId
                    };
            }

            if (_request?.PassagemAprovadaEdi?.Adesao?.Veiculo != null)
                _passagemAprovadaEdiDto.Veiculo = new VeiculoDto
                {
                    CategoriaVeiculoId = _request.PassagemAprovadaEdi.Adesao.Veiculo.Categoria.Id.TryToInt(),
                    Id = _request.PassagemAprovadaEdi.Adesao.Veiculo.Id.TryToInt(),
                    Placa = _request.PassagemAprovadaEdi.Adesao.Veiculo.Placa,
                    CategoriaConfirmada = _request.PassagemAprovadaEdi.Adesao.Veiculo.CategoriaConfirmada,
                    ContagemDivergenciaCategoriaConfirmada =
                        _request.PassagemAprovadaEdi.Adesao.Veiculo.ContagemDivergenciaCategoriaConfirmada,
                    ContagemConfirmacaoCategoria =
                        _request.PassagemAprovadaEdi.Adesao.Veiculo.ContagemConfirmacaoCategoria,
                    DataConfirmacaoCategoria = _request.PassagemAprovadaEdi.Adesao.Veiculo.DataConfirmacaoCategoria,
                };

            if (_request?.PassagemAprovadaEdi?.Adesao?.ConfiguracaoAdesao != null && (_request?.PassagemAprovadaEdi?.Adesao?.ConfiguracaoAdesao.Categoria.Id ?? 0) > 0)
                _passagemAprovadaEdiDto.ConfiguracaoAdesao = new ConfiguracaoAdesaoDto
                {
                    CategoriaId = _request.PassagemAprovadaEdi.Adesao.ConfiguracaoAdesao.Categoria.Id.TryToInt(),
                    Id = _request.PassagemAprovadaEdi.Adesao.ConfiguracaoAdesao.Id.TryToInt(),
                };
        }

        private void PreencherDetalheTrfRecusado()
        {
            if (_request.DetalheTrfRecusado != null)
            {
                _passagemAprovadaEdiDto.DetalheTRFRecusaEvasao = new DetalheTRFRecusadoDto
                {
                    Id = _request.DetalheTrfRecusado.Id.TryToInt(),
                    ArquivoTRFId = _request.PassagemAprovadaEdi.ArquivoTrfId.TryToInt(),
                    DetalheTRNId = _request.PassagemAprovadaEdi.DetalheTrnId.TryToInt(),
                    CodigoRetornoId = (int)_request.DetalheTrfRecusado.CodigoRetorno,
                    SurrogateKey = _request.PassagemAprovadaEdi.DetalheTrnId
                };
            }
        }

        private void PreencherDetalheViagens()
        {
            if (_request.DetalheViagens != null && _request.DetalheViagens.Any())
            {
                _passagemAprovadaEdiDto.Viagens = new List<DetalheViagemDto>();

                foreach (var detalheViagem in _request.DetalheViagens)
                {
                    var dto = new DetalheViagemDto
                    {
                        CodigoPracaRoadCard = detalheViagem.CodigoPracaRoadCard,
                        DataCancelamento = detalheViagem.DataCancelamento,
                        Id = detalheViagem.Id.TryToInt(),
                        PracaId = detalheViagem.PracaId,
                        Sequencia = detalheViagem.Sequencia,
                        StatusId = detalheViagem.StatusDetalheViagemId,
                        ValorPassagem = detalheViagem.ValorPassagem,
                        ViagemId = (detalheViagem?.Viagem?.Id ?? 0).TryToInt(),
                    };

                    // caso o detalheviagem tenha sido consumido, vincular SurrogateKey
                    if (dto.StatusId == (int)StatusDetalheViagem.Utilizada)
                    {
                        dto.SurrogateKey = _request.PassagemAprovadaEdi.DetalheTrnId;
                    }
                    _passagemAprovadaEdiDto.Viagens.Add(dto);
                }

            }
        }

        private void PreencherEventoPrimeiraPassagemManual()
        {
            if (_request.Evento != null)
            {
                _passagemAprovadaEdiDto.EventoPrimeiraPassagemManual = new EventoDto
                {
                    DataCriacao = DateTime.Now,
                    IdRegistro = _request.PassagemAprovadaEdi.Adesao.Id.TryToInt(),
                    TipoEventoId = (int)TipoEvento.PrimeiraPassagemManual
                };
            }
        }

        private void PreencherExtrato()
        {
            var transacao = _request.PassagemAprovadaEdi.Transacao ?? _request.PassagemAprovadaEdi.TransacaoProvisoria;

            var dataPassagem = _request.PassagemAprovadaEdi.DataPassagem;
            var nomeFatura = _request.PassagemAprovadaEdi.Conveniado.NomeFatura;
            var nomeFantasia = _request.PassagemAprovadaEdi.Conveniado.NomeFantasia;
            var identificacaoPraca = _request.PassagemAprovadaEdi.Praca.IdentificacaoPraca;

            var sb = new StringBuilder();
            if (_request.DetalheViagens != null && _request.DetalheViagens.Any())
            {
                var detalheViagem = _request.DetalheViagens.FirstOrDefault();
                if (detalheViagem != null)
                    sb.Append(
                        $@"Passagem Vale Pedágio em {dataPassagem.ToStringPtBr()} Embarcador {
                                detalheViagem.Viagem.Embarcador
                            } / CNPJ {detalheViagem.Viagem.CnpjEmbarcador} / Viagem {
                                detalheViagem.Viagem.CodigoViagemParceiro
                            }");
            }
            sb.Append(!string.IsNullOrEmpty(nomeFatura) ? nomeFatura.PadLeft(' ') : nomeFantasia.PadLeft(' '));
            sb.Append(identificacaoPraca.PadLeft(' '));

            _passagemAprovadaEdiDto.Extrato = new ExtratoDto
            {
                AdesaoId = _request.PassagemAprovadaEdi.Adesao.Id.TryToInt(),
                ChaveCriptografiaBanco = _chaveCriptografiaBanco,
                DataTransacao = transacao.Data,
                Placa = _request.PassagemAprovadaEdi.Placa,
                SubDescricao = _request.PassagemAprovadaEdi.DataPassagem.ToStringPtBr(),
                SurrogateKey = transacao.DetalheTrnId,
                TipoOperacaoId = transacao.TipoOperacaoId,
                TransacaoId = 0,
                ValorD = transacao.Valor,
                Descricao = sb.ToString()
            };
        }

        private void EnviarParaBarramento(long detalheTrnId)
        {
            var topicName = ServiceBusUtil.ObterNomeTopicAprovada(EnumInfra.ProtocolosEnum.PassagensAprovadasEdi);

            var passagemCommand = new PassagemAprovadaEDITopicCommand(_dataSource, true, topicName);
            var mensagem = Mapper.Map(_passagemAprovadaEdiDto, new PassagemAprovadaEDIMessage());

            Log.Info($"Json saida TRN - {detalheTrnId} (aprovado): {JsonConvert.SerializeObject(new List<PassagemAprovadaEDIMessage> { mensagem })}");
            //Enviando para o barramento...
            passagemCommand.Execute(new List<PassagemAprovadaEDIMessage> { mensagem });
            Log.Info($"DetalheTrn ID: {detalheTrnId} | Detalhe Processado.");
        }
    }
}