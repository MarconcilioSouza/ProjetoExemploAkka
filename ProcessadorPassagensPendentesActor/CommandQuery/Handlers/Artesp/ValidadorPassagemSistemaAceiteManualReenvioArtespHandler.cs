using ConectCar.Framework.Infrastructure.Cqrs.Commands;
using ConectCar.Framework.Infrastructure.Cqrs.Handlers;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Framework.Infrastructure.Data.Cache.DataProviders;
using ConectCar.Framework.Infrastructure.Log;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Cache;
using ProcessadorPassagensActors.CommandQuery.Enums;
using ProcessadorPassagensActors.CommandQuery.Exceptions;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Responses;
using ProcessadorPassagensActors.CommandQuery.Validators.Validator;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp
{
    public class ValidadorPassagemSistemaAceiteManualReenvioArtespHandler : Loggable,
        ICommand<ValidadorPassagemSistemaAceiteManualReenvioRequest, ValidadorPassagemSistemaAceiteManualReenvioResponse>
    {

        readonly GenericValidator<PassagemPendenteArtesp> _validator;
        private PassagemForaDoPrazoValidator _passagemForaDoPrazoValidator;
        private ReenvioInvalidoValidator _reenvioInvalidoValidator;
        private TagPracaBloqueadoLadoMensageriaValidator _tagPracaBloqueadoLadoMensageriaValidator;
        private CobrancaIndevidaValidator _cobrancaIndevidaValidator;

        public ValidadorPassagemSistemaAceiteManualReenvioArtespHandler()
        {
            _validator = new GenericValidator<PassagemPendenteArtesp>();
            _passagemForaDoPrazoValidator = new PassagemForaDoPrazoValidator();
            _reenvioInvalidoValidator = new ReenvioInvalidoValidator();
            _tagPracaBloqueadoLadoMensageriaValidator = new TagPracaBloqueadoLadoMensageriaValidator();
            _cobrancaIndevidaValidator = new CobrancaIndevidaValidator();
        }

        /// <summary>
        /// Executa o processamento de Passagens
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ValidadorPassagemSistemaAceiteManualReenvioResponse Execute(ValidadorPassagemSistemaAceiteManualReenvioRequest request)
        {
            _passagemForaDoPrazoValidator.Init(request.PassagemPendenteArtesp);

            var motivoNaoCompensado = MotivoNaoCompensado.SemMotivoNaoCompensado;

            VerificarPassagemEnviadaForaDoPrazo(request);
            VerificarReenvioInvalido(request);
            VerificarTagPracaBloqueada(request);

            if(motivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
                motivoNaoCompensado = ValidarValorAceiteManualReenvio(request);

            if (motivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
                motivoNaoCompensado = ValidarCobrancaIndevida(request);

            //Retorna o objeto pelo response...
            var response = new ValidadorPassagemSistemaAceiteManualReenvioResponse();
            response.PassagemPendenteArtesp = request.PassagemPendenteArtesp;
            response.MotivoNaoCompensado = motivoNaoCompensado;

            return response;
        }

        private void VerificarReenvioInvalido(ValidadorPassagemSistemaAceiteManualReenvioRequest request)
        {
            if (request.PassagemPendenteArtesp.PassagemRecusadaMensageria != true)
            {
                Log.Debug($"Passagem ID: {request.PassagemPendenteArtesp.MensagemItemId} - Fluxo: ValidadorPassagemSistemaHandler |  Validar Reenvio Inválido");

                if (_reenvioInvalidoValidator.ValidateForaSequencia(request.PassagemPendenteArtesp))
                    request.PassagemPendenteArtesp.PassagemRecusadaMensageria = true;
            }
        }
        private MotivoNaoCompensado ValidarCobrancaIndevida(ValidadorPassagemSistemaAceiteManualReenvioRequest request)
        {
            Log.Debug($"Passagem ID: {request.PassagemPendenteArtesp.MensagemItemId} - Fluxo: ValidadorPassagemSistemaHandler | Validar Cobrança Indevida");            
            return _cobrancaIndevidaValidator.Validate(request.PassagemPendenteArtesp);
        }

        private MotivoNaoCompensado ValidarValorAceiteManualReenvio(ValidadorPassagemSistemaAceiteManualReenvioRequest request)
        {
            Log.Debug($"Passagem ID: {request.PassagemPendenteArtesp.MensagemItemId} - Fluxo: ValidadorPassagemSistemaHandler |  Validar Valor Aceite Manual Reenvio");
            if (_passagemForaDoPrazoValidator.AchouPassagemAnterior)
            {
                if (_passagemForaDoPrazoValidator.ValidateValor() && _passagemForaDoPrazoValidator.ValidatePrazo())                
                    return MotivoNaoCompensado.PassagemEnviadaForaDoPrazo;                
            }

            return MotivoNaoCompensado.SemMotivoNaoCompensado;
        }

        private void VerificarPassagemEnviadaForaDoPrazo(ValidadorPassagemSistemaAceiteManualReenvioRequest request)
        {
            if (request.PassagemPendenteArtesp.PassagemRecusadaMensageria != true && _passagemForaDoPrazoValidator.AchouPassagemAnterior)
            {
                Log.Debug($"Passagem ID: {request.PassagemPendenteArtesp.MensagemItemId} - Fluxo: ValidadorPassagemSistemaHandler | Verificar PassagemEnviadaForaDoPrazo");
                if (_passagemForaDoPrazoValidator.ValidatePrazo())
                    request.PassagemPendenteArtesp.PassagemRecusadaMensageria = true;
            }
        }

        public void VerificarTagPracaBloqueada(ValidadorPassagemSistemaAceiteManualReenvioRequest request)
        {
            if (request.PassagemPendenteArtesp.PassagemRecusadaMensageria != true)
            {
                Log.Debug($"Passagem ID: {request.PassagemPendenteArtesp.MensagemItemId} - Fluxo: ValidadorPassagemHandler | validarTagPracaBloqueada");
                _tagPracaBloqueadoLadoMensageriaValidator.Init(request.PassagemPendenteArtesp);
                if (_tagPracaBloqueadoLadoMensageriaValidator.ValidateSituacaoTag() ||
                    _tagPracaBloqueadoLadoMensageriaValidator.ValidatePracaBloqueada(request.PassagemPendenteArtesp.Praca.CodigoPraca ?? 0))
                {
                    request.PassagemPendenteArtesp.PassagemRecusadaMensageria = true;
                }
            }

        }
    }
}
