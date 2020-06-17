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
            public string MachineName { get; set; }
            public string LocalIp { get; set; }
            public string HostName { get; set; }
            public string HostAddress { get; set; }

            public Command(Guid id, string machineName, string localIp, string hostName, string hostAddress)
            {
                Id = id;
                MachineName = machineName;
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
                    RuleFor(a => a.Id).NotEqual(Guid.Empty).WithMessage("Identificador da empresa é inválido");
                    RuleFor(a => a.MachineName).Length(1, 100).WithMessage("Nome da maquina deve possuir entre 1 e 100 caracteres");
                    RuleFor(a => a.LocalIp).NotEmpty().WithMessage("IP não pode ser em branco em nulo");
                    RuleFor(a => a.HostName);
                    RuleFor(a => a.HostAddress).Length(4, 100).WithMessage("Endereço do host não pode ser em branco ou nulo");
                }
            }
        }

        public class Handler : IRequestHandler<Command, Result<Exception, Unit>>
        {
            private readonly IAgentRepository _repository;

            public Handler( IAgentRepository repository)
            {
                _repository = repository;
            }

            public async Task<Result<Exception, Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                if(string.IsNullOrEmpty(request.HostName))
                    request.HostName = "Sem hostname";

                Result<Exception, Agent> agentCallback = await _repository.GetByIdAsync(request.Id);


                if (agentCallback.IsFailure)
                {
                    return agentCallback.Failure;
                }

                Agent agent = agentCallback.Success;
                agent.Configured = true;
                Mapper.Map(request, agent);

                return await _repository.UpdateAsync(agent);
            }
        }
    }
}
