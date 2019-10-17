using ConectCar.Framework.Infrastructure.Cqrs.Commands;
using ConectCar.Framework.Infrastructure.Log;
using ConectCar.Transacoes.Domain.ValueObject;
using ProcessadorPassagensActors.CommandQuery.Enums;
using ProcessadorPassagensActors.CommandQuery.Exceptions;
using ProcessadorPassagensActors.CommandQuery.Handlers.Park.Request;
using ProcessadorPassagensActors.CommandQuery.Handlers.Park.Response;
using ProcessadorPassagensActors.CommandQuery.Validators.Validator;

namespace ProcessadorPassagensActors.CommandQuery.Handlers.Park
{
    public class ValidarPassagemPendenteParkHandler : Loggable, ICommand<ValidarPassagemPendenteParkRequest, ValidarPassagemPendenteParkResponse>
    {
        #region [Properties]

        private readonly GenericValidator<PassagemPendenteEstacionamento> _validator;

        #endregion [Properties]

        #region [Ctor]

        public ValidarPassagemPendenteParkHandler()
        {
            _validator = new GenericValidator<PassagemPendenteEstacionamento>();
        }

        #endregion [Ctor]

        public ValidarPassagemPendenteParkResponse Execute(ValidarPassagemPendenteParkRequest request)
        {
            Log.Info($"Passagem RegistroTransacaoId: {request.PassagemPendenteEstacionamento.RegistroTransacaoId} - Fluxo: ValidarPassagemPendenteParkHandler | ValidarPossuiNumeroConveniado");
            if (!_validator.Validate(request.PassagemPendenteEstacionamento, PassagemPendenteParkValidatorEnum.ValidarPossuiNumeroConveniado.ToString()))
                throw new ParkException(request.PassagemPendenteEstacionamento, EstacionamentoErros.ConveniadoNaoInformado);

            Log.Info($"Passagem RegistroTransacaoId: {request.PassagemPendenteEstacionamento.RegistroTransacaoId} - Fluxo: ValidarPassagemPendenteParkHandler | ValidarPossuiNumeroTag");
            if (!_validator.Validate(request.PassagemPendenteEstacionamento, PassagemPendenteParkValidatorEnum.ValidarPossuiNumeroTag.ToString()))
                throw new ParkException(request.PassagemPendenteEstacionamento, EstacionamentoErros.TagNaoInformada);

            Log.Info($"Passagem RegistroTransacaoId: {request.PassagemPendenteEstacionamento.RegistroTransacaoId} - Fluxo: ValidarPassagemPendenteParkHandler | ValidarPossuiNumeroPraca");
            if (!_validator.Validate(request.PassagemPendenteEstacionamento, PassagemPendenteParkValidatorEnum.ValidarPossuiNumeroPraca.ToString()))
                throw new ParkException(request.PassagemPendenteEstacionamento, EstacionamentoErros.PracaNaoInformada);

            Log.Info($"Passagem RegistroTransacaoId: {request.PassagemPendenteEstacionamento.RegistroTransacaoId} - Fluxo: ValidarPassagemPendenteParkHandler | ValidarPossuiNumeroPista");
            if (!_validator.Validate(request.PassagemPendenteEstacionamento, PassagemPendenteParkValidatorEnum.ValidarPossuiNumeroPista.ToString()))
                throw new ParkException(request.PassagemPendenteEstacionamento, EstacionamentoErros.PistaNaoInformada);

            Log.Info($"Passagem RegistroTransacaoId: {request.PassagemPendenteEstacionamento.RegistroTransacaoId} - Fluxo: ValidarPassagemPendenteParkHandler | ValidarValor");
            if (!_validator.Validate(request.PassagemPendenteEstacionamento, PassagemPendenteParkValidatorEnum.ValidarValor.ToString()))
                throw new ParkException(request.PassagemPendenteEstacionamento, EstacionamentoErros.ValorCobradoMenorZero);

            return new ValidarPassagemPendenteParkResponse { PassagemPendenteEstacionamento = request.PassagemPendenteEstacionamento };
        }
    }
}