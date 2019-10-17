using AutoMapper;
using ConectCar.Framework.Infrastructure.Cqrs.Commands;
using ConectCar.Framework.Infrastructure.Data.ServiceBus.DataProviders;
using ConectCar.Framework.Infrastructure.Log;
using ConectCar.Transacoes.Domain.Dto;
using Newtonsoft.Json;
using ProcessadorPassagensActors.CommandQuery.Commands.Edi;
using ProcessadorPassagensActors.CommandQuery.Enums;
using ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Request;
using ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Responses;
using ProcessadorPassagensActors.CommandQuery.Messages.Edi;
using ProcessadorPassagensActors.CommandQuery.Util;
using System.Collections.Generic;
using ProcessadorPassagensActors.Infrastructure;
using ProcessadorPassagensActors.Infrastructure.Util;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Edi
{
    public class ProcessadorPassagemReprovadaEdiHandler : Loggable,
            ICommand<ProcessadorPassagemReprovadaEdiRequest, ProcessadorPassagemReprovadaEdiResponse>
    {
        #region [Properties]

        private readonly ServiceBusDataSourceBase _dataSource;
        private ProcessadorPassagemReprovadaEdiRequest _request;
        private PassagemReprovadaEdiDto _passagemReprovadaEdiDto;

        #endregion [Properties]

        #region [Ctor]

        public ProcessadorPassagemReprovadaEdiHandler()
        {
            _dataSource = new ServiceBusDataSourceBase("TransacoesServiceBus", ServiceBusUtil.FactoriesCount);
        }

        #endregion [Ctor]

        /// <summary>
        /// Envia passagens reprovadas para o barramento...
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ProcessadorPassagemReprovadaEdiResponse Execute(ProcessadorPassagemReprovadaEdiRequest request)
        {
            _request = request;
            _passagemReprovadaEdiDto = new PassagemReprovadaEdiDto();

            PreencherDetalheTrfRecusado();
            PreencherTransacaoRecusadaParceiro();
            PreencherVeiculo();

            EnviarParaBarramento(_request.PassagemReprovadaEDI.DetalheTrnId);

            return new ProcessadorPassagemReprovadaEdiResponse() { Processado = true };
        }

        private void PreencherDetalheTrfRecusado()
        {
            _passagemReprovadaEdiDto.DetalheTRFRecusado = new DetalheTRFRecusadoDto
            {
                ArquivoTRFId = _request.PassagemReprovadaEDI.ArquivoTrfId.TryToInt(),
                CodigoRetornoId = (int)_request.PassagemReprovadaEDI.DetalheTRFRecusado.CodigoRetorno,
                SurrogateKey = _request.PassagemReprovadaEDI.DetalheTRFRecusado.DetalheTRNId,
                DetalheTRNId = _request.PassagemReprovadaEDI.DetalheTRFRecusado.DetalheTRNId,
                Id = _request.PassagemReprovadaEDI.DetalheTRFRecusado.Id.TryToInt(),
            };
        }

        private void PreencherTransacaoRecusadaParceiro()
        {
            if (_request.PassagemReprovadaEDI.TransacaoRecusadaParceiro != null)
            {
                _passagemReprovadaEdiDto.TransacaoRecusadaParceiro = new TransacaoRecusadaParceiroEdiDto
                {
                    DataEnvioAoParceiro = _request.PassagemReprovadaEDI.TransacaoRecusadaParceiro.DataEnvioAoParceiro,
                    DataPassagemNaPraca = _request.PassagemReprovadaEDI.TransacaoRecusadaParceiro.DataPassagemNaPraca,
                    ParceiroId = _request.PassagemReprovadaEDI.TransacaoRecusadaParceiro.ParceiroId,
                    Id = _request.PassagemReprovadaEDI.TransacaoRecusadaParceiro.Id.TryToInt(),
                    Valor = _request.PassagemReprovadaEDI.TransacaoRecusadaParceiro.Valor,
                    ViagemAgendadaId = _request.PassagemReprovadaEDI.TransacaoRecusadaParceiro.ViagemAgendada.Id.TryToInt(),
                    SurrogateKey = _request.PassagemReprovadaEDI.DetalheTrnId,
                    DetalheTRNId = _request.PassagemReprovadaEDI.DetalheTrnId,
                    CodigoRetornoTRFId = _request.PassagemReprovadaEDI.CodigoRetorno.TryToInt(),
                };
            }
        }

        private void PreencherVeiculo()
        {
            if (_request.PassagemReprovadaEDI.Veiculo != null)
                _passagemReprovadaEdiDto.Veiculo = new VeiculoDto
                {
                    Id = _request.PassagemReprovadaEDI.Adesao.Veiculo.Id.TryToInt(),
                    CategoriaVeiculoId = _request.PassagemReprovadaEDI.Adesao.Veiculo.Categoria.Id.TryToInt(),
                    Placa = _request.PassagemReprovadaEDI.Adesao.Veiculo.Placa,
                    CategoriaConfirmada = _request.PassagemReprovadaEDI.Adesao.Veiculo.CategoriaConfirmada,
                    ContagemConfirmacaoCategoria = _request.PassagemReprovadaEDI.Adesao.Veiculo.ContagemConfirmacaoCategoria,
                    ContagemDivergenciaCategoriaConfirmada = _request.PassagemReprovadaEDI.Adesao.Veiculo.ContagemDivergenciaCategoriaConfirmada,
                    DataConfirmacaoCategoria = _request.PassagemReprovadaEDI.Adesao.Veiculo.DataConfirmacaoCategoria,
                };
        }

        private void EnviarParaBarramento(long detalheTrnId)
        {
            var topicName = ServiceBusUtil.ObterNomeTopicReprovada(EnumInfra.ProtocolosEnum.PassagensReprovadasEDI);
            var passagemCommand = new PassagemReprovadaEDITopicCommand(_dataSource, true, topicName);

            var mensagem = Mapper.Map(_passagemReprovadaEdiDto, new PassagemReprovadaEDIMessage());

            Log.Info($"Json saida TRN - {detalheTrnId} (reprovado): {JsonConvert.SerializeObject(new List<PassagemReprovadaEDIMessage> { mensagem })}");

            //Enviando para o barramento...
            passagemCommand.Execute(new List<PassagemReprovadaEDIMessage> { mensagem });
            Log.Info($"DetalheTrn ID: {detalheTrnId} | Detalhe Processado.");
        }
    }
}