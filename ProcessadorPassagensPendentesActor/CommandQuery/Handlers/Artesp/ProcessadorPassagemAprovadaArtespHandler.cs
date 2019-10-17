using System;
using System.Collections.Generic;
using AutoMapper;
using ConectCar.Framework.Infrastructure.Data.ServiceBus.DataProviders;
using ConectCar.Framework.Infrastructure.Log;
using ConectCar.Transacoes.Domain.Dto;
using ConectCar.Transacoes.Domain.Enum;
using ProcessadorPassagensActors.CommandQuery.Commands.Artesp;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests;
using ProcessadorPassagensActors.CommandQuery.Messages.Artesp;
using System.Text;
using System.Linq;
using ProcessadorPassagensActors.Infrastructure;
using ProcessadorPassagensActors.Infrastructure.Util;
using Microsoft.Azure.Documents.SystemFunctions;
using ProcessadorPassagensActors.CommandQuery.Enums;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp
{
    public class ProcessadorPassagemAprovadaArtespHandler : Loggable
    {
        private readonly ServiceBusDataSourceBase _dataSource;
        public ProcessadorPassagemAprovadaArtespHandler()
        {
            _dataSource = new ServiceBusDataSourceBase("TransacoesServiceBus", ServiceBusUtil.FactoriesCount);
        }

        /// <summary>
        /// Envia passagens aprovadas para o barramento...
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public void Execute(ProcessadorPassagemAprovadaRequest request)
        {
            try
            {
                var passagemAprovadaArtespDto = new PassagemAprovadaArtespDto();

                PreencherPassagemProcessada(request, passagemAprovadaArtespDto);
                PreencherPassagem(request, passagemAprovadaArtespDto);
                PreencherTransacaoPassagem(request, passagemAprovadaArtespDto);
                PreencherExtrato(request, passagemAprovadaArtespDto);
                PreencherEventoPrimeiraPassagemManual(request, passagemAprovadaArtespDto);
                PreencherAceiteManualReenvioPassagemDto(request, passagemAprovadaArtespDto);
                PreencherSolicitacaoImagemDto(request, passagemAprovadaArtespDto);
                PreencherConfiguracaoAdesaoDto(request, passagemAprovadaArtespDto);
                PreencherDivergenciaCategoriaConfirmadaDto(request, passagemAprovadaArtespDto);
                PreencherVeiculoDto(request, passagemAprovadaArtespDto);
                PreencherEstornoPassagemDto(request, passagemAprovadaArtespDto);
                PreencherViagens(request, passagemAprovadaArtespDto);                

                EnviarParaBarramento(passagemAprovadaArtespDto, request.PassagemAprovadaArtesp.MensagemItemId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        /// <summary>
        /// Esse metodo não faz nada do lado do sys, só gera uma passagem processada do lado do mensageria e remove a passagem do item pendente.
        /// </summary>
        /// <param name="request"></param>
        public void Execute(GeradorPassagemProcessadaMensageriaRequest request)
        {
            var passagemAprovadaArtespDto = new PassagemAprovadaArtespDto
            {
                PassagemProcessada = new PassagemProcessadaArtespDto
                {
                    MotivoNaoCompensado = (int)request.MotivoNaoCompensado,
                    MensagemId = request.PassagemPendenteArtesp.Mensagem.Id ?? 0,
                    MensagemItemId = request.PassagemPendenteArtesp.MensagemItemId,
                    Reenvio = request.PassagemPendenteArtesp.NumeroReenvio,
                    Resultado = (int)request.ResultadoPassagem,
                    ValePedagio = false,
                    DataPagamento = request.DataPagamento.ToUnixDate(),
                    MotivoOutroValor = null,
                    MotivoProvisionado = null,
                    PassagemId = request.PassagemId,
                    Valor = Convert.ToInt32(request.Valor * 100),
                    TransacaoId = null,
                }
            };

            if (passagemAprovadaArtespDto.PassagemProcessada.DataPagamento == DateTime.MinValue.ToUnixDate())
                passagemAprovadaArtespDto.PassagemProcessada.DataPagamento = null;
            
            EnviarParaBarramento(passagemAprovadaArtespDto, request.PassagemPendenteArtesp.MensagemItemId);
        }


        private void EnviarParaBarramento(PassagemAprovadaArtespDto passagemAprovadaArtespDto, long mensagemItemId)
        {
            var topicName = ServiceBusUtil.ObterNomeTopicAprovada(EnumInfra.ProtocolosEnum.PassagensAprovadasArtesp);
            var passagemCommand = new PassagemAprovadaArtespTopicCommand(_dataSource, true, topicName);


            var mensagem = Mapper.Map<PassagemAprovadaMessage>(passagemAprovadaArtespDto);
            Log.Info($"Passagem ID: {mensagemItemId} | Passagem Aprovada.");
            passagemCommand.Execute(new List<PassagemAprovadaMessage> { mensagem }, (p) =>
            {
                var properties =
                    new Dictionary<string, object> {{"MensagemItemId", p.PassagemProcessada.MensagemItemId}};
                return properties;
            });
            Log.Info($"Passagem ID: {mensagemItemId} | Passagem Aprovada - Barramento.");

        }

        /// <summary>
        /// Preenche as informações de passagem processada para sinalizar a Mensageria.
        /// </summary>
        /// <param name="request">ProcessadorPassagemAprovadaRequest</param>
        /// <param name="passagemApoAprovadaArtespDto">PassagemAprovadaArtespDTO</param>
        private void PreencherPassagemProcessada(ProcessadorPassagemAprovadaRequest request,
            PassagemAprovadaArtespDto passagemApoAprovadaArtespDto)
        {
            Log.Debug($"Passagem ID: {request.PassagemAprovadaArtesp.MensagemItemId} - Fluxo: ProcessadorPassagemAprovadaHandler | Preencher Passagem Processada");

            passagemApoAprovadaArtespDto.PassagemProcessada = new PassagemProcessadaArtespDto
            {
                MotivoNaoCompensado = (int)MotivoNaoCompensado.SemMotivoNaoCompensado,
                MensagemId = request.PassagemAprovadaArtesp.Mensagem.Id ?? 0,
                MensagemItemId = request.PassagemAprovadaArtesp.MensagemItemId,
                Reenvio = request.PassagemAprovadaArtesp.NumeroReenvio,
                Resultado = (int)ResultadoPassagem.Compensado,
                ValePedagio = request.PassagemAprovadaArtesp.ValePedagio,
                DataPagamento = request.PassagemAprovadaArtesp.DataPassagem != DateTime.MinValue ? request.PassagemAprovadaArtesp.DataPassagem.ToUtcDate().AddHours(12).ToUnixDate() : (long?)null,
                MotivoOutroValor = null,
                MotivoProvisionado = null,
                PassagemId = 0,
                Valor = Convert.ToInt32(request.PassagemAprovadaArtesp.Transacao.ValorRepasse * 100),
                TransacaoId = null,
            };

            if (request.PassagemAprovadaArtesp.Valor == 0 &&
                request.PassagemAprovadaArtesp.MotivoSemValor != MotivoSemValor.CobrancaIndevida &&
                request.PassagemAprovadaArtesp.NumeroReenvio > 0)
            {
                if (request.PassagemAprovadaArtesp.MotivoSemValor == MotivoSemValor.GrupoIsento ||
                    request.PassagemAprovadaArtesp.MotivoSemValor == MotivoSemValor.IsentoConcessionaria)
                {
                    passagemApoAprovadaArtespDto.PassagemProcessada.MotivoNaoCompensado = (int)MotivoNaoCompensado.Isento;
                    passagemApoAprovadaArtespDto.PassagemProcessada.Resultado = (int)ResultadoPassagem.NaoCompensado;
                }
                else
                {
                    passagemApoAprovadaArtespDto.PassagemProcessada.MotivoNaoCompensado = (int)MotivoNaoCompensado.TagNaoPertenceGrupoIsento;
                    passagemApoAprovadaArtespDto.PassagemProcessada.Resultado = (int)ResultadoPassagem.NaoCompensado;
                }
            }

        }

        /// <summary>
        /// Preenche as informações de passagem. Será salva no ConectSys.
        /// </summary>
        /// <param name="request">ProcessadorPassagemAprovadaRequest</param>
        /// <param name="passagemApoAprovadaArtespDto">PassagemAprovadaArtespDTO</param>
        private void PreencherPassagem(ProcessadorPassagemAprovadaRequest request,
           PassagemAprovadaArtespDto passagemApoAprovadaArtespDto)
        {
            Log.Debug($"Passagem ID: {request.PassagemAprovadaArtesp.MensagemItemId} - Fluxo: ProcessadorPassagemAprovadaHandler | Preencher Passagem");

            passagemApoAprovadaArtespDto.Passagem = new PassagemDto
            {
                Id = request.PassagemAprovadaArtesp.Id ?? 0,
                AdesaoId = request.PassagemAprovadaArtesp.Adesao.Id.TryToInt(),
                CategoriaDetectadaId = request.PassagemAprovadaArtesp.CategoriaDetectada.Id.TryToInt(),
                CategoriaCobradaId = request.PassagemAprovadaArtesp.CategoriaCobrada.Id.TryToInt(),
                CategoriaTagId = request.PassagemAprovadaArtesp.CategoriaTag.Id.TryToInt(),
                CodigoPassagemConveniado = request.PassagemAprovadaArtesp.ConveniadoPassagemId,
                CodigoPista = request.PassagemAprovadaArtesp.Pista.CodigoPista,
                CodigoPraca = request.PassagemAprovadaArtesp.Praca.CodigoPraca.TryToInt(),
                ConveniadoId = request.PassagemAprovadaArtesp.Conveniado.Id.TryToInt(),
                Data = request.PassagemAprovadaArtesp.DataPassagem.ToUtcDate(),
                PassagemRecusadaMensageria = request.PassagemAprovadaArtesp.PassagemRecusadaMensageria ?? false,
                Placa = request.PassagemAprovadaArtesp.Placa,
                MensagemItemId = request.PassagemAprovadaArtesp.MensagemItemId,
                Reenvio = request.PassagemAprovadaArtesp.NumeroReenvio,
                TagId = request.PassagemAprovadaArtesp.Tag.Id.TryToInt(),
                TagBloqueadaMomentoRecebimento = request.PassagemAprovadaArtesp.TagBloqueadaMomentoRecebimento ?? false,
                Valor = request.PassagemAprovadaArtesp.Valor,
                StatusCobrancaId = (int)request.PassagemAprovadaArtesp.StatusCobranca,
                MotivoSemvalorId = (int)request.PassagemAprovadaArtesp.MotivoSemValor,
                SomenteInformacoesAlteradas = request.PassagemAprovadaArtesp.SomenteInformacoesAlteradas,
                StatusPassagemId = (int)request.PassagemAprovadaArtesp.StatusPassagem,
                SurrogateKey = request.PassagemAprovadaArtesp.MensagemItemId
            };
        }

        /// <summary>
        /// Preenche uma transação passagem
        /// </summary>
        /// <param name="request">ProcessadorPassagemAprovadaRequest</param>
        /// <param name="passagemApoAprovadaArtespDto">PassagemAprovadaArtespDto</param>
        private void PreencherTransacaoPassagem(ProcessadorPassagemAprovadaRequest request,
           PassagemAprovadaArtespDto passagemApoAprovadaArtespDto)
        {
            Log.Debug($"Passagem ID: {request.PassagemAprovadaArtesp.MensagemItemId} - Fluxo: ProcessadorPassagemAprovadaHandler | Preencher Transação Passagem");

            var dto = new TransacaoPassagemArtespDto
            {
                AdesaoId = request.PassagemAprovadaArtesp.Adesao.Id.TryToInt(),
                CategoriaUtilizadaId = request.PassagemAprovadaArtesp.Transacao.CategoriaUtilizadaId,
                ChaveCriptografiaBanco = string.Empty,
                Credito = false,
                Data = request.PassagemAprovadaArtesp.Transacao.Data,
                DataDePassagem = request.PassagemAprovadaArtesp.DataPassagem.ToUtcDate(),
                DataRepasse = request.PassagemAprovadaArtesp.Transacao.DataRepasse,
                //EstornoId = //, -- Vale Pedagio
                Id = request.PassagemAprovadaArtesp.Transacao.Id.TryToInt(),
                ItemListaDeParaUtilizado = request.PassagemAprovadaArtesp.ItemListaDeParaUtilizado,
                OrigemTransacaoId = request.PassagemAprovadaArtesp.Transacao.OrigemTransacaoId,
                PassagemId = request.PassagemAprovadaArtesp.Id.TryToInt(),
                PistaId = request.PassagemAprovadaArtesp.Transacao.PistaId,
                RepasseId = request.PassagemAprovadaArtesp.Transacao.RepasseId,
                SaldoId = request.PassagemAprovadaArtesp.Adesao.SaldoId,
                StatusId = request.PassagemAprovadaArtesp.Transacao.StatusId,
                SurroGateKey = request.PassagemAprovadaArtesp.MensagemItemId,
                TarifaDeinterconexao = request.PassagemAprovadaArtesp.Transacao.TarifaDeInterconexao,
                TipoOperacaoId = request.PassagemAprovadaArtesp.Transacao.TipoOperacaoId,
                Valor = request.PassagemAprovadaArtesp.Transacao.Valor,
                ValorRepasse = request.PassagemAprovadaArtesp.Transacao.ValorRepasse
            };

            // caso exista alguma viagem utilizado, altera o tipo de operação da transação.
            if (request.PassagemAprovadaArtesp.ValePedagio)
            {
                if (request.PassagemAprovadaArtesp.Transacao.Viagens != null &&
                    request.PassagemAprovadaArtesp.Transacao.Viagens.Any(x => x.StatusDetalheViagemId == (int)StatusDetalheViagem.Utilizada))
                {
                    dto.TipoOperacaoId = (int)TipoOperacaoMovimentoFinanceiro.PassagemValePedagio;
                }
            }

            passagemApoAprovadaArtespDto.TransacaoPassagemArtesp = dto;
        }

        /// <summary>
        /// Peencher o extrato.
        /// </summary>
        /// <param name="request">ProcessadorPassagemAprovadaRequest</param>
        /// <param name="passagemAprovadaArtespDto">PassagemAprovadaArtespDto</param>
        private void PreencherExtrato(ProcessadorPassagemAprovadaRequest request,
            PassagemAprovadaArtespDto passagemAprovadaArtespDto)
        {
            Log.Debug($"Passagem ID: {request.PassagemAprovadaArtesp.MensagemItemId} - Fluxo: ProcessadorPassagemAprovadaHandler | Preencher Extrato");

            var dataPassagem = request.PassagemAprovadaArtesp.DataPassagem;
            var nomeFatura = request.PassagemAprovadaArtesp.Conveniado.NomeFatura;
            var nomeFantasia = request.PassagemAprovadaArtesp.Conveniado.NomeFantasia;
            var identificacaoPraca = request.PassagemAprovadaArtesp.Praca.IdentificacaoPraca;

            var sb = new StringBuilder();
            if (request.PassagemAprovadaArtesp.ValePedagio)
            {
                if (request.PassagemAprovadaArtesp.Transacao.Viagens != null && request.PassagemAprovadaArtesp.Transacao.Viagens.Any())
                {
                    var detalheViagem = request.PassagemAprovadaArtesp.Transacao.Viagens.FirstOrDefault();

                    sb.Append($@"Passagem Vale Pedágio em {dataPassagem.ToStringPtBr()} Embarcador {detalheViagem.Viagem.Embarcador} / CNPJ {detalheViagem.Viagem.CnpjEmbarcador} / Viagem {detalheViagem.Viagem.CodigoViagemParceiro}");
                }
            }
            sb.Append(!string.IsNullOrEmpty(nomeFatura) ? nomeFatura.PadLeft(' ') : nomeFantasia.PadLeft(' '));
            sb.Append(identificacaoPraca.PadLeft(' '));

            passagemAprovadaArtespDto.Extrato = new ExtratoDto
            {
                AdesaoId = request.PassagemAprovadaArtesp.Adesao.Id.TryToInt(),
                ChaveCriptografiaBanco = string.Empty,
                DataTransacao = request.PassagemAprovadaArtesp.Transacao.Data,
                Descricao = sb.ToString(),
                Placa = request.PassagemAprovadaArtesp.Placa,
                SubDescricao = request.PassagemAprovadaArtesp.DataPassagem.ToStringPtBr(),
                SurrogateKey = request.PassagemAprovadaArtesp.MensagemItemId.TryToLong(),
                TipoOperacaoId = request.PassagemAprovadaArtesp.Transacao.TipoOperacaoId,
                TransacaoId = 0,
                ValorD = request.PassagemAprovadaArtesp.Transacao.Valor
            };
        }

        /// <summary>
        /// Preenche o evento de primeira passagem manual.
        /// </summary>
        /// <param name="request">ProcessadorPassagemAprovadaRequest</param>
        /// <param name="passagemApoAprovadaArtespDto">PassagemAprovadaArtespDto</param>
        private void PreencherEventoPrimeiraPassagemManual(ProcessadorPassagemAprovadaRequest request,
            PassagemAprovadaArtespDto passagemApoAprovadaArtespDto)
        {
            if (request.PassagemAprovadaArtesp.PrimeiraPassagemManual)
            {
                Log.Debug($"Passagem ID: {request.PassagemAprovadaArtesp.MensagemItemId} - Fluxo: ProcessadorPassagemAprovadaHandler | Preencher Evento Primeira Passagem Manual");

                passagemApoAprovadaArtespDto.EventoPrimeiraPassagemManual = new EventoDto
                {
                    DataCriacao = DateTime.Now, //Esse DateTime.Now está sendo refeito na api de persistência.
                    IdRegistro = request.PassagemAprovadaArtesp.Adesao.Id.TryToInt(),
                    TipoEventoId = (int)TipoEvento.PrimeiraPassagemManual
                };
            }
        }

        /// <summary>
        /// Preenche as informações do aceite manual de reenvio.
        /// </summary>
        /// <param name="request">ProcessadorPassagemAprovadaRequest</param>
        /// <param name="passagemApoAprovadaArtespDto">PassagemAprovadaArtespDto</param>
        private void PreencherAceiteManualReenvioPassagemDto(ProcessadorPassagemAprovadaRequest request,
            PassagemAprovadaArtespDto passagemApoAprovadaArtespDto)
        {
            if (request.PassagemAprovadaArtesp.PossuiAceiteManualReenvioPassagem)
            {
                Log.Debug($"Passagem ID: {request.PassagemAprovadaArtesp.MensagemItemId} - Fluxo: ProcessadorPassagemAprovadaHandler | Preencher Aceite Manual Reenvio Passagem");

                passagemApoAprovadaArtespDto.AceiteManualReenvioPassagem = new AceiteManualReenvioPassagemDto
                {
                    DataProcessamento = request.PassagemAprovadaArtesp.Transacao.AceiteManualReenvioPassagem.DataProcessamento,
                    Id = request.PassagemAprovadaArtesp.Transacao.AceiteManualReenvioPassagem.Id.TryToInt(),
                    Processado = request.PassagemAprovadaArtesp.Transacao.AceiteManualReenvioPassagem.Processado
                };
            }

        }

        /// <summary>
        /// Preenche a solicitação de imagem.
        /// </summary>
        /// <param name="request">ProcessadorPassagemAprovadaRequest</param>
        /// <param name="passagemApoAprovadaArtespDto">PassagemAprovadaArtespDto</param>
        private void PreencherSolicitacaoImagemDto(ProcessadorPassagemAprovadaRequest request,
            PassagemAprovadaArtespDto passagemApoAprovadaArtespDto)
        {
            if (request.PassagemAprovadaArtesp.PossuiSolicitacaoImagem)
            {
                Log.Debug($"Passagem ID: {request.PassagemAprovadaArtesp.MensagemItemId} - Fluxo: ProcessadorPassagemAprovadaHandler | Preencher Possui Solicitação Imagem");

                passagemApoAprovadaArtespDto.SolicitacaoImagem = new SolicitacaoImagemDto
                {
                    TagId = request.PassagemAprovadaArtesp.Tag.Id.TryToInt()
                };
            }
        }

        /// <summary>
        /// Preenche a configuração de adesão.
        /// </summary>
        /// <param name="request">ProcessadorPassagemAprovadaRequest</param>
        /// <param name="passagemApoAprovadaArtespDto">PassagemAprovadaArtespDto</param>
        private void PreencherConfiguracaoAdesaoDto(ProcessadorPassagemAprovadaRequest request,
            PassagemAprovadaArtespDto passagemApoAprovadaArtespDto)
        {
            if (request.PassagemAprovadaArtesp.PossuiDivergenciaCategoriaVeiculo)
            {
                Log.Debug($"Passagem ID: {request.PassagemAprovadaArtesp.MensagemItemId} - Fluxo: ProcessadorPassagemAprovadaHandler | Preencher Configuração de Adesão");

                if (request.PassagemAprovadaArtesp.Adesao.ConfiguracaoAdesao != null &&
                    request.PassagemAprovadaArtesp.Adesao.ConfiguracaoAdesao.Id > 0)
                {
                    passagemApoAprovadaArtespDto.ConfiguracaoAdesao = new ConfiguracaoAdesaoDto
                    {
                        CategoriaId = request.PassagemAprovadaArtesp.Adesao.ConfiguracaoAdesao.Categoria.Id.TryToInt(),
                        Id = request.PassagemAprovadaArtesp.Adesao.ConfiguracaoAdesao.Id.TryToInt()
                    };
                }


            }

        }

        /// <summary>
        /// Preenche a divergência de categoria confirmada.
        /// </summary>
        /// <param name="request">ProcessadorPassagemAprovadaRequest</param>
        /// <param name="passagemApoAprovadaArtespDto">PassagemAprovadaArtespDto</param>
        private void PreencherDivergenciaCategoriaConfirmadaDto(ProcessadorPassagemAprovadaRequest request,
            PassagemAprovadaArtespDto passagemApoAprovadaArtespDto)
        {
            if (request.PassagemAprovadaArtesp.PossuiDivergenciaCategoriaVeiculo)
            {
                if (request.PassagemAprovadaArtesp.Transacao.DivergenciaCategoriaConfirmada != null)
                {
                    Log.Debug($"Passagem ID: {request.PassagemAprovadaArtesp.MensagemItemId} - Fluxo: ProcessadorPassagemAprovadaHandler | Preencher Divergência de Categoria");

                    var divergenciaCategoriaConfirmada = request.PassagemAprovadaArtesp.Transacao.DivergenciaCategoriaConfirmada;

                    passagemApoAprovadaArtespDto.DivergenciaCategoriaConfirmada = new DivergenciaCategoriaConfirmadaDto
                    {
                        CategoriaVeiculoId = divergenciaCategoriaConfirmada.CategoriaVeiculo.Id.TryToInt(),
                        Data = DateTime.Now, //Esse DateTime.Now está sendo refeito na api de persistência.
                        Id = divergenciaCategoriaConfirmada.Id.TryToInt(),
                        SurrogateKey = request.PassagemAprovadaArtesp.MensagemItemId,
                        TransacaoPassagemId = 0
                    };
                }


            }

        }

        /// <summary>
        /// Preenche informações de veículo
        /// </summary>
        /// <param name="request">ProcessadorPassagemAprovadaRequest</param>
        /// <param name="passagemApoAprovadaArtespDto">PassagemAprovadaArtespDto</param>
        private void PreencherVeiculoDto(ProcessadorPassagemAprovadaRequest request,
            PassagemAprovadaArtespDto passagemApoAprovadaArtespDto)
        {
            Log.Debug($"Passagem ID: {request.PassagemAprovadaArtesp.MensagemItemId} - Fluxo: ProcessadorPassagemAprovadaHandler | Preencher Veículo");

            passagemApoAprovadaArtespDto.Veiculo = new VeiculoDto
            {
                Id = request.PassagemAprovadaArtesp.Adesao.Veiculo.Id.TryToInt(),
                Placa = request.PassagemAprovadaArtesp.Adesao.Veiculo.Placa,
                CategoriaConfirmada = request.PassagemAprovadaArtesp.Adesao.Veiculo.CategoriaConfirmada,
                ContagemConfirmacaoCategoria = request.PassagemAprovadaArtesp.Adesao.Veiculo.ContagemConfirmacaoCategoria,
                ContagemDivergenciaCategoriaConfirmada = request.PassagemAprovadaArtesp.Adesao.Veiculo.ContagemDivergenciaCategoriaConfirmada,
                DataConfirmacaoCategoria = request.PassagemAprovadaArtesp.Adesao.Veiculo.DataConfirmacaoCategoria
            };

            if (request.PassagemAprovadaArtesp.Adesao.Veiculo.Categoria != null && (request.PassagemAprovadaArtesp.Adesao.Veiculo.Categoria.Id ?? 0) > 0)
            {
                passagemApoAprovadaArtespDto.Veiculo.CategoriaVeiculoId =
                    request.PassagemAprovadaArtesp.Adesao.Veiculo.Categoria.Id.TryToInt();
            }
        }

        /// <summary>
        /// Preenche o estorno e o extrato associado ao estorno.
        /// </summary>
        /// <param name="request">ProcessadorPassagemAprovadaRequest</param>
        /// <param name="passagemAprovadaArtespDto">PassagemAprovadaArtespDto</param>
        private void PreencherEstornoPassagemDto(ProcessadorPassagemAprovadaRequest request,
            PassagemAprovadaArtespDto passagemAprovadaArtespDto)
        {
            if (request.PassagemAprovadaArtesp.TransacaoAnterior != null)
            {
                Log.Debug($"Passagem ID: {request.PassagemAprovadaArtesp.MensagemItemId} - Fluxo: ProcessadorPassagemAprovadaHandler | Preencher Estorno de Passagem");

                var transacaoAnterior = request.PassagemAprovadaArtesp.TransacaoAnterior;

                var estornoPassagem = new EstornoPassagemDto
                {
                    AdesaoId = transacaoAnterior.AdesaoId,
                    ChaveCriptografiaBanco = string.Empty,
                    Credito = true,
                    Data = DateTime.Now, //Esse DateTime.Now está sendo refeito na api de persistência.
                    DataEstorno = request.PassagemAprovadaArtesp.DataPassagem.ToUtcDate(),
                    SaldoId = request.PassagemAprovadaArtesp.Adesao.SaldoId,
                    SomenteInformacoesAlteradas = request.PassagemAprovadaArtesp.TransacaoAnterior.Estorno.SomenteInformacoesAlteradas,
                    StatusId = (int)StatusAutorizacao.Ativa,
                    SurroGateKey = transacaoAnterior.Id.TryToLong(),
                    TipoOperacaoId = (int)TipoOperacaoMovimentoFinanceiro.EstornoPassagem,
                    TransacaoPassagemOrigemlId = transacaoAnterior.Id.TryToInt(),
                    Valor = transacaoAnterior.Valor
                };

                passagemAprovadaArtespDto.EstornoPassagem = estornoPassagem;

                var identificacaoPraca = request.PassagemAprovadaArtesp.Praca.IdentificacaoPraca;
                var razaoSocial = request.PassagemAprovadaArtesp.Conveniado.RazaoSocial;

                var sb = new StringBuilder();
                sb.Append(razaoSocial.PadLeft(' '));
                sb.Append(identificacaoPraca.PadLeft(' '));

                passagemAprovadaArtespDto.ExtratoEstorno = new ExtratoDto
                {
                    AdesaoId = transacaoAnterior.AdesaoId,
                    ChaveCriptografiaBanco = string.Empty,
                    DataTransacao = estornoPassagem.Data,
                    Descricao = sb.ToString(),
                    Placa = request.PassagemAprovadaArtesp.Placa,
                    SubDescricao = request.PassagemAprovadaArtesp.DataPassagem.ToStringPtBr(),
                    SurrogateKey = transacaoAnterior.Id.TryToLong(),
                    TipoOperacaoId = (int)TipoOperacaoMovimentoFinanceiro.EstornoPassagem,
                    TransacaoId = 0,
                    ValorD = transacaoAnterior.Valor
                };
            }

        }

        /// <summary>
        /// Preenche as viagens dos cenários de Vale Pedagio.
        /// </summary>
        /// <param name="request">ProcessadorPassagemAprovadaRequest</param>
        /// <param name="passagemAprovadaArtespDto">PassagemAprovadaArtespDto</param>
        private void PreencherViagens(ProcessadorPassagemAprovadaRequest request,
            PassagemAprovadaArtespDto passagemAprovadaArtespDto)
        {
            var viagens = request.PassagemAprovadaArtesp.Transacao.Viagens;
            if (viagens != null && viagens.Count > 0)
            {
                Log.Debug($"Passagem ID: {request.PassagemAprovadaArtesp.MensagemItemId} - Fluxo: ProcessadorPassagemAprovadaHandler | Preencher {viagens.Count} Viagens");

                passagemAprovadaArtespDto.Viagens = new List<DetalheViagemDto>();

                foreach (var detalheViagem in viagens)
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
                        dto.SurrogateKey = request.PassagemAprovadaArtesp.MensagemItemId;
                    }

                    passagemAprovadaArtespDto.Viagens.Add(dto);
                }                

            }

        }
    }
}
