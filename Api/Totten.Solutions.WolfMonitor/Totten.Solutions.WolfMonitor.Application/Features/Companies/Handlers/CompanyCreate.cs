using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Features.Companies;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Application.Features.Companies.Handlers
{
    public class CompanyCreate
    {
        public class Command : IRequest<Result<Exception, Guid>>
        {
            public string Name { get; set; }
            public string Nick { get; set; }

            public ValidationResult Validate()
            {
                return new Validator().Validate(this);
            }

            private class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(a => a.Name).NotEmpty().Length(4, 100);
                    RuleFor(a => a.Nick).NotEmpty().Length(2, 50);
                }
            }
        }

        public class Handler : IRequestHandler<Command, Result<Exception, Guid>>
        {
            private readonly ICompanyRepository _repository;

            public Handler(ICompanyRepository repository)
            {
                _repository = repository;
            }

            public async Task<Result<Exception, Guid>> Handle(Command request, CancellationToken cancellationToken)
            {
                var company = Mapper.Map<Command, Company>(request);

                Result<Exception, Company> callback = await _repository.CreateAsync(company);

                if (callback.IsFailure)
                {
                    return callback.Failure;
                }

                return callback.Success.Id;
            }
        }
    }
}
