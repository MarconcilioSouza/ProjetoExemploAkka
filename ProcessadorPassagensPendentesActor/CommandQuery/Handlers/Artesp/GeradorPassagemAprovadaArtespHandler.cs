using System;
using ConectCar.Transacoes.Domain.Model;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Bo;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Responses;
using ProcessadorPassagensActors.CommandQuery.Queries;
using AutoMapper;
using ProcessadorPassagensActors.CommandQuery.Queries.Filter;
using ConectCar.Framework.Infrastructure.Log;
using ProcessadorPassagensActors.CommandQuery.Connections;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp
{
    public class GeradorPassagemAprovadaArtespHandler : Loggable
    {
        private ObterAceiteManualReenvioIdPorPassagemNaoProcessadoQuery _aceiteManualReenvioIdPorPassagemNaoProcessadoQuery;
        private CalcularRepasseBo _calcularRepasseBo;
        private EstornoBo _estornoBo;
        private ObterPassagemProcessadaQuery _passagemProcessadaQuery;

        public GeradorPassagemAprovadaArtespHandler()
        {
            _aceiteManualReenvioIdPorPassagemNaoProcessadoQuery = new ObterAceiteManualReenvioIdPorPassagemNaoProcessadoQuery();
            _calcularRepasseBo = new CalcularRepasseBo();
            _estornoBo = new EstornoBo();
            _passagemProcessadaQuery = new ObterPassagemProcessadaQuery();
         }

        /// <summary>
        /// Executa o processamento de Passagens
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GeradorPassagemAprovadaResponse Execute(GeradorPassagemAprovadaRequest request)
        {
            var passagemAprovada = Mapper.Map<PassagemAprovadaArtesp>(request.PassagemPendenteArtesp);

            passagemAprovada.Transacao = new TransacaoPassagemArtesp
            {
                Id = 0,
                AdesaoId = (int)(request.PassagemPendenteArtesp.Adesao.Id ?? 0),
                CategoriaUtilizadaId = (int)(request.PassagemPendenteArtesp.CategoriaUtilizada.Id ?? 0),
                Data = DateTime.Now,
                DataDePassagem = request.PassagemPendenteArtesp.DataPassagem,
                DataRepasse = DateTime.Now,
                PistaId = (int)(request.PassagemPendenteArtesp.Pista.Id ?? 0),
                Valor = request.PassagemPendenteArtesp.Valor,
                StatusId = 1,
                OrigemTransacaoId = 1, // Mensagem
                TipoOperacaoId = 7,
                RepasseId = 0, // será preenchido no calculo de repasse
                ValorRepasse = 0, // será preenchido no calculo de repasse
                TarifaDeInterconexao = 0, // será preenchido no calculo de repasse
                ItemListaDeParaUtilizado = request.PassagemPendenteArtesp.ItemListaDeParaUtilizado,
                Credito = false, // **verificar**

                Viagens = request.ViagensAgendadas
            };


            if (request.PassagemPendenteArtesp.Adesao.ConfiguracaoAdesao != null &&
                request.PassagemPendenteArtesp.Adesao.ConfiguracaoAdesao?.Id > 0)
                passagemAprovada.Adesao.ConfiguracaoAdesao = request.PassagemPendenteArtesp.Adesao.ConfiguracaoAdesao;


            if (passagemAprovada.PossuiDivergenciaCategoriaVeiculo)
            {
                passagemAprovada.Transacao.DivergenciaCategoriaConfirmada = new DivergenciaCategoriaConfirmada
                {
                    CategoriaVeiculo = passagemAprovada.CategoriaUtilizada
                };
            }

            if (passagemAprovada.PossuiAceiteManualReenvioPassagem)
            {                
                var filter = new AceiteManualReenvioPassagemPorPassagemNaoProcessadoFilter(request.PassagemPendenteArtesp.ConveniadoPassagemId, request.PassagemPendenteArtesp.Conveniado.CodigoProtocoloArtesp);
                var aceiteManualId = DataBaseConnection.HandleExecution(_aceiteManualReenvioIdPorPassagemNaoProcessadoQuery.Execute, filter);
                passagemAprovada.Transacao.AceiteManualReenvioPassagem = new AceiteManualReenvioPassagem
                {
                    DataProcessamento = request.PassagemPendenteArtesp.DataPassagem,
                    Processado = true,
                    Id = aceiteManualId
                };
            }

            Log.Debug($"Passagem ID: {passagemAprovada.MensagemItemId} - Fluxo: GeradorPassagemAprovadaHandler | Calcular Repasse");            
            _calcularRepasseBo.Calular(passagemAprovada);

           
            if (request.PassagemPendenteArtesp.TransacaoPassagemIdAnterior > 0)
            {                
                passagemAprovada.TransacaoAnterior = _estornoBo.ValidarEstornoSeNecessario(request.PassagemPendenteArtesp, request.ViagensAgendadas);
            }

            var response = new GeradorPassagemAprovadaResponse
            {
                PassagemAprovadaArtesp = passagemAprovada,
                CodigoProtocoloArtesp = request.PassagemPendenteArtesp.Conveniado.CodigoProtocoloArtesp
            };           

            return response;
        }

        /// <summary>
        /// Execute responsável por carregar a passagem processada do banco de dados, uma vez que elá já se encontra salva.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GeradorPassagemAprovadaResponse Execute(GeradorPassagemAprovadaComTransacaoExistenteRequest request)
        {
            var passagemAprovada = DataBaseConnection.HandleExecution(_passagemProcessadaQuery.Execute, request.TransacaoId);

            var response = new GeradorPassagemAprovadaResponse
            {
                PassagemAprovadaArtesp = passagemAprovada,
                CodigoProtocoloArtesp = request.CodigoProtocoloArtesp
            };

            Log.Debug($"Passagem ID: {passagemAprovada.MensagemItemId} - Fluxo: GeradorPassagemAprovadaHandler | Gerando passagem Processada.");

            return response;

        }



    }
}
