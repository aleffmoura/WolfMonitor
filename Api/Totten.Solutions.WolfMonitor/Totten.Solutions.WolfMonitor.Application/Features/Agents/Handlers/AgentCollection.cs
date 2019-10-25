using MediatR;
using System;
using System.Linq;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Application.Features.Agents.Handlers
{
    public class AgentCollection
    {
        public class Query : IRequest<Result<Exception, IQueryable<Agent>>>
        {
            public string CompanyId { get; set; }

            public Query(string companyId)
            {
                CompanyId = companyId;
            }
        }

        public class QueryHandler : RequestHandler<Query, Result<Exception, IQueryable<Agent>>>
        {
            private readonly IAgentRepository _repository;

            public QueryHandler(IAgentRepository repository)
            {
                _repository = repository;
            }

            protected override Result<Exception, IQueryable<Agent>> Handle(Query request)
            {
                var companyId = Guid.Parse(request.CompanyId);
                return _repository.GetAll(companyId);
            }
        }
    }
}
