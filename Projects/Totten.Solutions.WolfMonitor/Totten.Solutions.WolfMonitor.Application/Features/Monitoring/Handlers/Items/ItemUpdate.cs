using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation;
using Totten.Solutions.WolfMonitor.Domain.Features.Logs;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;
using Unit = Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs.Unit;

namespace Totten.Solutions.WolfMonitor.Application.Features.Monitoring.Handlers.Items
{
    public class ItemUpdate
    {
        public class Command : IRequest<Result<Exception, Unit>>
        {
            public Guid AgentId { get; set; }
            public string Name { get; set; }
            public string Value { get; set; }
            public string AboutCurrentValue { get; set; }
            public string LastValue { get; set; }
            public DateTime MonitoredAt { get; set; }


            public Command(Guid agentId, string name, string value, string abountCurrentValue, string lastValue, DateTime monitoredAt)
            {
                AgentId = agentId;
                Name = name;
                Value = value;
                LastValue = lastValue;
                AboutCurrentValue = abountCurrentValue;
                MonitoredAt = monitoredAt;
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
                    RuleFor(a => a.AboutCurrentValue).NotEmpty().Length(4, 150);
                }
            }
        }

        public class Handler : IRequestHandler<Command, Result<Exception, Unit>>
        {
            private readonly IItemRepository _repository;

            public Handler(IItemRepository repository)
            {
                _repository = repository;
            }

            public async Task<Result<Exception, Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                Result<Exception, Unit> returned = Unit.Successful;

                Result<Exception, Item> itemCallback = await _repository.GetByNameWithAgentId(request.Name, request.AgentId);

                if (itemCallback.IsFailure)
                    return itemCallback.Failure;

                Item itemToUpdate = itemCallback.Success;

                var lastValue = $"{itemToUpdate.Value}";

                var itemHistoric = Mapper.Map<ItemHistoric>(itemCallback.Success);

                Mapper.Map(request, itemToUpdate);

                returned = await _repository.UpdateAsync(itemToUpdate);

                if (!lastValue.Equals(request.Value) && returned.IsSuccess)
                {
                    await _repository.CreateHistoricAsync(itemHistoric);
                }

                return returned;
            }
        }
    }
}
