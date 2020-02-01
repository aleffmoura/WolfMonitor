using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Application.Features.Monitoring.Handlers.Items
{
    public class ItemResume
    {

        public class Query : IRequest<Result<Exception, Item>>
        {
            public Guid Id { get; set; }
            public Guid AgentId { get; set; }
            public Guid CompanyId { get; set; }

            public Query(Guid companyId, Guid agentId, Guid id)
            {
                Id = id;
                AgentId = agentId;
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
                    RuleFor(d => d.AgentId).NotEqual(Guid.Empty);
                    RuleFor(d => d.CompanyId).NotEqual(Guid.Empty);
                }
            }
        }

        public class Handler : IRequestHandler<Query, Result<Exception, Item>>
        {
            private readonly IItemRepository _repository;
            private readonly IAgentRepository _agentRepository;

            public Handler(IItemRepository repository, IAgentRepository agentRepository)
            {
                _repository = repository;
                _agentRepository = agentRepository;
            }

            public async Task<Result<Exception, Item>> Handle(Query request, CancellationToken cancellationToken)
            {
                var agentCallback = await _agentRepository.GetByIdAsync(request.CompanyId, request.AgentId);

                if (agentCallback.IsFailure)
                    return new NotFoundException("Não foi encontrado um agent com o identificador informado na empresa do usuário");

                return await _repository.GetByIdAsync(request.AgentId, request.Id);
            }
        }
    }
}
