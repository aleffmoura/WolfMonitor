using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;
using Unit = Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs.Unit;

namespace Totten.Solutions.WolfMonitor.Application.Features.Agents.Handlers
{
    public class AgentUpdate
    {
        public class Command : IRequest<Result<Exception, Unit>>
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string LocalIp { get; set; }
            public string HostName { get; set; }
            public string HostAddress { get; set; }


            public Command(Guid id, string name, string localIp, string hostName, string hostAddress)
            {
                Id = id;
                Name = name;
                LocalIp = localIp;
                HostName = hostName;
                HostAddress = hostAddress;
            }

            public ValidationResult Validate()
            {
                return new Validator().Validate(this);
            }

            private class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(a => a.Id).NotEqual(Guid.Empty);
                    RuleFor(a => a.Name).NotEmpty().Length(4, 100);
                    RuleFor(a => a.LocalIp).NotEmpty();
                    RuleFor(a => a.HostName).NotEmpty().Length(4, 100);
                    RuleFor(a => a.HostAddress).NotEmpty().Length(4, 100);
                }
            }
        }

        public class Handler : IRequestHandler<Command, Result<Exception, Unit>>
        {
            private readonly IMapper _mapper;
            private readonly IAgentRepository _repository;

            public Handler(IMapper mapper, IAgentRepository repository)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<Result<Exception, Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                Result<Exception, Agent> agentCallback = await _repository.GetByIdAsync(request.Id);


                if (agentCallback.IsFailure)
                {
                    return agentCallback.Failure;
                }

                Agent agent = agentCallback.Success;
                agent.Configured = true;
                _mapper.Map(request, agent);

                return await _repository.UpdateAsync(agent);
            }
        }
    }
}
