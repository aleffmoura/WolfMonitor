using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Application.Features.Services;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Extensions;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;
using Unit = Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs.Unit;

namespace Totten.Solutions.WolfMonitor.Application.Features.UsersAggregation.Handlers.ForgotPasswords
{
    public class UserChangePasswordByTokenCreate
    {
        public class Command : IRequest<Result<Exception, Unit>>
        {
            public string Login { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public Guid TokenSolicitationCode { get; set; }
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
                    RuleFor(a => a.Email).NotEmpty().Length(6, 200);
                    RuleFor(a => a.Password).NotEmpty().Length(8, 150);
                    RuleFor(a => a.TokenSolicitationCode).NotEmpty().NotEqual(default(Guid));
                    RuleFor(a => a.RecoverSolicitationCode).NotEqual(default(Guid)); ;
                }
            }
        }

        public class Handler : IRequestHandler<Command, Result<Exception, Unit>>
        {
            private readonly IUserRepository _repository;
            private readonly IEmailService _emailService;

            public Handler(IUserRepository repository, IEmailService emailService)
            {
                _repository = repository;
                _emailService = emailService;
            }

            public async Task<Result<Exception, Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                Result<Exception, User> callback = await _repository.GetByLoginAndEmail(request.Login, request.Email);

                if (callback.IsFailure)
                    return callback.Failure;

                var emailCallback = _emailService.Send(callback.Success.Email);

                if (emailCallback.IsFailure)
                    return emailCallback.Failure;

                if (!callback.Success.TokenSolicitationCode.Equals(request.TokenSolicitationCode.ToString()))
                    return new BusinessException(Domain.Enums.ErrorCodes.InvalidObject, "O Token da solicitação não está correto com a requisição, contate um administrador");
                if (!callback.Success.RecoverSolicitationCode.Equals(request.RecoverSolicitationCode.ToString()))
                    return new BusinessException(Domain.Enums.ErrorCodes.InvalidObject, "O Codigo de recuperação da solicitação não está correto com a requisição, contate um administrador");

                callback.Success.Password = request.Password.GenerateHash();
                callback.Success.Token = string.Empty;
                callback.Success.TokenSolicitationCode = string.Empty;
                callback.Success.RecoverSolicitationCode = string.Empty;

                await _repository.UpdateAsync(callback.Success);

                return Unit.Successful;
            }
        }
    }
}
