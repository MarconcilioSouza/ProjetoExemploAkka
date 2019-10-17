using ConectCar.Framework.Infrastructure.Cqrs.Commands;
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
    public class ValidadorPassagemPendenteEdiHandler : Loggable,
        ICommand<ValidadorPassagemPendenteEdiRequest, ValidadorPassagemPendenteEdiResponse>
    {
        #region [Properties]

        readonly GenericValidator<PassagemPendenteEDI> _validator;

        #endregion

        #region [Ctor]

        public ValidadorPassagemPendenteEdiHandler()
        {
            _validator = new GenericValidator<PassagemPendenteEDI>();
        }

        #endregion
        public ValidadorPassagemPendenteEdiResponse Execute(ValidadorPassagemPendenteEdiRequest request)
        {
            var possuiTransacaoAprovadaManualmente = _validator.Validate(request.PassagemPendenteEdi, PassagemPendenteEdiValidatorEnum.PossuiTransacaoAprovadaManualmente.ToString());

            #region ValidarArquivoNulo Regra 1 
            Log.Info($"Passagem DetalheTrnId: {request.PassagemPendenteEdi.DetalheTrnId} - Fluxo: ValidadorPassagemPendenteEdiHandler | Validar ValidarArquivoNulo");
            if (!_validator.Validate(request.PassagemPendenteEdi, PassagemPendenteEdiValidatorEnum.ValidarArquivoNulo.ToString()))
                throw new EdiDomainException($"DetalheTRN não existente:{request.PassagemPendenteEdi.DetalheTrnId}", request.PassagemPendenteEdi);
            #endregion

            #region ValidarPossuiArquivoTrn Regra 2 
            Log.Info($"Passagem DetalheTrnId: {request.PassagemPendenteEdi.DetalheTrnId} - Fluxo: ValidadorPassagemPendenteEdiHandler | Validar ValidarPossuiArquivoTrn");
            if (!_validator.Validate(request.PassagemPendenteEdi, PassagemPendenteEdiValidatorEnum.ValidarPossuiArquivoTrn.ToString()))
                throw new EdiDomainException($"ArquivoTRN não existente do DetaheTrnId:{request.PassagemPendenteEdi.DetalheTrnId}", request.PassagemPendenteEdi);
            #endregion

            #region ValidarPossuiArquivoTrf Regra 3 
            Log.Info($"Passagem DetalheTrnId: {request.PassagemPendenteEdi.DetalheTrnId} - Fluxo: ValidadorPassagemPendenteEdiHandler | Validar ValidarPossuiArquivoTrf");
            if (!_validator.Validate(request.PassagemPendenteEdi, PassagemPendenteEdiValidatorEnum.ValidarPossuiArquivoTrf.ToString()))
                throw new EdiDomainException($"ArquivoTRF não existente do DetaheTrnId: {request.PassagemPendenteEdi.DetalheTrnId}", request.PassagemPendenteEdi);
            #endregion

            #region ValidarPossuiNumeroTag Regra 4 
            Log.Info($"Passagem DetalheTrnId: {request.PassagemPendenteEdi.DetalheTrnId} - Fluxo: ValidadorPassagemPendenteEdiHandler | Validar ValidarPossuiNumeroTag");
            if (!_validator.Validate(request.PassagemPendenteEdi, PassagemPendenteEdiValidatorEnum.ValidarPossuiNumeroTag.ToString()))
                throw new EdiTransacaoException(CodigoRetornoTransacaoTRF.EmissorTagInvalido, request.PassagemPendenteEdi);
            #endregion

            #region ValidarPassagemManualComNumeroTagInvalida Regra 9 
            Log.Info($"Passagem DetalheTrnId: {request.PassagemPendenteEdi.DetalheTrnId} - Fluxo: ValidadorPassagemPendenteEdiHandler | Validar ValidarPassagemManualComNumeroTagInvalida");
            if (!_validator.Validate(request.PassagemPendenteEdi, PassagemPendenteEdiValidatorEnum.ValidarPassagemManualComNumeroTagInvalida.ToString()))
                throw new EdiTransacaoException(CodigoRetornoTransacaoTRF.PassagemManualSemTag, request.PassagemPendenteEdi);
            #endregion

            #region ValidarPassagemListaNela Regra 13 
            if (!possuiTransacaoAprovadaManualmente)
            {
                Log.Info(
                    $"Passagem DetalheTrnId: {request.PassagemPendenteEdi.DetalheTrnId} - Fluxo: ValidadorPassagemPendenteEdiHandler | Validar ValidarPassagemListaNela");
                if (!_validator.Validate(request.PassagemPendenteEdi,PassagemPendenteEdiValidatorEnum.ValidarPassagemListaNela.ToString()))
                    throw new EdiTransacaoException(CodigoRetornoTransacaoTRF.PassagemValidaListaNela,
                        request.PassagemPendenteEdi);
            }
            #endregion

            #region ValidarPassagemIsenta 
            if (!possuiTransacaoAprovadaManualmente)
            {
                if (!_validator.Validate(request.PassagemPendenteEdi, PassagemPendenteEdiValidatorEnum.PassagemIsenta.ToString()))
                {
                    Log.Info(
                        $"Passagem DetalheTrnId: {request.PassagemPendenteEdi.DetalheTrnId} - Fluxo: ValidadorPassagemPendenteEdiHandler | Validar ValidarPassagemIsentaComValor");
                    if (!_validator.Validate(request.PassagemPendenteEdi, PassagemPendenteEdiValidatorEnum.ValidarPassagemIsentaComValor.ToString()))
                        throw new EdiTransacaoException(CodigoRetornoTransacaoTRF.PassageIsentoValorDifZero,
                            request.PassagemPendenteEdi);
                }
                else
                {
                    Log.Info(
                        $"Passagem DetalheTrnId: {request.PassagemPendenteEdi.DetalheTrnId} - Fluxo: ValidadorPassagemPendenteEdiHandler | Validar PassagemValorZerado");
                    if (!_validator.Validate(request.PassagemPendenteEdi, PassagemPendenteEdiValidatorEnum.PassagemValorZerado.ToString()))
                        throw new EdiTransacaoException(CodigoRetornoTransacaoTRF.PassagensIsentas,
                            request.PassagemPendenteEdi);
                } 
                #endregion
            }

            return new ValidadorPassagemPendenteEdiResponse { PassagemPendenteEdi = request.PassagemPendenteEdi };
        }
    }
}
