using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Application.Features.Agents.Handlers
{
    public class AgentResume
    {

        public class Query : IRequest<Result<Exception, Agent>>
        {
            public Guid Id { get; set; }
            public Guid CompanyId { get; set; }
            public Guid UserId { get; set; }

            public Query(Guid id, Guid companyId, Guid userId)
            {
                Id = id;
                CompanyId = companyId;
                UserId = userId;
            }

            public ValidationResult Validate()
            {
                return new Validator().Validate(this);
            }

            private class Validator : AbstractValidator<Query>
            {
                public Validator()
                {
                    RuleFor(d => d.Id).NotEqual(Guid.Empty).WithMessage("Id do agent é invalido");
                    RuleFor(d => d.UserId).NotEqual(Guid.Empty).WithMessage("Id do usuário é invalido");
                    RuleFor(d => d.CompanyId).NotEqual(Guid.Empty).WithMessage("Id da empresa é invalido");
                }
            }
        }

        public class Handler : IRequestHandler<Query, Result<Exception, Agent>>
        {
            private readonly IAgentRepository _repository;

            public Handler(IAgentRepository repository)
            {
                _repository = repository;
            }

            public async Task<Result<Exception, Agent>> Handle(Query request, CancellationToken cancellationToken)
            {
                var agentCallback = await _repository.GetByIdAsync(request.CompanyId, request.Id);

                if (agentCallback.IsFailure)
                    return new NotFoundException("Não foi encontrado agent com o id informado na empresa do usuário");

                return agentCallback.Success;
            }
        }
    }
}
