using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Application.Features.ItemsAggregation.Handlers.Services
{
    public class ServicesResume
    {
        public class Query : IRequest<Result<Exception, Item>>
        {
            public string _agentId { get; set; }

            public Query(string agentId)
            {
                _agentId = agentId;
            }

            public ValidationResult Validate()
            {
                return new Validator().Validate(this);
            }

            private class Validator : AbstractValidator<Query>
            {
                public Validator()
                {
                    RuleFor(d => d._agentId).NotEmpty().Length(36);
                }
            }
        }

        public class Handler : IRequestHandler<Query, Result<Exception, Item>>
        {
            private readonly IItemRepository _repository;

            public Handler(IItemRepository repository)
            {
                _repository = repository;
            }

            public async Task<Result<Exception, Item>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _repository.GetByAgentIdAsync(Guid.Parse(request._agentId));
            }
        }
    }
}
