using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;


namespace Totten.Solutions.WolfMonitor.Cfg.Startup.Behaviours
{
    public class ValidationPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, Result<Exception, TResponse>>
        where TRequest : IRequest<Result<Exception, TResponse>>
    {
        private readonly IValidator<TRequest>[] _validators;

        public ValidationPipeline(IValidator<TRequest>[] validators)
        {
            _validators = validators;
        }

        public async Task<Result<Exception, TResponse>> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<Result<Exception, TResponse>> next)
        {
            List<FluentValidation.Results.ValidationFailure> failures = _validators
                .Select(v => v.Validate(request))
                .SelectMany(result => result.Errors)
                .Where(error => error != null)
                .ToList();

            if (failures.Any())
                return new ValidationException(failures);

            return await next();
        }
    }
}
