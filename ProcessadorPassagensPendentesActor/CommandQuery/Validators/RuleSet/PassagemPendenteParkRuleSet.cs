using ConectCar.Transacoes.Domain.ValueObject;
using FluentValidation;
using ProcessadorPassagensActors.CommandQuery.Enums;

namespace ProcessadorPassagensActors.CommandQuery.Validators.RuleSet
{
    public class PassagemPendenteParkRuleSet : AbstractValidator<PassagemPendenteEstacionamento>
    {
        public PassagemPendenteParkRuleSet()
        {
            RuleSet(PassagemPendenteParkValidatorEnum.ValidarPossuiNumeroConveniado.ToString(), () =>
            {
                RuleFor(passagem => passagem).Must(x => x.Conveniado.CodigoProtocolo != 0);
            });

            RuleSet(PassagemPendenteParkValidatorEnum.ValidarPossuiNumeroTag.ToString(), () =>
            {
                RuleFor(passagem => passagem).Must(x => x.Tag.OBUId != 0);
            });

            RuleSet(PassagemPendenteParkValidatorEnum.ValidarPossuiNumeroPraca.ToString(), () =>
            {
                RuleFor(passagem => passagem).Must(x => (x.Praca.CodigoPraca ?? 0) != 0);
            });

            RuleSet(PassagemPendenteParkValidatorEnum.ValidarPossuiNumeroPista.ToString(), () =>
            {
                RuleFor(passagem => passagem).Must(x => x.Pista.CodigoPista != 0);
            });

            RuleSet(PassagemPendenteParkValidatorEnum.ValidarConveniado.ToString(), () =>
            {
                RuleFor(passagem => passagem).Must(x => (x.Conveniado.Id ?? 0) != 0);
            });

            RuleSet(PassagemPendenteParkValidatorEnum.ValidarTag.ToString(), () =>
            {
                RuleFor(passagem => passagem).Must(x => (x.Tag.Id ?? 0) != 0);
            });

            RuleSet(PassagemPendenteParkValidatorEnum.ValidarPraca.ToString(), () =>
            {
                RuleFor(passagem => passagem).Must(x => (x.Praca.Id ?? 0) != 0);
            });

            RuleSet(PassagemPendenteParkValidatorEnum.ValidarPista.ToString(), () =>
            {
                RuleFor(passagem => passagem).Must(x => (x.Pista.Id ?? 0) != 0);
            });

            RuleSet(PassagemPendenteParkValidatorEnum.ValidarValor.ToString(), () =>
            {
                RuleFor(passagem => passagem).Must(x => x.Valor >= 0);
            });
        }
    }
}
