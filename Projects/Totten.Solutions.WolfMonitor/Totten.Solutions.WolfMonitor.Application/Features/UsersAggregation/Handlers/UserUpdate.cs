using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Extensions;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;
using Unit = Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs.Unit;

namespace Totten.Solutions.WolfMonitor.Application.Features.UsersAggregation.Handlers
{
    public class UserUpdate
    {
        public class Command : IRequest<Result<Exception, Unit>>
        {
            public Guid CompanyId { get; set; }
            public Guid UserId { get; set; }
            public string Login { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public Command(Guid userId, Guid companyId, string login, string password, string email,
                            string firstname, string lastname)
            {
                CompanyId = companyId;
                UserId = userId;
                Login = login;
                Password = password;
                Email = email;
                FirstName = firstname;
                LastName = lastname;
            }
            public ValidationResult Validate()
            {
                return new Validator().Validate(this);
            }

            private class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(a => a.Login).Length(4, 100).WithMessage("Login deve possuir entre 4 e 100 caracteres");
                    RuleFor(a => a.Password).MinimumLength(8).WithMessage("Senha deve possuir no mínimo 8 caracteres");
                    RuleFor(a => a.Email).EmailAddress().WithMessage("Email não é válido")
                                        .Length(6, 200).WithMessage("Email deve possuir entre 6 e 200 caracteres");
                    RuleFor(a => a.FirstName).Length(3, 25).WithMessage("Nome deve possuir entre 3 e 25 caracteres");
                    RuleFor(a => a.LastName).Length(4, 150).WithMessage("Sobrenome deve possuir entre 3 e 25 caracteres");
                }
            }
        }

        public class Handler : IRequestHandler<Command, Result<Exception, Unit>>
        {
            private readonly IUserRepository _repository;

            public Handler(IUserRepository repository)
            {
                _repository = repository;
            }

            public async Task<Result<Exception, Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var userEmail = await _repository.GetByEmail(request.CompanyId, request.Email);

                if (userEmail.IsSuccess && userEmail.Success.Id != request.UserId)
                    return new DuplicateException("Já existe um usuário com o email informado.");

                var userLogin = await _repository.GetByLogin(request.CompanyId, request.Login);

                if (userEmail.IsSuccess && userEmail.Success.Id != request.UserId)
                    return new DuplicateException("Já existe um usuário com o login informado.");

                Result<Exception, User> userCallback = await _repository.GetByIdAsync(request.UserId);

                if (userCallback.IsFailure)
                    return userCallback.Failure;

                if (!request.Password.Equals(userCallback.Success.Password))
                    request.Password = request.Password.GenerateHash();

                Mapper.Map(request, userCallback.Success);

                return await _repository.UpdateAsync(userCallback.Success);
            }
        }
    }
}
