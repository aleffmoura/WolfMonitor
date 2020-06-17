using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Enums;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.Companies;
using Totten.Solutions.WolfMonitor.Domain.Features.Logs;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Extensions;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Validation.FluentValidations;

namespace Totten.Solutions.WolfMonitor.Application.Features.UsersAggregation.Handlers
{
    public class UserCreate
    {
        public class Command : IRequest<Result<Exception, Guid>>
        {
            public Guid UserId { get; set; }
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

            public Command(Guid userId, Guid userCompany, Guid companyId, string email, string cpf,
                           string firstName, string lastName, string language,
                           string login, string password, string roleCurrentUser)
            {
                UserId = userId;
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
                    RuleFor(a => a.UserId).NotEqual(Guid.Empty).WithMessage("Usuário da solicitação está inválido");
                    RuleFor(a => a.UserCompany).NotEqual(Guid.Empty).WithMessage("Empresa do usuário está inválida");
                    RuleFor(a => a.CompanyId).NotEqual(Guid.Empty).WithMessage("Empresa da solicitação está inválido");
                    RuleFor(a => a.Email).EmailAddress().WithMessage("Email não é válido")
                                        .Length(6, 200).WithMessage("Email deve possuir entre 6 e 200 caracteres");
                    RuleFor(a => a.Cpf).IsValidCPF();
                    RuleFor(a => a.FirstName).Length(3, 25).WithMessage("Nome deve possuir entre 3 e 25 caracteres");
                    RuleFor(a => a.LastName).Length(4, 150).WithMessage("Sobrenome deve possuir entre 3 e 25 caracteres");
                    RuleFor(a => a.Language).Length(5).WithMessage("Linguagem  deve possuir 5 caracteres");
                    RuleFor(a => a.Login).Length(4, 100).WithMessage("Login deve possuir entre 4 e 100 caracteres");
                    RuleFor(a => a.Password).MinimumLength(8).WithMessage("Senha deve possuir no mínimo 8 caracteres");
                }
            }
        }

        public class Handler : IRequestHandler<Command, Result<Exception, Guid>>
        {
            private readonly IUserRepository _repository;
            private readonly IRoleRepository _roleRepository;
            private readonly ICompanyRepository _companyRepository;
            private readonly ILogRepository _logRepository;

            public Handler(IUserRepository repository,
                            IRoleRepository roleRepository,
                            ICompanyRepository companyRepository,
                            ILogRepository logRepository)
            {
                _repository = repository;
                _roleRepository = roleRepository;
                _companyRepository = companyRepository;
                _logRepository = logRepository;
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
                    return new BusinessException(ErrorCodes.AlreadyExists, "Já existe um usuário com o cpf informado cadastrado nesta empresa.");

                if (usersCallback.Success.Any(u => u.Email.Equals(request.Email)))
                    return new BusinessException(ErrorCodes.AlreadyExists, "Já existe um usuário com o email informado cadastrado nesta empresa.");

                if (usersCallback.Success.Any(u => u.Login.Equals(request.Login)))
                    return new BusinessException(ErrorCodes.AlreadyExists, "Já existe um usuário com o login informado cadastrado nesta empresa.");
                
                Result<Exception, Role> role = await _roleRepository.GetRoleAsync(RoleLevelEnum.User);

                if (role.IsFailure)
                    return role.Failure;

                request.Password = request.Password.GenerateHash();
                User user = Mapper.Map<Command, User>(request);
                user.RoleId = role.Success.Id;

                Result<Exception, User> callback = await _repository.CreateAsync(user);

                if (callback.IsFailure)
                    return callback.Failure;

                Log log = new Log
                {
                    UserId = request.UserId,
                    UserCompanyId = request.UserCompany,
                    TargetId = callback.Success.Id,
                    EntityType = ETypeEntity.Users,
                    TypeLogMethod = ETypeLogMethod.Create,
                    CreatedIn = DateTime.Now
                };

                await _logRepository.CreateAsync(log);

                return callback.Success.Id;
            }
        }
    }
}
