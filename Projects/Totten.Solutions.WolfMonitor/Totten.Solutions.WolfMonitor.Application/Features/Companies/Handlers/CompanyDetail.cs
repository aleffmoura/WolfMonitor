using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.Companies;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Application.Features.Companies.Handlers
{
    public class CompanyDetail
    {
        public class Query : IRequest<Result<Exception, Company>>
        {
            public Guid Id { get; set; }
            public Guid UserCompany { get; set; }
            public string Role { get; set; }

            public Query(Guid id, Guid userCompany, string role)
            {
                Id = id;
                UserCompany = userCompany;
                Role = role;
            }

            public ValidationResult Validate()
            {
                return new Validator().Validate(this);
            }

            private class Validator : AbstractValidator<Query>
            {
                public Validator()
                {
                    RuleFor(d => d.Id).NotEqual(Guid.Empty);
                    RuleFor(d => d.UserCompany).NotEqual(Guid.Empty);
                    RuleFor(d => d.Role).NotEmpty().WithMessage("Nivel do usuário está incorreto, contate o administrador");
                }
            }
        }

        public class Handler : IRequestHandler<Query, Result<Exception, Company>>
        {
            private readonly ICompanyRepository _repository;

            public Handler(ICompanyRepository repository)
            {
                _repository = repository;
            }

            public async Task<Result<Exception, Company>> Handle(Query request, CancellationToken cancellationToken)
            {
                var roleEnum = Enum.Parse<RoleLevelEnum>(request.Role);

                if (request.Id != request.UserCompany && roleEnum != (RoleLevelEnum.System | RoleLevelEnum.Admin))
                    return new BusinessException(Domain.Enums.ErrorCodes.Unauthorized, "Empresa não permitida ser acessada por seu usuário");

                return await _repository.GetByIdAsync(request.Id);
            }
        }
    }
}
