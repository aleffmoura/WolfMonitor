using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Application.Features.Services;
using Totten.Solutions.WolfMonitor.Domain.Enums;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.EMails;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Application.Features.UsersAggregation.Handlers.ForgotPasswords
{
    public class UserTokenReSend
    {
        public class Command : IRequest<Result<Exception, Guid>>
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

        public class Handler : IRequestHandler<Command, Result<Exception, Guid>>
        {
            private readonly IUserRepository _repository;
            private readonly IEmailService _emailService;

            public Handler(IUserRepository repository, IEmailService emailService)
            {
                _repository = repository;
                _emailService = emailService;
            }

            public async Task<Result<Exception, Guid>> Handle(Command request, CancellationToken cancellationToken)
            {
                Result<Exception, User> callback = await _repository.GetByLoginAndEmail(request.Login, request.Email);

                if (callback.IsFailure)
                {
                    return callback.Failure;
                }

                var emailCallback = _emailService.Send(callback.Success.Email);

                if (emailCallback.IsFailure)
                    return emailCallback.Failure;

                if (string.IsNullOrEmpty(callback.Success.Token) ||
                    string.IsNullOrEmpty(callback.Success.RecoverSolicitationCode))
                    return new BusinessException(ErrorCodes.NotAllowed, "Não existe uma solicitação de token valida, contate o administrador");

                try
                {
                    var body = "Recuperação de senha:<br/> Nome: Totem Solutions<br/> Token : " + callback.Success.Token + " <br/>Mensagem automática, não responda-a";

                    EMail.Send("Recuperação de senha", body, callback.Success.Email, "Totem Solutions", "tottenprogramming@gmail.com");
                }
                catch
                {
                    return new BusinessException(ErrorCodes.ServiceUnavailable, "Serviço indisponivel, contact o administrador");
                }

                return Guid.Parse(callback.Success.RecoverSolicitationCode);
            }
        }
    }
}
