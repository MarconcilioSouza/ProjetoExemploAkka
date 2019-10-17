using ConectCar.Framework.Infrastructure.Cqrs.Commands;
using ConectCar.Framework.Infrastructure.Cqrs.Handlers;
using ConectCar.Framework.Infrastructure.Data.Ado.DataProviders;
using ConectCar.Framework.Infrastructure.Log;
using ConectCar.Transacoes.Domain.Enum;
using ConectCar.Transacoes.Domain.Model;
using ProcessadorPassagensActors.CommandQuery.Enums;
using ProcessadorPassagensActors.CommandQuery.Exceptions;
using ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Request;
using ProcessadorPassagensActors.CommandQuery.Handlers.Edi.Responses;
using ProcessadorPassagensActors.CommandQuery.Validators.Validator;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Edi
{
    public class ValidadorPassagemEdiHandler : Loggable, ICommand<ValidadorPassagemEdiActorRequest, ValidadorPassagemEdiActorResponse>
    {

        #region [Properties]
        readonly GenericValidator<PassagemPendenteEDI> _validator;
        #endregion

        #region [Ctor]
        public ValidadorPassagemEdiHandler()
        {
            _validator = new GenericValidator<PassagemPendenteEDI>();
        }
        #endregion

        public ValidadorPassagemEdiActorResponse Execute(ValidadorPassagemEdiActorRequest request)
        {
            var possuiTransacaoAprovadaManualmente = _validator.Validate(request.PassagemPendenteEdi, PassagemPendenteEdiValidatorEnum.PossuiTransacaoAprovadaManualmente.ToString());


            #region ValidarTempoSlaEnvioPassagem 
            if (!possuiTransacaoAprovadaManualmente)
            {
                Log.Info($"Passagem DetalheTrnId: {request.PassagemPendenteEdi.DetalheTrnId} - Fluxo: ValidadorPassagemEdiHandler | Validar ValidarTempoSlaEnvioPassagem");
                if (!_validator.Validate(request.PassagemPendenteEdi,
                    PassagemPendenteEdiValidatorEnum.ValidarTempoSlaEnvioPassagem.ToString()))
                    throw new EdiTransacaoException(CodigoRetornoTransacaoTRF.PassagemForaDoPeriodo,
                        request.PassagemPendenteEdi);
            }
            #endregion

            #region ValidarEmissorTagId 
            Log.Info($"Passagem DetalheTrnId: {request.PassagemPendenteEdi.DetalheTrnId} - Fluxo: ValidadorPassagemEdiHandler | Validar ValidarEmissorTagId");
            if (!_validator.Validate(request.PassagemPendenteEdi, PassagemPendenteEdiValidatorEnum.ValidarEmissorTagId.ToString()))
                throw new EdiTransacaoException(CodigoRetornoTransacaoTRF.EmissorTagInvalido, request.PassagemPendenteEdi);
            #endregion

            #region ValidarCategoria 
            Log.Info($"Passagem DetalheTrnId: {request.PassagemPendenteEdi.DetalheTrnId} - Fluxo: ValidadorPassagemEdiHandler | Validar ValidarCategoria");
            if (!_validator.Validate(request.PassagemPendenteEdi, PassagemPendenteEdiValidatorEnum.ValidarCategoria.ToString()))
                throw new EdiTransacaoException(CodigoRetornoTransacaoTRF.CATCobradaNaoCompativel, request.PassagemPendenteEdi);
            #endregion

            #region ValidarTag
            Log.Info($"Passagem DetalheTrnId: {request.PassagemPendenteEdi.DetalheTrnId} - Fluxo: ValidadorPassagemEdiHandler | Validar ValidarTag");
            if (!_validator.Validate(request.PassagemPendenteEdi, PassagemPendenteEdiValidatorEnum.ValidarTag.ToString()))
                throw new EdiTransacaoException(CodigoRetornoTransacaoTRF.EmissorTagInvalido, request.PassagemPendenteEdi);
            #endregion

            #region ValidarAdesao 
            Log.Info($"Passagem DetalheTrnId: {request.PassagemPendenteEdi.DetalheTrnId} - Fluxo: ValidadorPassagemEdiHandler | Validar ValidarAdesao");
            if (!_validator.Validate(request.PassagemPendenteEdi, PassagemPendenteEdiValidatorEnum.ValidarAdesao.ToString()))
                throw new EdiTransacaoException(CodigoRetornoTransacaoTRF.EmissorTagInvalido, request.PassagemPendenteEdi);
            #endregion

            #region ValidarPistaPraca
            Log.Info($"Passagem DetalheTrnId: {request.PassagemPendenteEdi.DetalheTrnId} - Fluxo: ValidadorPassagemEdiHandler | Validar ValidarPistaPraca");
            if (!_validator.Validate(request.PassagemPendenteEdi, PassagemPendenteEdiValidatorEnum.ValidarPistaPraca.ToString()))
                throw new EdiTransacaoException(CodigoRetornoTransacaoTRF.PracaInvalida, request.PassagemPendenteEdi);
            #endregion
            
            return new ValidadorPassagemEdiActorResponse { PassagemPendenteEdi = request.PassagemPendenteEdi };
        }
    }
}
