using MediatR;
using System;
using System.Linq;
using Totten.Solutions.WolfMonitor.Domain.Enums;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Application.Features.Agents.Handlers
{
    public class AgentCollection
    {
        public class Query : IRequest<Result<Exception, IQueryable<Agent>>>
        {
            public Guid CompanyId { get; set; }

            public Query(Guid companyId)
            {
                CompanyId = companyId;
            }
        }

        public class QueryHandler : RequestHandler<Query, Result<Exception, IQueryable<Agent>>>
        {
            private readonly IAgentRepository _repository;
            private readonly IUserRepository _userRepository;
            private readonly IItemRepository _itemRepository;

            public QueryHandler(IAgentRepository repository, IUserRepository userRepository, IItemRepository itemRepository)
            {
                _repository = repository;
                _userRepository = userRepository;
                _itemRepository = itemRepository;
            }

            protected override Result<Exception, IQueryable<Agent>> Handle(Query request)
            {
                Result<Exception, IQueryable<Agent>> agentCallback = _repository.GetAll(request.CompanyId);

                if (agentCallback.IsFailure)
                    return agentCallback.Failure;

                var agents = agentCallback.Success.ToList();

                foreach (var agent in agents)
                {
                    Result<Exception, IQueryable<Item>> itemsCallback = _itemRepository.GetAll(agent.Id);

                    if (itemsCallback.IsSuccess)
                    {
                        agent.Items.Add((int)ETypeItem.SystemService, itemsCallback.Success.Count(x => x.Type == ETypeItem.SystemService));
                        agent.Items.Add((int)ETypeItem.SystemArchive, itemsCallback.Success.Count(x => x.Type == ETypeItem.SystemArchive));
                    }
                }

                return Result<Exception, IQueryable<Agent>>.Of(agents.AsQueryable());
            }
        }
    }
}
