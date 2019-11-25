using MediatR;
using System;
using System.Linq;
using Totten.Solutions.WolfMonitor.Domain.Features.SystemServices;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Application.Features.Monitoring.Handlers.SystemServices
{
    public class SystemServiceCollectionForAgent
    {
        public class Query : IRequest<Result<Exception, IQueryable<SystemService>>>
        {
            public Guid AgentId { get; set; }

            public Query(Guid agentId)
            {
                AgentId = agentId;
            }
        }

        public class QueryHandler : RequestHandler<Query, Result<Exception, IQueryable<SystemService>>>
        {
            private readonly ISystemServiceRepository _repository;

            public QueryHandler(ISystemServiceRepository repository)
            {
                _repository = repository;
            }

            protected override Result<Exception, IQueryable<SystemService>> Handle(Query request)
            {
                Result<Exception, IQueryable<SystemService>> systemService = _repository.GetAll(request.AgentId);

                return systemService;
            }
        }
    }
}
