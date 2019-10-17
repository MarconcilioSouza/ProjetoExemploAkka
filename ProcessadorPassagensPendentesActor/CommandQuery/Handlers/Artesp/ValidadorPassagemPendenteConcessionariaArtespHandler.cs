using ConectCar.Framework.Infrastructure.Cqrs.Commands;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Requests;
using ProcessadorPassagensActors.CommandQuery.Handlers.Artesp.Responses;
using ProcessadorPassagensActors.CommandQuery.Exceptions;
using ConectCar.Transacoes.Domain.Enum;
using ProcessadorPassagensActors.CommandQuery.Enums;
using ProcessadorPassagensActors.CommandQuery.Validators.Validator;
using ConectCar.Transacoes.Domain.ValueObject;
using ConectCar.Framework.Infrastructure.Log;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Artesp
{
    /// <summary>
    /// Responsável pelas seguintes validações:
    /// 1 - Concessionária)
    /// 2 - Código da passagem no conveniado
    /// </summary>
    public class ValidadorPassagemPendenteConcessionariaArtespHandler : Loggable,
        ICommand<ValidadorPassagemPendenteConcessionariaRequest, ValidadorPassagemPendenteConcessionariaResponse>
    {
        
        #region [Properties]

        private GenericValidator<PassagemPendenteArtesp> _validator;
        private PassagemCompensadaPreviamenteValidator _passagemCompensadaPreviamenteValidator;

        #endregion

        #region [Ctor]

        public ValidadorPassagemPendenteConcessionariaArtespHandler()
        {
            _validator = new GenericValidator<PassagemPendenteArtesp>();
            _passagemCompensadaPreviamenteValidator = new PassagemCompensadaPreviamenteValidator();
        }


        #endregion

        #region [Methods]

        public ValidadorPassagemPendenteConcessionariaResponse Execute(ValidadorPassagemPendenteConcessionariaRequest request)
        {
            var mensagemItemId = request.PassagemPendenteArtesp.MensagemItemId;
            var motivoNaoCompensado = MotivoNaoCompensado.SemMotivoNaoCompensado;

            if(motivoNaoCompensado== MotivoNaoCompensado.SemMotivoNaoCompensado)
                motivoNaoCompensado = ValidarConcessionaria(request, mensagemItemId);

            if (motivoNaoCompensado == MotivoNaoCompensado.SemMotivoNaoCompensado)
                motivoNaoCompensado = ValidarIdentificadorPassagem(request, mensagemItemId);

            Log.Debug($"Passagem ID: {mensagemItemId} - Fluxo: ValidadorPassagemPendenteHandler | Validar PassagemCompensadaPreviamente");
            _passagemCompensadaPreviamenteValidator.Validate(request.PassagemPendenteArtesp);


            var response = new ValidadorPassagemPendenteConcessionariaResponse {
                PassagemPendenteArtesp = request.PassagemPendenteArtesp,
                MotivoNaoCompensado = motivoNaoCompensado
            };
            return response;
        }

        private MotivoNaoCompensado ValidarIdentificadorPassagem(ValidadorPassagemPendenteConcessionariaRequest request, long mensagemItemId)
        {
            Log.Debug($"Passagem ID: {mensagemItemId} - Fluxo: ValidadorPassagemPendenteHandler | Validar IdentificadorPassagem");
            if (!_validator.Validate(request.PassagemPendenteArtesp, PassagemPendenteArtespValidatorEnum.ValidarIdentificadorPassagem.ToString()))
                return MotivoNaoCompensado.IdentificadorPassagemInvalido;
            return MotivoNaoCompensado.SemMotivoNaoCompensado;
        }

        private MotivoNaoCompensado ValidarConcessionaria(ValidadorPassagemPendenteConcessionariaRequest request, long mensagemItemId)
        {
            Log.Debug($"Passagem ID: {mensagemItemId} - Fluxo: ValidadorPassagemPendenteConcessionariaArtespHandler | Validar Concessionária");
            if (!_validator.Validate(request.PassagemPendenteArtesp, PassagemPendenteArtespValidatorEnum.ValidarConcessionaria.ToString()))
                return MotivoNaoCompensado.ConcessionariaInvalida;
            return MotivoNaoCompensado.SemMotivoNaoCompensado;
        }

        #endregion
    }
}
