using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConectCar.Framework.Infrastructure.Cqrs.Commands;
using ConectCar.Framework.Infrastructure.Log;
using ProcessadorPassagensActors.CommandQuery.Handlers.Park.Request;
using ProcessadorPassagensActors.CommandQuery.Handlers.Park.Response;
using ConectCar.Framework.Infrastructure.Data.ServiceBus.DataProviders;
using ConectCar.Transacoes.Domain.Dto;
using ProcessadorPassagensActors.CommandQuery.Util;
using AutoMapper;
using Newtonsoft.Json;
using ProcessadorPassagensActors.CommandQuery.Enums;
using ProcessadorPassagensActors.CommandQuery.Commands.Park;
using ProcessadorPassagensActors.CommandQuery.Messages.Park;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Park
{
    public class ProcessarPassagemReprovadaParkHandler : Loggable,
            ICommand<ProcessarPassagemReprovadaParkRequest, ProcessarPassagemReprovadaParkResponse>
    {
        #region [Properties]

        private readonly ServiceBusDataSourceBase _dataSource;
        private ProcessarPassagemReprovadaParkRequest _request;
        private PassagemReprovadaEstacionamentoDto _passagemReprovadaDto;

        #endregion [Properties]

        #region [Ctor]

        public ProcessarPassagemReprovadaParkHandler()
        {
            _dataSource = new ServiceBusDataSourceBase("TransacoesServiceBus", ServiceBusUtil.FactoriesCount);
        }

        #endregion [Ctor]

        public ProcessarPassagemReprovadaParkResponse Execute(ProcessarPassagemReprovadaParkRequest request)
        {
            _request = request;
            _passagemReprovadaDto = new PassagemReprovadaEstacionamentoDto();

            PreencherTransacaoEstacionamentoRecusada(); 

            EnviarParaBarramento(_request.PassagemReprovadaEstacionamento.Ticket);

            return new ProcessarPassagemReprovadaParkResponse() { Processado = true };
        }

        private void PreencherTransacaoEstacionamentoRecusada()
        {
            _passagemReprovadaDto.TransacaoEstacionamentoRecusada.Mensalista = _request.PassagemReprovadaEstacionamento.Mensalista;
            _passagemReprovadaDto.TransacaoEstacionamentoRecusada.Ticket = _request.PassagemReprovadaEstacionamento.Ticket;
            _passagemReprovadaDto.TransacaoEstacionamentoRecusada.DataReferencia = _request.PassagemReprovadaEstacionamento.TransacaoEstacionamentoRecusada.DataReferencia;
            _passagemReprovadaDto.TransacaoEstacionamentoRecusada.TipoTransacaoEstacionamentoId = _request.PassagemReprovadaEstacionamento.TransacaoEstacionamentoRecusada.TipoTransacaoEstacionamentoId;
            _passagemReprovadaDto.TransacaoEstacionamentoRecusada.MotivoAtrasoTransamissaoId = _request.PassagemReprovadaEstacionamento.TransacaoEstacionamentoRecusada.MotivoAtrasoTransamissaoId;
            _passagemReprovadaDto.TransacaoEstacionamentoRecusada.NumeroRPS = _request.PassagemReprovadaEstacionamento.TransacaoEstacionamentoRecusada.NumeroRPS;
            _passagemReprovadaDto.TransacaoEstacionamentoRecusada.SerieRPS = _request.PassagemReprovadaEstacionamento.TransacaoEstacionamentoRecusada.SerieRPS;
            _passagemReprovadaDto.TransacaoEstacionamentoRecusada.TempoPermamente = _request.PassagemReprovadaEstacionamento.TransacaoEstacionamentoRecusada.TempoPermamente;
            _passagemReprovadaDto.TransacaoEstacionamentoRecusada.MotivoRecusaId = _request.PassagemReprovadaEstacionamento.TransacaoEstacionamentoRecusada.MotivoRecusaId;
            _passagemReprovadaDto.TransacaoEstacionamentoRecusada.MotivoDesconto = _request.PassagemReprovadaEstacionamento.MotivoDesconto;
            _passagemReprovadaDto.TransacaoEstacionamentoRecusada.DataHoraEntrada = _request.PassagemReprovadaEstacionamento.DataHoraEntrada;
            _passagemReprovadaDto.TransacaoEstacionamentoRecusada.DataHoraTransacao = _request.PassagemReprovadaEstacionamento.TransacaoEstacionamentoRecusada.DataHoraTransacao;
            _passagemReprovadaDto.TransacaoEstacionamentoRecusada.PracaId = _request.PassagemReprovadaEstacionamento.TransacaoEstacionamentoRecusada.PracaId;
            _passagemReprovadaDto.TransacaoEstacionamentoRecusada.PistaId = _request.PassagemReprovadaEstacionamento.TransacaoEstacionamentoRecusada.PistaId;
            _passagemReprovadaDto.TransacaoEstacionamentoRecusada.TagId = _request.PassagemReprovadaEstacionamento.TransacaoEstacionamentoRecusada.TagId;
            _passagemReprovadaDto.TransacaoEstacionamentoRecusada.ConveniadoId = _request.PassagemReprovadaEstacionamento.TransacaoEstacionamentoRecusada.ConveniadoId;
            _passagemReprovadaDto.TransacaoEstacionamentoRecusada.Detalhes = _request.PassagemReprovadaEstacionamento.TransacaoEstacionamentoRecusada.Detalhes;
            _passagemReprovadaDto.TransacaoEstacionamentoRecusada.DataCadastro = _request.PassagemReprovadaEstacionamento.TransacaoEstacionamentoRecusada.DataCadastro;
            _passagemReprovadaDto.TransacaoEstacionamentoRecusada.ValorDesconto = _request.PassagemReprovadaEstacionamento.ValorDesconto;
            _passagemReprovadaDto.TransacaoEstacionamentoRecusada.SurrogateKey = _request.PassagemReprovadaEstacionamento.TransacaoEstacionamentoRecusada.SurrogateKey;
        }

        private void EnviarParaBarramento(string ticket)
        {
            var topicName = ServiceBusUtil.ObterNomeTopicReprovada(ProtocolosEnum.PassagensReprovadasEDI);
            var passagemCommand = new PassagemReprovadaParkTopicCommand(_dataSource, true, topicName);

            var mensagem = Mapper.Map(_passagemReprovadaDto, new PassagemReprovadaParkMessage());

            Log.Info($"Json saida Ticket - {ticket} (reprovado): {JsonConvert.SerializeObject(new List<PassagemReprovadaParkMessage> { mensagem })}");

            //Enviando para o barramento...
            passagemCommand.Execute(new List<PassagemReprovadaParkMessage> { mensagem });
            Log.Info($"Ticket: {ticket} | Detalhe Processado.");
        }
    }
}
