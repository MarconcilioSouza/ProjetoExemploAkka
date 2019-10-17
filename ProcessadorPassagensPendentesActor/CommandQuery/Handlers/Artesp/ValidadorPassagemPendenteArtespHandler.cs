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
    public class ValidadorPassagemPendenteArtespHandler: Loggable,
            ICommand<ValidadorPassagemPendenteRequest, ValidadorPassagemPendenteResponse>
    {
        #region [Properties]

        readonly GenericValidator<PassagemPendenteArtesp> _validator;

        #endregion

        #region [Ctor]

        public ValidadorPassagemPendenteArtespHandler()
        {
            _validator = new GenericValidator<PassagemPendenteArtesp>();
        }

        #endregion


        /// <summary>
        /// Executa o processamento de Passagens
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ValidadorPassagemPendenteResponse Execute(ValidadorPassagemPendenteRequest request)
        {
            var mensagemItemId = request.PassagemPendenteArtesp.MensagemItemId;
            var motivoNaoCompensado = MotivoNaoCompensado.SemMotivoNaoCompensado;

            if (motivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
                motivoNaoCompensado = ValidarMotivoSemValor(request, mensagemItemId);

            if (motivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
                motivoNaoCompensado = ValidarCodigoPraca(request, mensagemItemId);

            if (motivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
                motivoNaoCompensado = ValidarReenvioEPrimeiroEnvio(request, mensagemItemId);

            if (motivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
                motivoNaoCompensado = ValidarPassagemManual(request, mensagemItemId);

            if (motivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
                motivoNaoCompensado = ValidarPassagemAutomatica(request, mensagemItemId);

            if (motivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
                motivoNaoCompensado = ValidarGrupoIsento(request, mensagemItemId);            

            var response = new ValidadorPassagemPendenteResponse
            {
                PassagemPendenteArtesp = request.PassagemPendenteArtesp,
                MotivoNaoCompensado = motivoNaoCompensado
            };
            return response;
        }

        private MotivoNaoCompensado ValidarGrupoIsento(ValidadorPassagemPendenteRequest request, long mensagemItemId)
        {
            Log.Debug($"Passagem ID: {mensagemItemId} - Fluxo: ValidadorPassagemPendenteHandler | ValidarGrupoIsentoDadosInvalidos");
            if (_validator.Validate(request.PassagemPendenteArtesp, PassagemPendenteArtespValidatorEnum.ValidarGrupoIsentoDadosInvalidos.ToString()))
                return MotivoNaoCompensado.DadosInvalidos;

            Log.Debug($"Passagem ID: {mensagemItemId} - Fluxo: ValidadorPassagemPendenteHandler | ValidarGrupoIsentoMotivoSemValorNaoInformado");
            if (_validator.Validate(request.PassagemPendenteArtesp, PassagemPendenteArtespValidatorEnum.ValidarGrupoIsentoMotivoSemValorNaoInformado.ToString()))
                return MotivoNaoCompensado.MotivoSemValorNaoInformado;

            return MotivoNaoCompensado.SemMotivoNaoCompensado;
        }

        private MotivoNaoCompensado ValidarPassagemAutomatica(ValidadorPassagemPendenteRequest request, long mensagemItemId)
        {
            if (request.PassagemPendenteArtesp.StatusPassagem == StatusPassagem.Automatica)
            {
                Log.Debug($"Passagem ID: {mensagemItemId} - Fluxo: ValidadorPassagemPendenteHandler | Validar FlagPassagemAutomaticaNaoInformada");
                if (!_validator.Validate(request.PassagemPendenteArtesp, PassagemPendenteArtespValidatorEnum.ValidarFlagPassagemAutomatica.ToString()))
                {
                    request.PassagemPendenteArtesp.PassagemRecusadaMensageria = true;
                    return MotivoNaoCompensado.FlagPassagemAutomaticaNaoInformada;
                }
            }

            return MotivoNaoCompensado.SemMotivoNaoCompensado;
        }

        private MotivoNaoCompensado ValidarPassagemManual(ValidadorPassagemPendenteRequest request, long mensagemItemId)
        {
            if (request.PassagemPendenteArtesp.StatusPassagem == StatusPassagem.Manual)
            {
                Log.Debug($"Passagem ID: {mensagemItemId} - Fluxo: ValidadorPassagemPendenteHandler | Validar MotivoManualNaoInformado");
                if (!_validator.Validate(request.PassagemPendenteArtesp, PassagemPendenteArtespValidatorEnum.ValidarMotivoManualNaoInformado.ToString()))
                {
                    request.PassagemPendenteArtesp.PassagemRecusadaMensageria = true;
                    return MotivoNaoCompensado.MotivoManualNaoInformado;
                }

                Log.Debug($"Passagem ID: {mensagemItemId} - Fluxo: ValidadorPassagemPendenteHandler | Validar PassagemManualSemValorBloqueadoComMotivoDiferenteDeBloqueado");
                if (!_validator.Validate(request.PassagemPendenteArtesp, PassagemPendenteArtespValidatorEnum.ValidarPassagemManualSemValorBloqueadoComMotivoDiferenteDeBloqueado.ToString()))
                {
                    request.PassagemPendenteArtesp.PassagemRecusadaMensageria = true;
                    return MotivoNaoCompensado.PassagemManualSemValorBloqueadoComMotivoDiferenteDeBloqueado;
                }

                Log.Debug($"Passagem ID: {mensagemItemId} - Fluxo: ValidadorPassagemPendenteHandler | Validar Passagem Manual Sem Tag");
                if (!_validator.Validate(request.PassagemPendenteArtesp, PassagemPendenteArtespValidatorEnum.ValidarPassagemManualSemTag.ToString()))
                {
                    return MotivoNaoCompensado.TagInvalido;
                }
            }

            return MotivoNaoCompensado.SemMotivoNaoCompensado;
        }

        private MotivoNaoCompensado ValidarReenvioEPrimeiroEnvio(ValidadorPassagemPendenteRequest request, long mensagemItemId)
        {
            if (_validator.Validate(request.PassagemPendenteArtesp, PassagemPendenteArtespValidatorEnum.EReenvio.ToString()))
            {
                Log.Debug($"Passagem ID: {mensagemItemId} - Fluxo: ValidadorPassagemPendenteHandler | Validar Motivo Reenvio Não Informado");
                if (!_validator.Validate(request.PassagemPendenteArtesp, PassagemPendenteArtespValidatorEnum.ValidarMotivoReenvioNaoInformado.ToString()))
                {
                    request.PassagemPendenteArtesp.PassagemRecusadaMensageria = true;
                    return MotivoNaoCompensado.MotivoReenvioNaoInformado;
                }
            }
            else
            {
                Log.Debug($"Passagem ID: {mensagemItemId} - Fluxo: ValidadorPassagemPendenteHandler | Validar Motivo Reenvio no 1º Envio");
                if (!_validator.Validate(request.PassagemPendenteArtesp, PassagemPendenteArtespValidatorEnum.ValidarReenvioSemNumeroDeReenvio.ToString()))
                {
                    request.PassagemPendenteArtesp.PassagemRecusadaMensageria = true;
                    return MotivoNaoCompensado.SemNumeroDeReenvio;
                }
            }

            return MotivoNaoCompensado.SemMotivoNaoCompensado;
        }

        private MotivoNaoCompensado ValidarCodigoPraca(ValidadorPassagemPendenteRequest request, long mensagemItemId)
        {
            Log.Debug($"Passagem ID: {mensagemItemId} - Fluxo: ValidadorPassagemPendenteHandler | Validar CodigoPraca");
            if (!_validator.Validate(request.PassagemPendenteArtesp, PassagemPendenteArtespValidatorEnum.ValidarCodigoPraca.ToString()))
            {
                request.PassagemPendenteArtesp.PassagemRecusadaMensageria = true;
                return MotivoNaoCompensado.PracaInvalida;
            }

            return MotivoNaoCompensado.SemMotivoNaoCompensado;
        }

        private MotivoNaoCompensado ValidarMotivoSemValor(ValidadorPassagemPendenteRequest request, long mensagemItemId)
        {
            if (request.PassagemPendenteArtesp.Valor == 0)
            {
                Log.Debug($"Passagem ID: {mensagemItemId} - Fluxo: ValidadorPassagemPendenteHandler | Validar MotivoSemValor");
                if (!_validator.Validate(request.PassagemPendenteArtesp, PassagemPendenteArtespValidatorEnum.ValidarMotivoSemValor.ToString()))
                {
                    request.PassagemPendenteArtesp.PassagemRecusadaMensageria = true;
                     return MotivoNaoCompensado.MotivoSemValorNaoInformado;
                }
            }

            return MotivoNaoCompensado.SemMotivoNaoCompensado;
        }
    }
}
