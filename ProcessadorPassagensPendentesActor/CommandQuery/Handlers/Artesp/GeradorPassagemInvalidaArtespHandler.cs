using ConectCar.Framework.Infrastructure.Log;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Cache;
using ProcessadorPassagensActors.CommandQuery.Enums;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Responses;
using ProcessadorPassagensActors.Infrastructure;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp
{
    public class GeradorPassagemInvalidaArtespHandler : Loggable
    {

        private int _qtdTentativas;

        public GeradorPassagemInvalidaArtespHandler()
        {
            var configuracaoQtdTentativas = ConfiguracaoSistemaCacheRepository.Obter(NomeConfiguracaoSistema.QuantidadeTentativasProcessamentoMensagemItem.GetDescription());
            _qtdTentativas = configuracaoQtdTentativas.Valor.TryToInt();
        }

        /// <summary>
        /// Executa o processamento de Passagens
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GeradorPassagemInvalidaResponse Execute(GeradorPassagemInvalidaRequest request)
        {
            var response = new GeradorPassagemInvalidaResponse();
            
            response.PassagemInvalidaArtesp = new PassagemInvalidaArtesp
            {
                MensagemItemId = request.PassagemPendenteArtesp.MensagemItemId,
                QtdTentativas = _qtdTentativas
            };

            return response;
        }
    }
}
