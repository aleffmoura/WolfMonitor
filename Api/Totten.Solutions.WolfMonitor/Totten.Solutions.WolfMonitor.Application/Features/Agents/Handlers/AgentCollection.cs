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
            public Guid CompanyId { get; set; }

            public Query(Guid companyId)
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
                Result<Exception, IQueryable<Agent>> agents =  _repository.GetAll(request.CompanyId);


                return agents;
            }
        }
    }
}
