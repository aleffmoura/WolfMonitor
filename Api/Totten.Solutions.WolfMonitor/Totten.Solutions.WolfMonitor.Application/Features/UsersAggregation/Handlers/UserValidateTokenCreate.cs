using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Application.Features.UsersAggregation.Handlers
{
    public class UserValidateTokenCreate
    {
        public class Command : IRequest<Result<Exception, string>>
        {
            public string Token { get; set; }
            public string Login { get; set; }
            public string Email { get; set; }
            public string RecoverSolicitationCode { get; set; }

            public ValidationResult Validate()
            {
                return new Validator().Validate(this);
            }

            private class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(a => a.Token).NotEmpty().Length(4, 100);
                    RuleFor(a => a.Login).NotEmpty().Length(4, 100);
                    RuleFor(a => a.Email).NotEmpty().Length(6, 200);
                    RuleFor(a => a.RecoverSolicitationCode).NotEmpty().Length(4, 100);
                }
            }
        }

        public class Handler : IRequestHandler<Command, Result<Exception, string>>
        {
            private readonly IMapper _mapper;
            private readonly IUserRepository _repository;

            public Handler(IMapper mapper, IUserRepository repository)
            {
                _mapper = mapper;
                _repository = repository;
            }

            public async Task<Result<Exception, string>> Handle(Command request, CancellationToken cancellationToken)
            {
                Result<Exception, User> callback = await _repository.GetByLoginAndEmail(request.Login, request.Email);

                if (callback.IsFailure)
                {
                    return callback.Failure;
                }

                if(!callback.Success.Token.Equals(request.Token))
                    return new Exception("O Token informado está incorreto");
                if(!callback.Success.RecoverSolicitationCode.Equals(request.RecoverSolicitationCode))
                    return new Exception("Solicitação é incorreta, contate um administrador");

                /*
                 * aqui enviara o token para o usuario
                 * 
                */

                callback.Success.TokenSolicitationCode = Guid.NewGuid().ToString();

                await _repository.UpdateAsync(callback.Success);

                return callback.Success.TokenSolicitationCode;
            }
        }
    }
}
