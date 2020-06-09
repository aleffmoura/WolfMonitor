using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents.Profiles;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Application.Features.Agents.Handlers.Profiles
{
    public class ProfileCollection
    {
        public class Query : IRequest<Result<Exception, IQueryable<Profile>>>
        {
            public Guid UserId { get; set; }
            public Guid CompanyId { get; set; }
            public Guid AgentId { get; set; }

            public Query(Guid userId, Guid companyId, Guid agentId)
            {
                UserId = userId;
                CompanyId = companyId;
                AgentId = agentId;
            }
        }

        public class QueryHandler : RequestHandler<Query, Result<Exception, IQueryable<Profile>>>
        {
            private readonly IProfileRepository _repository;
            private readonly IAgentRepository _agentRepository;

            public QueryHandler(IProfileRepository repository, IAgentRepository agentRepository)
            {
                _repository = repository;
                _agentRepository = agentRepository;
            }

            protected override Result<Exception, IQueryable<Profile>> Handle(Query request)
            {
                var agentCallback = _agentRepository.GetByIdAsync(request.AgentId).ConfigureAwait(false).GetAwaiter().GetResult();

                if (agentCallback.IsFailure)
                    return agentCallback.Failure;

                if (agentCallback.Success.CompanyId != request.CompanyId)
                    return new BusinessException(Domain.Enums.ErrorCodes.NotAllowed, "Usuário não permitido a criar perfil no agent informado");

                return _repository.GetAllByAgentId(request.AgentId);
            }
        }
    }
}
