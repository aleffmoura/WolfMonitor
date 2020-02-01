using MediatR;
using System;
using System.Linq;
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

        public class QueryHandler : RequestHandler<Query, Result<Exception, IQueryable<Item>>>
        {
            private readonly IItemRepository _repository;

            public QueryHandler(IItemRepository repository)
            {
                _repository = repository;
            }

            protected override Result<Exception, IQueryable<Item>> Handle(Query request)
            {
                Result<Exception, IQueryable<Item>> Item = _repository.GetAll(request.AgentId);

                return Item;
            }
        }
    }
}
