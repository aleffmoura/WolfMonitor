using MediatR;
using System;
using System.Linq;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Domain.Features.SystemServices;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Application.Features.Monitoring.Handlers.SystemServices
{
    public class SystemServiceCollection
    {
        public class Query : IRequest<Result<Exception, IQueryable<SystemService>>>
        {
            public Guid AgentId { get; set; }
            public Guid CompanyId { get; set; }

            public Query(Guid agentId, Guid companyId)
            {
                AgentId = agentId;
                CompanyId = companyId;
            }
        }

        public class QueryHandler : RequestHandler<Query, Result<Exception, IQueryable<SystemService>>>
        {
            private readonly ISystemServiceRepository _repository;
            private readonly IAgentRepository _agentRepository;

            public QueryHandler(ISystemServiceRepository repository, IAgentRepository agentRepository)
            {
                _repository = repository;
                _agentRepository = agentRepository;
            }

            protected override Result<Exception, IQueryable<SystemService>> Handle(Query request)
            {
                Result<Exception, Agent> agentCallback = _agentRepository.GetByIdAsync(request.CompanyId, request.AgentId).ConfigureAwait(false).GetAwaiter().GetResult();

                if (agentCallback.IsFailure)
                {
                    return new NotFoundException("Não foi encontrado um agent com o identificador informado na empresa do usuário");
                }

                Result<Exception, IQueryable<SystemService>> systemService = _repository.GetAll(request.AgentId);

                return systemService;
            }
        }
    }
}
