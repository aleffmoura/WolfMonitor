using FluentValidation;

namespace Totten.Solutions.WolfMonitor.Infra.CrossCutting.Validation.FluentValidations
{
    public static partial class ValidatorExtensions
    {
        public static IRuleBuilderOptions<T, string> IsValidCNPJ<T>(this IRuleBuilder<T, string> ruleBuilder)
            => ruleBuilder.SetValidator(new CNPJValidator());
        public static IRuleBuilderOptions<T, string> IsValidCPF<T>(this IRuleBuilder<T, string> ruleBuilder)
            => ruleBuilder.SetValidator(new CPFValidator());
    }
}