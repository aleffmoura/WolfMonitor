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
using Totten.Solutions.WolfMonitor.Domain.Features.Companies;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.EMails;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Application.Features.UsersAggregation.Handlers.ForgotPasswords
{
    public class UserTokenReSend
    {
        public class Command : IRequest<Result<Exception, Guid>>
        {
            public string Company { get; set; }
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
                    RuleFor(a => a.Company).Length(4, 100).WithMessage("Empresa deve possuir entre 4 e 100 caracteres");
                    RuleFor(a => a.Login).Length(4, 100).WithMessage("Login deve possuir entre 4 e 100 caracteres");
                    RuleFor(a => a.Email).Length(6, 200).WithMessage("Email deve possuir entre 6 e 100 caracteres");
                }
            }
        }

        public class Handler : IRequestHandler<Command, Result<Exception, Guid>>
        {
            private readonly IUserRepository _repository;
            private readonly IEmailService _emailService;
            private readonly ICompanyRepository _companyRepository;

            public Handler(IUserRepository repository,
                           IEmailService emailService,
                           ICompanyRepository companyRepository)
            {
                _companyRepository = companyRepository;
                _repository = repository;
                _emailService = emailService;
            }

            public async Task<Result<Exception, Guid>> Handle(Command request, CancellationToken cancellationToken)
            {
                var companyCallback = await _companyRepository.GetByFantasyNameAsync(request.Company);

                if (companyCallback.IsFailure)
                    return new BusinessException(ErrorCodes.InvalidObject, "A empresa informada não foi encontrada.");

                Result<Exception, User> userCallback = await _repository.GetByLoginAndEmail(companyCallback.Success.Id, request.Login, request.Email);

                if (userCallback.IsFailure)
                    return userCallback.Failure;

                if (string.IsNullOrEmpty(userCallback.Success.Token) ||
                    string.IsNullOrEmpty(userCallback.Success.RecoverSolicitationCode))
                    return new BusinessException(ErrorCodes.NotAllowed, "Não existe uma solicitação de token valida, contate o administrador");

                var emailCallback = _emailService.Send(userCallback.Success.Email, Guid.Parse(userCallback.Success.Token));

                if (emailCallback.IsFailure)
                    return emailCallback.Failure;

                if (emailCallback.IsFailure)
                    return emailCallback.Failure;

                return Guid.Parse(userCallback.Success.RecoverSolicitationCode);
            }
        }
    }
}
