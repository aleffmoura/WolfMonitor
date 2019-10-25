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
    public class ClientUpdate
    {
        public class Command : IRequest<Result<Exception, Unit>>
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string LocalIp { get; set; }
            public string HostName { get; set; }
            public string HostAddress { get; set; }

            public ValidationResult Validate()
            {
                return new Validator().Validate(this);
            }

            private class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(a => a.Id).NotEmpty().Length(36);
                }
            }
        }

        public class Handler : IRequestHandler<Command, Result<Exception, Unit>>
        {
            private readonly IAgentRepository _repository;

            public Handler(IAgentRepository repository)
            {
                _repository = repository;
            }

            public async Task<Result<Exception, Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var id = Guid.Parse(request.Id);
                var doctorId = Guid.Parse(request.Id);

                var agentCallback = await _repository.GetByIdAsync(id);


                if (agentCallback.IsFailure)
                    return agentCallback.Failure;

                var agent = agentCallback.Success;

                Mapper.Map(request, agent);

                return await _repository.UpdateAsync(agent);
            }
        }
    }
}
