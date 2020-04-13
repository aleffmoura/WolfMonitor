using MediatR;
using System;
using System.Linq;
using Totten.Solutions.WolfMonitor.Domain.Enums;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Application.Features.Monitoring.Handlers.Items
{
    public class ItemCollection
    {
        public class Query : IRequest<Result<Exception, IQueryable<Item>>>
        {
            public Guid AgentId { get; set; }
            public Guid CompanyId { get; set; }
            public ETypeItem Type { get; set; }


            public Query(Guid agentId, Guid companyId, ETypeItem type)
            {
                AgentId = agentId;
                CompanyId = companyId;
                Type = type;
            }
        }

        public class QueryHandler : RequestHandler<Query, Result<Exception, IQueryable<Item>>>
        {
            private readonly IItemRepository _repository;
            private readonly IAgentRepository _agentRepository;

            public QueryHandler(IItemRepository repository, IAgentRepository agentRepository)
            {
                _repository = repository;
                _agentRepository = agentRepository;
            }

            protected override Result<Exception, IQueryable<Item>> Handle(Query request)
            {
                Result<Exception, Agent> agentCallback = _agentRepository.GetByIdAsync(request.CompanyId, request.AgentId).ConfigureAwait(false).GetAwaiter().GetResult();

                if (agentCallback.IsFailure)
                    return new NotFoundException("Não foi encontrado um agent com o identificador informado na empresa do usuário");

                Result<Exception, IQueryable<Item>> Item = _repository.GetAll(request.AgentId, request.Type);

                return Item;
            }
        }
    }
}
