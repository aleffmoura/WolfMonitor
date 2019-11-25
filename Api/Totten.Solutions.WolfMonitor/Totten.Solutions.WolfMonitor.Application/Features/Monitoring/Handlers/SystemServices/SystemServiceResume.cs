using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Domain.Features.SystemServices;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Application.Features.Monitoring.Handlers.SystemServices
{
    public class SystemServiceResume
    {

        public class Query : IRequest<Result<Exception, SystemService>>
        {
            public Guid Id { get; set; }
            public Guid AgentId { get; set; }
            public Guid CompanyId { get; set; }

            public Query(Guid companyId, Guid agentId, Guid id)
            {
                Id = id;
                AgentId = agentId;
                CompanyId = companyId;
            }

            public ValidationResult Validate()
            {
                return new Validator().Validate(this);
            }

            private class Validator : AbstractValidator<Query>
            {
                public Validator()
                {
                    RuleFor(d => d.AgentId).NotEqual(Guid.Empty);
                    RuleFor(d => d.Id).NotEqual(Guid.Empty);
                }
            }
        }

        public class Handler : IRequestHandler<Query, Result<Exception, SystemService>>
        {
            private readonly ISystemServiceRepository _repository;
            private readonly IAgentRepository _agentRepository;

            public Handler(ISystemServiceRepository repository, IAgentRepository agentRepository)
            {
                _repository = repository;
                _agentRepository = agentRepository;
            }

            public async Task<Result<Exception, SystemService>> Handle(Query request, CancellationToken cancellationToken)
            {
                var agentCallback = await _agentRepository.GetByIdAsync(request.CompanyId, request.AgentId);

                if (agentCallback.IsFailure)
                    return new NotFoundException("Não foi encontrado um agent com o identificador informado na empresa do usuário");

                return await _repository.GetByIdAsync(request.AgentId, request.Id);
            }
        }
    }
}
