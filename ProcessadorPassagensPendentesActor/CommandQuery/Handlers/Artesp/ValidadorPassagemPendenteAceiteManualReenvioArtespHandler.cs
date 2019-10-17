using ConectCar.Framework.Infrastructure.Cqrs.Commands;
using ConectCar.Framework.Infrastructure.Log;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Enums;
using ProcessadorPassagensActors.CommandQuery.Exceptions;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Responses;
using ProcessadorPassagensActors.CommandQuery.Validators.Validator;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp
{
    /// <summary>
    /// Responsável por validar as seguintes informações:
    /// 
    /// 1 - Praça Inválida
    /// 2 - Passagem Manual Sem Tag
    /// </summary>
    public class ValidadorPassagemPendenteAceiteManualReenvioArtespHandler : Loggable,
            ICommand<ValidadorPassagemPendenteAceiteManualReenvioRequest, ValidadorPassagemPendenteAceiteManualReenvioResponse>
    {
        #region [Properties]

        readonly GenericValidator<PassagemPendenteArtesp> _validator;

        #endregion

        #region [Ctor]

        public ValidadorPassagemPendenteAceiteManualReenvioArtespHandler()
        {
            _validator = new GenericValidator<PassagemPendenteArtesp>();
        }

        #endregion


        /// <summary>
        /// Executa o processamento de Passagens
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ValidadorPassagemPendenteAceiteManualReenvioResponse Execute(ValidadorPassagemPendenteAceiteManualReenvioRequest request)
        {
            var mensagemItemId = request.PassagemPendenteArtesp.MensagemItemId;
            var motivoNaoCompensado = MotivoNaoCompensado.SemMotivoNaoCompensado;

            VerificarMotivoReenvioNaoInformado(request, mensagemItemId);

            VerificarPassagemManual(request, mensagemItemId);

            VerificarPassagemAutomatica(request, mensagemItemId);

            VerificarMotivoSemValor(request, mensagemItemId);

            if(motivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
                motivoNaoCompensado = ValidarCodigoPraca(request, mensagemItemId);

            if (motivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
                motivoNaoCompensado = ValidarPassagemManualSemTag(request, mensagemItemId);

            var response = new ValidadorPassagemPendenteAceiteManualReenvioResponse {
                PassagemPendenteArtesp = request.PassagemPendenteArtesp,
                MotivoNaoCompensado = motivoNaoCompensado
            };
            return response;
        }

        private void VerificarMotivoSemValor(ValidadorPassagemPendenteAceiteManualReenvioRequest request, long mensagemItemId)
        {
            if (request.PassagemPendenteArtesp.PassagemRecusadaMensageria != true)
            {
                if (request.PassagemPendenteArtesp.Valor == 0)
                {
                    Log.Debug($"Passagem ID: {mensagemItemId} - Fluxo: ValidadorPassagemPendenteHandler | Validar MotivoSemValor");
                    if (!_validator.Validate(request.PassagemPendenteArtesp, PassagemPendenteArtespValidatorEnum.ValidarMotivoSemValor.ToString()))
                    {
                        request.PassagemPendenteArtesp.PassagemRecusadaMensageria = true;
                    }
                }
            }
        }

        private MotivoNaoCompensado ValidarPassagemManualSemTag(ValidadorPassagemPendenteAceiteManualReenvioRequest request, long mensagemItemId)
        {
            if (request.PassagemPendenteArtesp.StatusPassagem == StatusPassagem.Manual)
            {
                Log.Debug($"Passagem ID: {mensagemItemId} - Fluxo: ValidadorPassagemPendenteHandler | Validar Passagem Manual Sem Tag");
                if (!_validator.Validate(request.PassagemPendenteArtesp, PassagemPendenteArtespValidatorEnum.ValidarPassagemManualSemTag.ToString()))
                {
                    return MotivoNaoCompensado.TagInvalido;
                }
            }

            return MotivoNaoCompensado.SemMotivoNaoCompensado;
        }

        private MotivoNaoCompensado ValidarCodigoPraca(ValidadorPassagemPendenteAceiteManualReenvioRequest request, long mensagemItemId)
        {
            Log.Debug($"Passagem ID: {mensagemItemId} - Fluxo: ValidadorPassagemPendenteHandler | Validar CodigoPraca");
            if (!_validator.Validate(request.PassagemPendenteArtesp, PassagemPendenteArtespValidatorEnum.ValidarCodigoPraca.ToString()))
            {
                request.PassagemPendenteArtesp.PassagemRecusadaMensageria = true;
                return MotivoNaoCompensado.PracaInvalida;
            }

            return MotivoNaoCompensado.SemMotivoNaoCompensado;
        }

        private void VerificarPassagemAutomatica(ValidadorPassagemPendenteAceiteManualReenvioRequest request, long mensagemItemId)
        {
            if (request.PassagemPendenteArtesp.PassagemRecusadaMensageria != true)
            {
                if (request.PassagemPendenteArtesp.StatusPassagem == StatusPassagem.Automatica)
                {
                    Log.Debug($"Passagem ID: {mensagemItemId} - Fluxo: ValidadorPassagemPendenteHandler | Validar FlagPassagemAutomaticaNaoInformada");
                    if (!_validator.Validate(request.PassagemPendenteArtesp, PassagemPendenteArtespValidatorEnum.ValidarFlagPassagemAutomatica.ToString()))
                    {
                        request.PassagemPendenteArtesp.PassagemRecusadaMensageria = true;
                    }
                }
            }
        }

        private void VerificarPassagemManual(ValidadorPassagemPendenteAceiteManualReenvioRequest request, long mensagemItemId)
        {
            if (request.PassagemPendenteArtesp.PassagemRecusadaMensageria != true)
            {
                if (request.PassagemPendenteArtesp.StatusPassagem == StatusPassagem.Manual)
                {
                    Log.Debug($"Passagem ID: {mensagemItemId} - Fluxo: ValidadorPassagemPendenteHandler | Validar MotivoManualNaoInformado");
                    if (!_validator.Validate(request.PassagemPendenteArtesp, PassagemPendenteArtespValidatorEnum.ValidarMotivoManualNaoInformado.ToString()))
                    {
                        request.PassagemPendenteArtesp.PassagemRecusadaMensageria = true;
                    }
                    else
                    {
                        Log.Debug($"Passagem ID: {mensagemItemId} - Fluxo: ValidadorPassagemPendenteHandler | Validar PassagemManualSemValorBloqueadoComMotivoDiferenteDeBloqueado");
                        if (_validator.Validate(request.PassagemPendenteArtesp, PassagemPendenteArtespValidatorEnum.ValidarPassagemManualSemValorBloqueadoComMotivoDiferenteDeBloqueado.ToString()))
                        {
                            request.PassagemPendenteArtesp.PassagemRecusadaMensageria = true;
                        }
                    }

                }
            }
        }

        private void VerificarMotivoReenvioNaoInformado(ValidadorPassagemPendenteAceiteManualReenvioRequest request, long mensagemItemId)
        {
            if (request.PassagemPendenteArtesp.PassagemRecusadaMensageria != true)
            {
                Log.Debug($"Passagem ID: {mensagemItemId} - Fluxo: ValidadorPassagemPendenteHandler | Validar Motivo Reenvio Não Informado");
                if (!_validator.Validate(request.PassagemPendenteArtesp, PassagemPendenteArtespValidatorEnum.ValidarMotivoReenvioNaoInformado.ToString()))
                {
                    request.PassagemPendenteArtesp.PassagemRecusadaMensageria = true;
                }
            }
        }
    }
}
