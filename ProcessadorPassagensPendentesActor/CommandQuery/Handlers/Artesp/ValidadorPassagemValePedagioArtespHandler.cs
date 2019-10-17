using ConectCar.Framework.Infrastructure.Log;
using ConectCar.Transacoes.Domain.Enum;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Responses;
using ProcessadorPassagensActors.CommandQuery.Validators.Validator;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp
{
    public class ValidadorPassagemValePedagioArtespHandler : Loggable
    {

        private PassagemValePedagioValidator _passagemValePedagioValidator;

        public ValidadorPassagemValePedagioArtespHandler()
        {
            _passagemValePedagioValidator = new PassagemValePedagioValidator();
        }

        /// <summary>
        /// Executa o processamento de Passagens
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ValidadorPassagemValePedagioResponse Execute(ValidadorPassagemValePedagioRequest request)
        {
            var response = new ValidadorPassagemValePedagioResponse
            {
                PassagemPendenteArtesp = request.PassagemPendenteArtesp
            };

            if (!request.PassagemPendenteArtesp.PossuiAceiteManualReenvioPassagem || request.PassagemPendenteArtesp.MotivoSemValor == MotivoSemValor.CobrancaIndevida)
            {
                
                var retorno = _passagemValePedagioValidator.Validate(request.PassagemPendenteArtesp);
                response.ViagensAgendadas = retorno.ViagensParaRetorno;
                response.MotivoNaoCompensado = retorno.MotivoNaoCompensado;
                response.ViagemNaoCompensadaId = retorno.ViagemNaoCompensadaId;
            }

            return response;
        }
    }
}
