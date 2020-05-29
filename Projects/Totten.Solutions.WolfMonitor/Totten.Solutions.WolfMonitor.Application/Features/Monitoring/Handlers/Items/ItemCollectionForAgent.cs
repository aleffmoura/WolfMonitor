using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Application.Features.Monitoring.Handlers.Items
{
    public class ItemCollectionForAgent
    {
        public class Query : IRequest<Result<Exception, IQueryable<Item>>>
        {
            public Guid AgentId { get; set; }

            public Query(Guid agentId)
            {
                AgentId = agentId;
            }
        }

        public class QueryHandler : IRequestHandler<Query, Result<Exception, IQueryable<Item>>>
        {
            private readonly IAgentRepository _agentRepository;
            private readonly IItemRepository _repository;

            public QueryHandler(IItemRepository repository, IAgentRepository agentRepository)
            {
                _repository = repository;
                _agentRepository = agentRepository;
            }

            public async Task<Result<Exception, IQueryable<Item>>> Handle(Query request, CancellationToken cancellationToken)
            {
                Result<Exception, IQueryable<Item>> Item = _repository.GetAll(request.AgentId);

                var agentCallBack = await _agentRepository.GetByIdAsync(request.AgentId);

                if (agentCallBack.IsFailure)
                    return new BusinessException(Domain.Enums.ErrorCodes.NotFound, "O id informado não pertence a nenhum agent ativo.");

                agentCallBack.Success.LastConnection = DateTime.Now;

                await _agentRepository.UpdateAsync(agentCallBack.Success);

                return Item;
            }
        }
    }
}
