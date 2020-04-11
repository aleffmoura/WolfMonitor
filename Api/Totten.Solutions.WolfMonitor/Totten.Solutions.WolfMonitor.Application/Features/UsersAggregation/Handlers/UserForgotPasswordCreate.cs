using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Application.Features.Services;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Application.Features.UsersAggregation.Handlers
{
    public class UserForgotPasswordCreate
    {
        public class Command : IRequest<Result<Exception, string>>
        {
            public string Login { get; set; }
            public string Email { get; set; }

            public ValidationResult Validate()
            {
                return new Validator().Validate(this);
            }

            private class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(a => a.Login).NotEmpty().Length(4, 100);
                    RuleFor(a => a.Email).NotEmpty().Length(6, 200);
                }
            }
        }

        public class Handler : IRequestHandler<Command, Result<Exception, string>>
        {
            private readonly IUserRepository _repository;
            private readonly IEmailService _emailService;

            public Handler(IUserRepository repository, IEmailService emailService)
            {
                _repository = repository;
                _emailService = emailService;
            }

            public async Task<Result<Exception, string>> Handle(Command request, CancellationToken cancellationToken)
            {
                Result<Exception, User> callback = await _repository.GetByLoginAndEmail(request.Login, request.Email);

                if (callback.IsFailure)
                {
                    return callback.Failure;
                }

                var emailCallback = _emailService.Send(callback.Success.Email);

                if (emailCallback.IsFailure)
                    return emailCallback.Failure;

                callback.Success.Token = Guid.NewGuid().ToString();
                callback.Success.RecoverSolicitationCode = Guid.NewGuid().ToString();

                await _repository.UpdateAsync(callback.Success);

                return callback.Success.RecoverSolicitationCode;
            }
        }
    }
}
