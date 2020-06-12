using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;
using User = Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation.User;

namespace Totten.Solutions.WolfMonitor.Application.Features.UsersAggregation.Handlers
{
    public class UserResume
    {
        public class Query : IRequest<Result<Exception, User>>
        {
            public Guid Id { get; set; }

            public Query(Guid id)
            {
                Id = id;
            }

            public ValidationResult Validate()
            {
                return new Validator().Validate(this);
            }

            private class Validator : AbstractValidator<Query>
            {
                public Validator()
                {
                    RuleFor(d => d.Id).NotEqual(Guid.Empty).WithMessage("Id do usuário a ser deletado está invalido");
                }
            }
        }

        public class Handler : IRequestHandler<Query, Result<Exception, User>>
        {
            private readonly IUserRepository _repository;

            public Handler(IUserRepository repository)
            {
                _repository = repository;
            }

            public async Task<Result<Exception, User>> Handle(Query request, CancellationToken cancellationToken)
            {
                var userCallback = await _repository.GetByIdAsync(request.Id);

                if (userCallback.IsFailure)
                    return new NotFoundException("Não foi encontrado nenhum usuário com o id informado.");

                return userCallback;
            }
        }
    }
}
