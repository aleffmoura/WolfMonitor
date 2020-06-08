using MediatR;
using System;
using System.Linq;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents.Profiles;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Application.Features.Agents.Handlers.Profiles
{
    public class ProfileCollection
    {
        public class Query : IRequest<Result<Exception, IQueryable<Profile>>>
        {
            public Guid AgentId { get; set; }

            public Query(Guid agentId)
            {
                AgentId = agentId;
            }
        }

        public class QueryHandler : RequestHandler<Query, Result<Exception, IQueryable<Profile>>>
        {
            private readonly IProfileRepository _repository;

            public QueryHandler(IProfileRepository repository)
            {
                _repository = repository;
            }

            protected override Result<Exception, IQueryable<Profile>> Handle(Query request)
            {
                var callback = _repository.GetAllByAgentId(request.AgentId);
                return callback;
            }
        }
    }
}
