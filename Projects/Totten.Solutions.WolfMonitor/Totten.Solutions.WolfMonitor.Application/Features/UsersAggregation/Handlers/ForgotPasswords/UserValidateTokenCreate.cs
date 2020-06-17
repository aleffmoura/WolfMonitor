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

namespace Totten.Solutions.WolfMonitor.Application.Features.UsersAggregation.Handlers.ForgotPasswords
{
    public class UserValidateTokenCreate
    {
        public class Command : IRequest<Result<Exception, Guid>>
        {
            public string Company { get; set; }
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
                    RuleFor(a => a.Company).Length(4, 100).WithMessage("Empresa deve possuir entre 4 e 100 caracteres");
                    RuleFor(a => a.Login).Length(4, 100).WithMessage("Login deve possuir entre 4 e 100 caracteres");
                    RuleFor(a => a.Token).NotEqual(default(Guid)).WithMessage("Token da solicitação está inválido, contate o administrador");
                    RuleFor(a => a.RecoverSolicitationCode).NotEqual(default(Guid)).WithMessage("RecoverSolicitationCode está inválido, contate o administrador");
                }
            }
        }

        public class Handler : IRequestHandler<Command, Result<Exception, Guid>>
        {
            private readonly IUserRepository _repository;
            private readonly ICompanyRepository _companyRepository;

            public Handler(IUserRepository repository, ICompanyRepository companyRepository)
            {
                _companyRepository = companyRepository;
                _repository = repository;
            }

            public async Task<Result<Exception, Guid>> Handle(Command request, CancellationToken cancellationToken)
            {
                var companyCallback = await _companyRepository.GetByFantasyNameAsync(request.Company);

                if (companyCallback.IsFailure)
                    return new BusinessException(Domain.Enums.ErrorCodes.InvalidObject, "A empresa informada não foi encontrada.");

                Result<Exception, User> callback = await _repository.GetByLoginAndEmail(companyCallback.Success.Id, request.Login, request.Email);

                if (callback.IsFailure)
                    return callback.Failure;

                if (!callback.Success.Token.Equals(request.Token.ToString()))
                    return new BusinessException(Domain.Enums.ErrorCodes.InvalidObject, "O Token informado está incorreto");
                if (!callback.Success.RecoverSolicitationCode.Equals(request.RecoverSolicitationCode.ToString()))
                    return new BusinessException(Domain.Enums.ErrorCodes.InvalidObject, "Solicitação é incorreta, contate um administrador");

                callback.Success.TokenSolicitationCode = Guid.NewGuid().ToString();

                await _repository.UpdateAsync(callback.Success);

                return Guid.Parse(callback.Success.TokenSolicitationCode);
            }
        }
    }
}
