using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Features.Companies;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Application.Features.Authorizations
{
    public class AuthorizeLogin
    {
        public class Query : IRequest<Result<Exception, User>>
        {
            public string Company { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }

            public Query(string company)
            {
                Company = company;
            }

            public ValidationResult Validate()
            {
                return new Validator().Validate(this);
            }

            public class Validator : AbstractValidator<Query>
            {
                public Validator()
                {
                    RuleFor(u => u.UserName).NotEmpty().WithMessage("Nome de usuário não informado");
                    RuleFor(u => u.Password).NotEmpty().WithMessage("Senha não informada");
                    RuleFor(u => u.Company).NotEmpty().WithMessage("Nome de empresa não informado");
                }
            }
        }

        public class Handler : IRequestHandler<Query, Result<Exception, User>>
        {
            private IUserRepository _userRepository;
            private ICompanyRepository _companyRepository;

            public Handler(IUserRepository userRepository, ICompanyRepository companyRepository)
            {
                _userRepository = userRepository;
                _companyRepository = companyRepository;
            }

            public async Task<Result<Exception, User>> Handle(Query request, CancellationToken cancellationToken)
            {

                Result<Exception, Company> companCallback = await _companyRepository.GetByNameAsync(request.Company);

                if (companCallback.IsFailure)
                    return companCallback.Failure;

                return await _userRepository.GetByCredentials(companCallback.Success.Id, request.Password, request.Company);
            }

        }
    }
}
