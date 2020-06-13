using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Totten.Solutions.WolfMonitor.Infra.CrossCutting.Validation
{
    public class CNPJValidation : IValidator
    {
        public bool CanValidateInstancesOfType(Type type)
        {
            throw new NotImplementedException();
        }

        public IValidatorDescriptor CreateDescriptor()
        {
            throw new NotImplementedException();
        }

        public ValidationResult Validate(object instance)
        {



            return new ValidationResult(new List<ValidationFailure>
            {

                new ValidationFailure("CNPJ", "Cnpj está inválido")
            }); ;
        }

        public ValidationResult Validate(ValidationContext context)
        {
            throw new NotImplementedException();
        }

        public Task<ValidationResult> ValidateAsync(object instance, CancellationToken cancellation = default)
        {
            throw new NotImplementedException();
        }

        public Task<ValidationResult> ValidateAsync(ValidationContext context, CancellationToken cancellation = default)
        {
            throw new NotImplementedException();
        }
    }
}
