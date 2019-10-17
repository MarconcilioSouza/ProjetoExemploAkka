using ConectCar.Framework.Infrastructure.Log;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Responses;
using ProcessadorPassagensActors.CommandQuery.Validators.Validator;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp
{
    public class ValidadorDivergenciaCategoriaPassagemArtespHandler : Loggable
    {
        private DivergenciaCategoriaValidator _divergenciaCategoriaValidator;        

        public ValidadorDivergenciaCategoriaPassagemArtespHandler()
        {
            _divergenciaCategoriaValidator = new DivergenciaCategoriaValidator();            
        }

        /// <summary>
        /// Executa o processamento de Passagens
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ValidadorDivergenciaCategoriaPassagemResponse Execute(ValidadorDivergenciaCategoriaPassagemRequest request)
        {
            var response = new ValidadorDivergenciaCategoriaPassagemResponse();
           
            ValidarDivergenciaCategoria(request, response);            

            return response;
        }

        private void ValidarDivergenciaCategoria(ValidadorDivergenciaCategoriaPassagemRequest request, ValidadorDivergenciaCategoriaPassagemResponse response)
        {
            Log.Debug($"Passagem ID: {request.PassagemPendenteArtesp.MensagemItemId} - Fluxo: ValidadorDivergenciaCategoriaPassagemHandler | Validar Divergência");
            response.PassagemPendenteArtesp = _divergenciaCategoriaValidator.Validate(request.PassagemPendenteArtesp);
        }
    }
}
