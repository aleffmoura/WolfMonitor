using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Features.Users;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Extensions;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Application.Features.Authorizations
{
    public class AuthorizeLogin
    {
        public class Query : IRequest<Result<Exception, User>>
        {
            public string Company { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }

            public ValidationResult Validate()
            {
                return new Validator().Validate(this);
            }

            public class Validator : AbstractValidator<Query>
            {
                public Validator()
                {
                    RuleFor(u => u.UserName).NotEmpty().WithMessage("Nome de usuário não informado");

                    RuleFor(u => u.Password).NotEmpty().WithMessage("Senha não informada");

                    RuleFor(u => u.Company).NotEmpty().WithMessage("Nome de empresa não informado");
                }
            }
        }

        public class Handler : IRequestHandler<Query, Result<Exception, User>>
        {
            private IUserRepository _userRepository;

            public Handler(IUserRepository userRepository)
            {
                _userRepository = userRepository;
            }

            public async Task<Result<Exception, User>> Handle(Query request, CancellationToken cancellationToken)
            {
                var password = request.Password.GenerateHash();
                return await _userRepository.GetByCredentials(Guid.Empty, password, request.Company);
            }

        }
    }
}
