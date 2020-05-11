using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
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
            public string FantasyName { get; set; }
            public string Cnpj { get; set; }

            public ValidationResult Validate()
            {
                return new Validator().Validate(this);
            }

            private class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(a => a.Name).NotEmpty().Length(4, 200);
                    RuleFor(a => a.FantasyName).NotEmpty().Length(2, 150);
                    RuleFor(a => a.FantasyName).NotEmpty().Length(2, 150);
                }
            }
        }

        public class Handler : IRequestHandler<Command, Result<Exception, Guid>>
        {
            private readonly IMapper _mapper;
            private readonly ICompanyRepository _repository;

            public Handler(IMapper mapper, ICompanyRepository repository)
            {
                _mapper = mapper;
                _repository = repository;
            }

            public async Task<Result<Exception, Guid>> Handle(Command request, CancellationToken cancellationToken)
            {
                Company company = _mapper.Map<Command, Company>(request);

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
