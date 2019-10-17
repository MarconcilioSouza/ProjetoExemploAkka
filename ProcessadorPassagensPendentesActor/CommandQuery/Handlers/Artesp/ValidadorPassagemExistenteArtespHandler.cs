using ConectCar.Framework.Infrastructure.Cqrs.Commands;
using ConectCar.Framework.Infrastructure.Log;
using ProcessadorPassagensActors.CommandQuery.Connections;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Responses;
using ProcessadorPassagensActors.CommandQuery.Queries;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp
{
    public class ValidadorPassagemExistenteArtespHandler : Loggable,
        ICommand<ValidadorPassagemExistenteRequest, ValidadorPassagemExistenteResponse>

    {
        /// <summary>
        /// Obtem a transação recusada através do identificador da passagem.
        /// </summary>
        private ObterTransacaoRecusadaPorPassagemIdQuery _transacaoRecusadaPorPassagemIdQuery;

        /// <summary>
        /// Obtem a transação passagem através do identificador da passagem.
        /// </summary>
        private ObterTransacaoPassagemPorPassagemIdQuery _transacaoPassagemPorPassagemIdQuery;


        /// <summary>
        /// Construtor.
        /// </summary>
        public ValidadorPassagemExistenteArtespHandler()
        {
            _transacaoRecusadaPorPassagemIdQuery = new ObterTransacaoRecusadaPorPassagemIdQuery();
            _transacaoPassagemPorPassagemIdQuery = new ObterTransacaoPassagemPorPassagemIdQuery();
        }


        /// <summary>
        /// Executa o processamento de Passagens
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ValidadorPassagemExistenteResponse Execute(ValidadorPassagemExistenteRequest request)
        {
            Log.Debug($"Passagem ID: {request.PassagemPendenteArtesp.MensagemItemId} - Fluxo: ValidadorPassagemExistenteHandler | Validando passagem existente.");

            var transacaoRecusada = DataBaseConnection.HandleExecution(_transacaoRecusadaPorPassagemIdQuery.Execute, request.PassagemId);
            if (transacaoRecusada != null && transacaoRecusada.Id > 0)
                return new ValidadorPassagemExistenteTransacaoRecusadaResponse
                {
                    PassagemPendenteArtesp = request.PassagemPendenteArtesp,
                    TransacaoRecusada = transacaoRecusada
                };

            var transacaoPassagemDto = DataBaseConnection.HandleExecution(_transacaoPassagemPorPassagemIdQuery.Execute, request.PassagemId);
            if (transacaoPassagemDto != null && transacaoPassagemDto.TransacaoId > 0)
                return new ValidadorPassagemExistenteTransacaoPassagemResponse
                {
                    PassagemPendenteArtesp = request.PassagemPendenteArtesp,
                    CodigoProtocoloArtesp = request.PassagemPendenteArtesp.Conveniado.CodigoProtocoloArtesp,
                    MensagemItemId = request.PassagemPendenteArtesp.MensagemItemId,
                    TransacaoId = transacaoPassagemDto.TransacaoId
                };

            var response = new ValidadorPassagemExistenteResponse { PassagemPendenteArtesp = request.PassagemPendenteArtesp };
            return response;
        }
    }
}
