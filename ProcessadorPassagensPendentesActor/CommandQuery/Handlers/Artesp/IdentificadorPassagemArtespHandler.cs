using ConectCar.Framework.Infrastructure.Cqrs.Commands;
using ConectCar.Framework.Infrastructure.Log;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Responses;
using ProcessadorPassagensActors.CommandQuery.Queries;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp
{
    public class IdentificadorPassagemArtespHandler : Loggable
        , ICommand<IdentificadorPassagemRequest, IdentificadorPassagemResponse>
    {

        /// <summary>
        /// Query para identificar a passagem por mensagemitemid.
        /// </summary>
        private readonly ObterPassagemIdPorMensagemItemIdQuery _query;

        /// <summary>
        /// Construtor.
        /// </summary>
        public IdentificadorPassagemArtespHandler()
        {            
            _query = new ObterPassagemIdPorMensagemItemIdQuery();
        }

        /// <summary>
        /// Executa o processamento de Passagens
        /// </summary>
        /// <param name="request">IdentificadorPassagemRequest</param>
        /// <returns>IdentificadorPassagemResponse</returns>
        public IdentificadorPassagemResponse Execute(IdentificadorPassagemRequest request)
        {
            //TODO: analisar uso de índice na query.
            var mensagemItemId = request.PassagemPendenteArtesp.MensagemItemId;

            var passagemId = DataBaseConnection.HandleExecution(_query.Execute,mensagemItemId);

            Log.Debug($"Passagem ID: {mensagemItemId} - Fluxo: IdentificadorPassagemHandler | Identificando passagem.");

            return new IdentificadorPassagemResponse { PassagemId = passagemId };
        }
    }
}
