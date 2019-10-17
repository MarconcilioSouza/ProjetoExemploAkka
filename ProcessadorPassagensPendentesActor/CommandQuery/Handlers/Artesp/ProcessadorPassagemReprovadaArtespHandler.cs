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
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.Infrastructure;
using ProcessadorPassagensActors.Infrastructure.Util;
using ProcessadorPassagensActors.CommandQuery.Enums;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp
{
    public class ProcessadorPassagemReprovadaArtespHandler : Loggable
    {
        private readonly ServiceBusDataSourceBase _dataSource;
        private PassagemReprovadaArtespTopicCommand _passagemReprovadaArtespTopicCommand;

        public ProcessadorPassagemReprovadaArtespHandler()
        {
            _dataSource = new ServiceBusDataSourceBase("TransacoesServiceBus", ServiceBusUtil.FactoriesCount, 100, 2);
            var topicName = ServiceBusUtil.ObterNomeTopicReprovada(EnumInfra.ProtocolosEnum.PassagensReprovadasArtesp);
            _passagemReprovadaArtespTopicCommand = new PassagemReprovadaArtespTopicCommand(_dataSource, true, topicName);
        }

        /// <summary>
        /// Envia passagens reprovadas para o barramento...
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public void Execute(ProcessadorPassagemReprovadaRequest request)
        {
            var passagemReprovadaDto = new PassagemReprovadaArtespDto();

            ConverterMotivoNaoCompensado(request.PassagemReprovadaArtesp);

            if (request.PassagemReprovadaArtesp.TransacaoRecusada.Id > 0)
            {
                CarregarPassagemProcessada(request, passagemReprovadaDto);                
                EnviarParaBarramento(passagemReprovadaDto, request.PassagemReprovadaArtesp.MensagemItemId);
            }
            else
            {
                CarregarPassagemProcessada(request, passagemReprovadaDto);

                CarregarPassagem(request, passagemReprovadaDto);

                CarregarTransacaoRecusada(request, passagemReprovadaDto);

                CarregarTransacaoRecusadaParceiro(request, passagemReprovadaDto);

                CarregarVeiculo(request, passagemReprovadaDto);                            

                EnviarParaBarramento(passagemReprovadaDto, request.PassagemReprovadaArtesp.MensagemItemId);                
            }
        }

        private void CarregarTransacaoRecusadaParceiro(ProcessadorPassagemReprovadaRequest request,
            PassagemReprovadaArtespDto passagemReprovadaDto)
        {
            if (request.PassagemReprovadaArtesp.TransacaoRecusadaParceiro != null)
            {
                Log.Debug($"Passagem ID: {request.PassagemReprovadaArtesp.MensagemItemId} - Fluxo: ProcessadorPassagemReprovadaHandler | Preencher Transação Recusada Parceiro");

                passagemReprovadaDto.TransacaoRecusadaParceiro = new TransacaoRecusadaParceiroDto
                {
                    DataEnvioAoParceiro = request.PassagemReprovadaArtesp.TransacaoRecusadaParceiro.DataEnvioAoParceiro,
                    DataPassagemNaPraca = request.PassagemReprovadaArtesp.TransacaoRecusadaParceiro.DataPassagemNaPraca,
                    ParceiroId = request.PassagemReprovadaArtesp.TransacaoRecusadaParceiro.ParceiroId,
                    Id = request.PassagemReprovadaArtesp.TransacaoRecusada.PassagemId,
                    Valor = request.PassagemReprovadaArtesp.TransacaoRecusadaParceiro.Valor,
                    ViagemAgendadaId = (request.PassagemReprovadaArtesp.TransacaoRecusadaParceiro?.ViagemAgendada?.Id ?? 0).TryToInt(),
                    SurrogateKey = request.PassagemReprovadaArtesp.MensagemItemId
                };
            }
        }

        private void CarregarTransacaoRecusada(ProcessadorPassagemReprovadaRequest request,
            PassagemReprovadaArtespDto passagemReprovadaDto)
        {
            Log.Debug($"Passagem ID: {request.PassagemReprovadaArtesp.MensagemItemId} - Fluxo: ProcessadorPassagemReprovadaHandler | Preencher Transação Recusada");

            passagemReprovadaDto.TransacaoRecusada = new TransacaoRecusadaDto
            {
                DataProcessamento = DateTime.Now, //Esse DateTime.Now está sendo refeito na api de persistência.
                MotivoRecusadoId = (int)request.PassagemReprovadaArtesp.MotivoNaoCompensado,
                SurrogateKey = request.PassagemReprovadaArtesp.MensagemItemId
            };
        }

        private void CarregarPassagem(ProcessadorPassagemReprovadaRequest request,
            PassagemReprovadaArtespDto passagemReprovadaDto)
        {
            Log.Debug($"Passagem ID: {request.PassagemReprovadaArtesp.MensagemItemId} - Fluxo: ProcessadorPassagemReprovadaHandler | Preencher Passagem");

            passagemReprovadaDto.Passagem = new PassagemDto
            {
                AdesaoId = request.PassagemReprovadaArtesp.Adesao.Id.TryToInt(),
                CategoriaDetectadaId = request.PassagemReprovadaArtesp.CategoriaDetectada.Id.TryToInt(),
                CategoriaCobradaId = request.PassagemReprovadaArtesp.CategoriaCobrada.Id.TryToInt(),
                CategoriaTagId = request.PassagemReprovadaArtesp.CategoriaTag.Id.TryToInt(),
                CodigoPassagemConveniado = request.PassagemReprovadaArtesp.ConveniadoPassagemId,
                CodigoPista = request.PassagemReprovadaArtesp.Pista.CodigoPista.TryToInt(),
                CodigoPraca = request.PassagemReprovadaArtesp.Praca.CodigoPraca.TryToInt(),
                ConveniadoId = request.PassagemReprovadaArtesp.Conveniado.Id.TryToInt(),
                Data = request.PassagemReprovadaArtesp.DataPassagem.ToUtcDate(),
                PassagemRecusadaMensageria = request.PassagemReprovadaArtesp.PassagemRecusadaMensageria ?? false,
                Placa = request.PassagemReprovadaArtesp.Placa,
                MensagemItemId = request.PassagemReprovadaArtesp.MensagemItemId,
                Reenvio = request.PassagemReprovadaArtesp.NumeroReenvio,
                TagId = request.PassagemReprovadaArtesp.Tag.Id.TryToInt(),
                TagBloqueadaMomentoRecebimento = request.PassagemReprovadaArtesp.TagBloqueadaMomentoRecebimento ?? false,
                Valor = request.PassagemReprovadaArtesp.Valor,
                StatusCobrancaId = (int)request.PassagemReprovadaArtesp.StatusCobranca,
                MotivoSemvalorId = (int)request.PassagemReprovadaArtesp.MotivoSemValor,
                StatusPassagemId = (int)request.PassagemReprovadaArtesp.StatusPassagem,
                SurrogateKey = request.PassagemReprovadaArtesp.MensagemItemId
            };
        }

        private void CarregarPassagemProcessada(ProcessadorPassagemReprovadaRequest request,
            PassagemReprovadaArtespDto passagemReprovadaDto)
        {
            Log.Debug($"Passagem ID: {request.PassagemReprovadaArtesp.MensagemItemId} - Fluxo: ProcessadorPassagemReprovadaHandler | Preencher Passagem Processada");

            passagemReprovadaDto.PassagemProcessada = new PassagemProcessadaArtespDto
            {
                MotivoNaoCompensado = (int)request.PassagemReprovadaArtesp.MotivoNaoCompensado,
                MensagemId = request.PassagemReprovadaArtesp.Mensagem.Id ?? 0,
                MensagemItemId = request.PassagemReprovadaArtesp.MensagemItemId,
                Reenvio = request.PassagemReprovadaArtesp.NumeroReenvio,
                Resultado = (int)ResultadoPassagem.NaoCompensado,
                ValePedagio = request.PassagemReprovadaArtesp.ValePedagio,
                DataPagamento = null,
                MotivoOutroValor = null,
                MotivoProvisionado = null,
                PassagemId = request.PassagemReprovadaArtesp.TransacaoRecusada.PassagemId ?? 0,
                Valor = null,
                TransacaoId = null

            };


        }

        private void CarregarVeiculo(ProcessadorPassagemReprovadaRequest request,
            PassagemReprovadaArtespDto passagemReprovadaDto)
        {

            if (request.PassagemReprovadaArtesp.Adesao.Veiculo != null && request.PassagemReprovadaArtesp.Adesao.Veiculo.Id > 0)
            {
                Log.Debug($"Passagem ID: {request.PassagemReprovadaArtesp.MensagemItemId} - Fluxo: ProcessadorPassagemReprovadaHandler | Preencher Veículo");

                passagemReprovadaDto.Veiculo = new VeiculoDto
                {
                    Id = request.PassagemReprovadaArtesp.Adesao.Veiculo.Id.TryToInt(),
                    CategoriaVeiculoId = request.PassagemReprovadaArtesp.Adesao.Veiculo.Categoria.Id.TryToInt(),
                    Placa = request.PassagemReprovadaArtesp.Adesao.Veiculo.Placa,
                    CategoriaConfirmada = request.PassagemReprovadaArtesp.Adesao.Veiculo.CategoriaConfirmada,
                    ContagemConfirmacaoCategoria = request.PassagemReprovadaArtesp.Adesao.Veiculo.ContagemConfirmacaoCategoria,
                    ContagemDivergenciaCategoriaConfirmada = request.PassagemReprovadaArtesp.Adesao.Veiculo.ContagemDivergenciaCategoriaConfirmada,
                    DataConfirmacaoCategoria = request.PassagemReprovadaArtesp.Adesao.Veiculo.DataConfirmacaoCategoria
                };

                if (request.PassagemReprovadaArtesp.Adesao.Veiculo.Categoria != null && (request.PassagemReprovadaArtesp.Adesao.Veiculo.Categoria.Id ?? 0) > 0)
                {
                    passagemReprovadaDto.Veiculo.CategoriaVeiculoId =
                        request.PassagemReprovadaArtesp.Adesao.Veiculo.Categoria.Id.TryToInt();
                }
            }
        }


        private void EnviarParaBarramento(PassagemReprovadaArtespDto passagemReprovadaArtespDto, long mensagemItemId)
        {            
            var mensagem = Mapper.Map(passagemReprovadaArtespDto, new PassagemReprovadaMessage());

            //Enviando para o barramento...
            Log.Info($"Passagem ID: {mensagemItemId} | Passagem Reprovada.");
            _passagemReprovadaArtespTopicCommand.Execute(new List<PassagemReprovadaMessage> { mensagem }, (p) =>
            {
                var properties = new Dictionary<string, object>();
                properties.Add("MensagemItemId", p.PassagemProcessada.MensagemItemId);
                return properties;
            });
            Log.Info($"Passagem ID: {mensagemItemId} | Passagem Reprovada - Barramento.");

        }

        private void ConverterMotivoNaoCompensado(PassagemReprovadaArtesp passagemReprovadaArtesp)
        {
            Log.Debug($"Passagem ID: {passagemReprovadaArtesp.MensagemItemId} - Fluxo: ProcessadorPassagemReprovadaHandler | Converter Motivo Nao Compensado");

            switch (passagemReprovadaArtesp.MotivoNaoCompensado)
            {
                case MotivoNaoCompensado.SemNumeroDeReenvio:
                case MotivoNaoCompensado.AdesaoInvalida:
                case MotivoNaoCompensado.EmissorDeTagInvalido:
                case MotivoNaoCompensado.PassagemRecusadaMensageria:
                case MotivoNaoCompensado.TransacaoPassagemInvalido:
                case MotivoNaoCompensado.ValorNaoCorrespondenteCAT:

                    passagemReprovadaArtesp.MotivoNaoCompensado = MotivoNaoCompensado.DadosInvalidos;
                    break;
            }
        }
    }
}
