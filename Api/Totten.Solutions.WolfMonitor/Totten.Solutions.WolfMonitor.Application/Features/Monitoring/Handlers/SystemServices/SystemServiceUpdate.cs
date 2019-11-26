using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.Logs;
using Totten.Solutions.WolfMonitor.Domain.Features.SystemServices;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;
using Unit = Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs.Unit;

namespace Totten.Solutions.WolfMonitor.Application.Features.Monitoring.Handlers.SystemServices
{
    public class SystemServiceUpdate
    {
        public class Command : IRequest<Result<Exception, Unit>>
        {
            public Guid AgentId { get; set; }
            public string Name { get; set; }
            public string Value { get; set; }


            public Command(Guid agentId, string name, string value)
            {
                AgentId = agentId;
                Name = name;
                Value = value;
            }

            public ValidationResult Validate()
            {
                return new Validator().Validate(this);
            }

            private class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(a => a.AgentId).NotEqual(Guid.Empty);
                    RuleFor(a => a.Name).NotEmpty().Length(4, 255);
                    RuleFor(a => a.Value).NotEmpty().Length(4, 20);
                }
            }
        }

        public class Handler : IRequestHandler<Command, Result<Exception, Unit>>
        {
            private readonly ILogMonitoringRepository _logMonitoringRepository;
            private readonly ISystemServiceRepository _repository;

            public Handler(ISystemServiceRepository repository, ILogMonitoringRepository logMonitoringRepository)
            {
                _repository = repository;
                _logMonitoringRepository = logMonitoringRepository;
            }

            public async Task<Result<Exception, Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                Result<Exception, Unit> returned = Unit.Successful;
                Result<Exception, SystemService> systemServiceCallback = await _repository.GetByNameWithAgentId(request.Name, request.AgentId);

                if (systemServiceCallback.IsFailure)
                {
                    return systemServiceCallback.Failure;
                }

                MonitoringLog log = new MonitoringLog
                {
                    AgentId = request.AgentId,
                    Date = DateTime.Now,
                    Action = $"O tentou atualizar seu valor de: {systemServiceCallback.Success.Value} para: {request.Value}",
                    JsonResult = JsonConvert.SerializeObject(new BusinessException(Domain.Enums.ErrorCodes.InvalidObject, "Os valores são iguais."))
                };
                SystemService systemService = systemServiceCallback.Success;
                if (!systemService.Value.Equals(request.Value))
                {
                    Mapper.Map(request, systemService);
                    returned = await _repository.UpdateAsync(systemService);
                    log.IsSuccess = returned.IsSuccess;
                    log.JsonResult = returned.IsSuccess ? JsonConvert.SerializeObject(returned.Success) : JsonConvert.SerializeObject(returned.Failure);
                }
                await _logMonitoringRepository.CreateAsync(log);

                return returned;
            }
        }
    }
}
