using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Features.Companies;
using Totten.Solutions.WolfMonitor.Domain.Features.Users;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Extensions;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Application.Features.Users.Handlers
{
    public class UserCreate
    {
        public class Command : IRequest<Result<Exception, Guid>>
        {
            public Guid CompanyId { get; set; }
            public string Email { get; set; }
            public string Cpf { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Language { get; set; }
            public string Login { get; set; }
            public string Password { get; set; }

            public ValidationResult Validate()
            {
                return new Validator().Validate(this);
            }

            private class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(a => a.CompanyId).NotEqual(Guid.Empty);
                    RuleFor(a => a.Email).NotEmpty().Length(6, 200);
                    RuleFor(a => a.Cpf).NotEmpty().Length(11);
                    RuleFor(a => a.FirstName).NotEmpty().Length(6, 25);
                    RuleFor(a => a.LastName).NotEmpty().Length(6, 150);
                    RuleFor(a => a.Language).NotEmpty().Length(5);
                    RuleFor(a => a.Login).NotEmpty().Length(4, 100);
                    RuleFor(a => a.Password).NotEmpty().Length(8, 150);
                }
            }
        }

        public class Handler : IRequestHandler<Command, Result<Exception, Guid>>
        {
            private readonly IUserRepository _repository;
            private readonly IRoleRepository _roleRepository;
            private readonly ICompanyRepository _companyRepository;

            public Handler(IUserRepository repository,
                           IRoleRepository roleRepository,
                           ICompanyRepository companyRepository)
            {
                _repository = repository;
                _roleRepository = roleRepository;
                _companyRepository = companyRepository;
            }

            public async Task<Result<Exception, Guid>> Handle(Command request,
                                                              CancellationToken cancellationToken)
            {
                Result<Exception, Company> company = await _companyRepository.GetByIdAsync(request.CompanyId);
                if (company.IsFailure)
                {
                    return company.Failure;
                }

                Result<Exception, Role> role = await _roleRepository.GetRoleAsync(RoleLevelEnum.User);
                if (role.IsFailure)
                {
                    return role.Failure;
                }
                request.Password = request.Password.GenerateHash();
                User user = Mapper.Map<Command, User>(request);
                user.RoleId = role.Success.Id;

                Result<Exception, User> callback = await _repository.CreateAsync(user);

                if (callback.IsFailure)
                {
                    return callback.Failure;
                }

                return callback.Success.Id;
            }
        }
    }
}
