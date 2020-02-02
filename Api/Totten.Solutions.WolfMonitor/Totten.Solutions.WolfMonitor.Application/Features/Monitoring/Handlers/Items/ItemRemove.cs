using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation;
using Totten.Solutions.WolfMonitor.Domain.Features.Logs;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;
using Unit = Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs.Unit;

namespace Totten.Solutions.WolfMonitor.Application.Features.Monitoring.Handlers.Items
{
    public class ItemRemove
    {
        public class Command : IRequest<Result<Exception, Unit>>
        {
            public Guid Id { get; set; }
            public Guid AgentId { get; set; }


            public Command(Guid agentId, Guid id)
            {
                Id = id;
                AgentId = agentId;
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
                    RuleFor(a => a.AgentId).NotEqual(Guid.Empty);
                }
            }
        }

        public class Handler : IRequestHandler<Command, Result<Exception, Unit>>
        {
            private readonly IMapper _mapper;
            private readonly IItemRepository _repository;
            //private readonly ILogMonitoringRepository _logMonitoringRepository;

            public Handler(IMapper mapper, IItemRepository repository/*, ILogMonitoringRepository logMonitoringRepository*/)
            {
                _mapper = mapper;
                _repository = repository;
                //_logMonitoringRepository = logMonitoringRepository;
            }

            public async Task<Result<Exception, Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                Result<Exception, Unit> returned = Unit.Successful;
                Result<Exception, Item> ItemCallback = await _repository.GetByIdAsync(request.AgentId, request.Id);

                if (ItemCallback.IsFailure)
                {
                    return ItemCallback.Failure;
                }

                Item Item = ItemCallback.Success;

                _mapper.Map(request, Item);
                Item.Removed = true;
                returned = await _repository.UpdateAsync(Item);

                //await _logMonitoringRepository.CreateAsync(log);

                return returned;
            }
        }
    }
}
