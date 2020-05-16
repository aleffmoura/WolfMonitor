﻿using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Application.Features.UsersAggregation.Handlers.ForgotPasswords
{
    public class UserValidateTokenCreate
    {
        public class Command : IRequest<Result<Exception, Guid>>
        {
            public string Login { get; set; }
            public string Email { get; set; }
            public Guid Token { get; set; }
            public Guid RecoverSolicitationCode { get; set; }

            public ValidationResult Validate()
            {
                return new Validator().Validate(this);
            }

            private class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(a => a.Login).NotEmpty().Length(4, 100);
                    RuleFor(a => a.Token).NotEmpty().NotEqual(default(Guid));
                    RuleFor(a => a.RecoverSolicitationCode).NotEqual(default(Guid)); ;
                }
            }
        }

        public class Handler : IRequestHandler<Command, Result<Exception, Guid>>
        {
            private readonly IUserRepository _repository;

            public Handler(IUserRepository repository)
            {
                _repository = repository;
            }

            public async Task<Result<Exception, Guid>> Handle(Command request, CancellationToken cancellationToken)
            {
                Result<Exception, User> callback = await _repository.GetByLoginAndEmail(request.Login, request.Email);

                if (callback.IsFailure)
                    return callback.Failure;

                if(!callback.Success.Token.Equals(request.Token.ToString()))
                    return new BusinessException(Domain.Enums.ErrorCodes.InvalidObject,"O Token informado está incorreto");
                if(!callback.Success.RecoverSolicitationCode.Equals(request.RecoverSolicitationCode.ToString()))
                    return new BusinessException(Domain.Enums.ErrorCodes.InvalidObject, "Solicitação é incorreta, contate um administrador");

                callback.Success.TokenSolicitationCode = Guid.NewGuid().ToString();

                await _repository.UpdateAsync(callback.Success);

                return Guid.Parse(callback.Success.TokenSolicitationCode);
            }
        }
    }
}