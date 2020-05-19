using AutoMapper;
using FluentValidation;
using FluentValidation.Resources;
using FluentValidation.Results;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.Companies;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Extensions;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Application.Features.UsersAggregation.Handlers
{
    public class UserCreate
    {
        public class Command : IRequest<Result<Exception, Guid>>
        {
            public Guid UserCompany { get; set; }
            public Guid CompanyId { get; set; }
            public string Email { get; set; }
            public string Cpf { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Language { get; set; }
            public string Login { get; set; }
            public string Password { get; set; }

            public string RoleCurrentUser { get; set; }

            public Command(Guid userCompany, Guid companyId, string email, string cpf,
                           string firstName, string lastName, string language,
                           string login, string password, string roleCurrentUser)
            {
                UserCompany = userCompany;
                CompanyId = companyId;
                Email = email;
                Cpf = cpf;
                FirstName = firstName;
                LastName = lastName;
                Language = language;
                Login = login;
                Password = password;
                RoleCurrentUser = roleCurrentUser;
            }

            public ValidationResult Validate()
            {
                return new Validator().Validate(this);
            }


            private class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(a => a.UserCompany).NotEqual(Guid.Empty);
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
                var roleEnum = Enum.Parse<RoleLevelEnum>(request.RoleCurrentUser);

                if (request.CompanyId != request.UserCompany && roleEnum != (RoleLevelEnum.System | RoleLevelEnum.Admin))
                    return new BusinessException(Domain.Enums.ErrorCodes.Unauthorized, "Usuário não possui os privilégios necessários para essa ação");

                Result<Exception, Company> company = await _companyRepository.GetByIdAsync(request.CompanyId);

                if (company.IsFailure)
                    return company.Failure;

                var usersCallback = _repository.GetAllByCompanyId(request.CompanyId);

                if (usersCallback.IsFailure)
                    return usersCallback.Failure;

                if (usersCallback.Success.Any(u => u.Cpf.Equals(request.Cpf)))
                    return new BusinessException(Domain.Enums.ErrorCodes.AlreadyExists, "Já existe um usuário com o cpf informado cadastrado nesta empresa.");

                if (usersCallback.Success.Any(u => u.Email.Equals(request.Email)))
                    return new BusinessException(Domain.Enums.ErrorCodes.AlreadyExists, "Já existe um usuário com o email informado cadastrado nesta empresa.");

                if (usersCallback.Success.Any(u => u.Login.Equals(request.Login)))
                    return new BusinessException(Domain.Enums.ErrorCodes.AlreadyExists, "Já existe um usuário com o login informado cadastrado nesta empresa.");
                
                Result<Exception, Role> role = await _roleRepository.GetRoleAsync(RoleLevelEnum.User);

                if (role.IsFailure)
                    return role.Failure;

                request.Password = request.Password.GenerateHash();
                User user = Mapper.Map<Command, User>(request);
                user.RoleId = role.Success.Id;
                Result<Exception, User> callback = await _repository.CreateAsync(user);

                if (callback.IsFailure)
                    return callback.Failure;

                return callback.Success.Id;
            }
        }
    }
}
