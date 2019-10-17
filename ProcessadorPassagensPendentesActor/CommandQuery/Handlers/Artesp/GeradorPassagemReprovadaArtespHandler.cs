using System;
using AutoMapper;
using ConectCar.Framework.Infrastructure.Log;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.Model;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Responses;
using ProcessadorPassagensActors.CommandQuery.Queries;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp
{
    public class GeradorPassagemReprovadaArtespHandler : Loggable
    {

        private ObterParceiroNegocioIdPorCodigoQuery _parceiroNegocioIdPorCodigoQuery;
        private PassagemReprovadaArtesp _passagemReprovadaArtesp;
        private int _parceiroId;
        private GeradorPassagemArtespHandler _geradorPassagemArtespHandler;

        public GeradorPassagemReprovadaArtespHandler()
        {
            _parceiroNegocioIdPorCodigoQuery = new ObterParceiroNegocioIdPorCodigoQuery();
            _parceiroId = DataBaseConnection.HandleExecution(_parceiroNegocioIdPorCodigoQuery.Execute, "ROAD");
            _geradorPassagemArtespHandler = new GeradorPassagemArtespHandler();
        }

        /// <summary>
        /// Executa o processamento de Passagens pendentes reprovadas
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GeradorPassagemReprovadaResponse Execute(GeradorPassagemPendenteReprovadaRequest request)
        {
            _geradorPassagemArtespHandler.CarregarPassagemPendenteArtesp(request.PassagemPendenteArtesp); // carregamos as informações necessárias para se criar uma passagem

            PreencherPassagemETransacaoRecusada(request.PassagemPendenteArtesp, request.MotivoNaoCompensado);
            return new GeradorPassagemReprovadaResponse { PassagemReprovadaArtesp = _passagemReprovadaArtesp };
        }

        /// <summary>
        /// Executa o processamento de Passagens reprovadas
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GeradorPassagemReprovadaResponse Execute(GeradorPassagemReprovadaRequest request)
        {
            PreencherPassagemETransacaoRecusada(request.PassagemPendenteArtesp, request.MotivoNaoCompensado);
            return new GeradorPassagemReprovadaResponse { PassagemReprovadaArtesp = _passagemReprovadaArtesp };
        }

        /// <summary>
        /// Executa o processamento de Passagens reprovadas por Divergencia de categoria
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GeradorPassagemReprovadaResponse Execute(GeradorPassagemReprovadaDivergenciaCategoriaRequest request)
        {
            PreencherPassagemETransacaoRecusada(request.PassagemPendenteArtesp, request.MotivoNaoCompensado);
            return new GeradorPassagemReprovadaResponse { PassagemReprovadaArtesp = _passagemReprovadaArtesp };
        }

        /// <summary>
        /// Executa o processamento de Passagens reprovadas por Transacao Parceiro
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GeradorPassagemReprovadaResponse Execute(GeradorPassagemReprovadaTransacaParceiroArtespRequest request)
        {
            PreencherPassagemETransacaoRecusada(request.PassagemPendenteArtesp, request.MotivoNaoCompensado);

            _passagemReprovadaArtesp.TransacaoRecusadaParceiro = new TransacaoRecusadaParceiroArtesp
            {
                DataEnvioAoParceiro = null,
                DataPassagemNaPraca = request.PassagemPendenteArtesp.DataPassagem,
                PassagemId = 0,
                MotivoNaoCompensado = _passagemReprovadaArtesp.MotivoNaoCompensado,
                ParceiroId = _parceiroId,
                Valor = request.PassagemPendenteArtesp.Valor,
                ViagemAgendada = new DetalheViagem
                {
                    Id = request.DetalheViagemId
                }
            };
            return new GeradorPassagemReprovadaResponse { PassagemReprovadaArtesp = _passagemReprovadaArtesp };
        }

        /// <summary>
        /// Executa o processamento de Passagens pendentes reprovadas por já existir uma TransacaoReprovada 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GeradorPassagemReprovadaResponse Execute(GeradorPassagemPendenteReprovadaTransacaoReprovadaExistenteRequest request)
        {
            PreencherPassagemETransacaoRecusada(request.PassagemPendenteArtesp, request.TransacaoRecusada.MotivoRecusado);
            _passagemReprovadaArtesp.MotivoNaoCompensado = request.TransacaoRecusada.MotivoRecusado;
            _passagemReprovadaArtesp.TransacaoRecusada = request.TransacaoRecusada;

            var response = new GeradorPassagemReprovadaResponse
            {
                PassagemReprovadaArtesp = _passagemReprovadaArtesp
            };
            return response;
        }


        private void PreencherPassagemETransacaoRecusada(PassagemPendenteArtesp passagemPendenteArtesp, MotivoNaoCompensado motivo)
        {
            _passagemReprovadaArtesp = new PassagemReprovadaArtesp();
            Mapper.Map(passagemPendenteArtesp, _passagemReprovadaArtesp);

            _passagemReprovadaArtesp.TransacaoRecusada = new TransacaoRecusada
            {
                DataProcessamento = DateTime.Now,
                MotivoRecusado = motivo
            };

            _passagemReprovadaArtesp.MotivoNaoCompensado = motivo;
        }




    }
}
