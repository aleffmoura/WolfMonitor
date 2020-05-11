using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
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

            public Query(Guid companyId, Guid id)
            {
                Id = id;
                CompanyId = companyId;
            }

            public ValidationResult Validate()
            {
                return new Validator().Validate(this);
            }

            private class Validator : AbstractValidator<Query>
            {
                public Validator()
                {
                    RuleFor(d => d.Id).NotEqual(Guid.Empty);
                    RuleFor(d => d.CompanyId).NotEqual(Guid.Empty);
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
                return await _repository.GetByIdAsync(request.CompanyId, request.Id);
            }
        }
    }
}
