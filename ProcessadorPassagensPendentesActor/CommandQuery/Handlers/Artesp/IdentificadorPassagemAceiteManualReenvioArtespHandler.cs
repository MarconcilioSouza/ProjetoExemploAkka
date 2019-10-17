using ConectCar.Framework.Infrastructure.Cqrs.Commands;
using ConectCar.Framework.Infrastructure.Log;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Responses;
using ProcessadorPassagensActors.CommandQuery.Queries;
using ProcessadorPassagensActors.CommandQuery.Queries.Filter;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp
{
    /// <summary>
    /// Responsável por verificar se uma passagem pendente Artesp possui Aceite Manual de Reenvio.
    /// </summary>
    public class IdentificadorPassagemAceiteManualReenvioArtespHandler : Loggable,
        ICommand<IdentificadorPassagemAceiteManualReenvioRequest, IdentificadorPassagemAceiteManualReenvioResponse>
             
    {
        /// <summary>
        /// Identifica a existência do aceite manual de reenvio.
        /// </summary>
        private ObterAceiteManualReenvioIdPorPassagemNaoProcessadoQuery _aceiteManualReenvioIdPorPassagemNaoProcessadoQuery;

        /// <summary>
        /// Construtor
        /// </summary>
        public IdentificadorPassagemAceiteManualReenvioArtespHandler()
        {
            _aceiteManualReenvioIdPorPassagemNaoProcessadoQuery = new ObterAceiteManualReenvioIdPorPassagemNaoProcessadoQuery();
        }        

        /// <summary>
        /// Executa o processamento de Passagens
        /// </summary>
        /// <param name="request">IdentificadorPassagemRequest</param>
        /// <returns></returns>
        public IdentificadorPassagemAceiteManualReenvioResponse Execute(IdentificadorPassagemAceiteManualReenvioRequest request)
        {            
            var mensagemItemId = request.PassagemPendenteArtesp.MensagemItemId;
            var passagemPendenteArtesp = request.PassagemPendenteArtesp;

            Log.Debug($"Passagem ID: {mensagemItemId} - Fluxo: IdentificadorPassagemAceiteManualReenvioArtespHandler | Identificando passagem que possui aceite manual no reenvio.");

            if (passagemPendenteArtesp.NumeroReenvio > 0)
            {                
                var filter = new AceiteManualReenvioPassagemPorPassagemNaoProcessadoFilter(passagemPendenteArtesp.ConveniadoPassagemId, passagemPendenteArtesp.Conveniado.CodigoProtocoloArtesp);
                var aceiteManualReenvioId = DataBaseConnection.HandleExecution(_aceiteManualReenvioIdPorPassagemNaoProcessadoQuery.Execute,filter);
                passagemPendenteArtesp.PossuiAceiteManualReenvioPassagem = aceiteManualReenvioId > 0;
            }
            else
            {
                passagemPendenteArtesp.PossuiAceiteManualReenvioPassagem = false;
            }            

            return new IdentificadorPassagemAceiteManualReenvioResponse { PassagemPendenteArtesp = passagemPendenteArtesp };

        }
    }
}
