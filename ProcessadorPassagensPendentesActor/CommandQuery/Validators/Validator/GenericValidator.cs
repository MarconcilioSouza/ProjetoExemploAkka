using ConectCar.Framework.Infrastructure.Ioc;
using ConectCar.Framework.Infrastructure.Ioc.Validation;
using FluentValidation;

namespace ProcessadorPassagensActors.CommandQuery.Validators.Validator
{
    public class GenericValidator<T>
    {
        private AbstractValidator<T> _validator;

        public GenericValidator()
        {
            _validator = IocContainer.Container.ResolveAbstractValidator<T>();
        }

        public bool Validate(T item, string ruleSet)
        {
            return _validator.Validate(item, ruleSet: ruleSet).IsValid;
        }
    }
}
