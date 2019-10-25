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
            public string Id { get; set; }

            public Query(string id)
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
                    RuleFor(d => d.Id).NotEmpty().Length(36);
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
                var id = Guid.Parse(request.Id);

                return await _repository.GetByIdAsync(id);
            }
        }
    }
}
