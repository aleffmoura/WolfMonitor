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
using Totten.Solutions.WolfMonitor.Domain.Enums;
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
            public Guid UserId { get; set; }
            public Guid CompanyId { get; set; }


            public Command(Guid agentId, Guid id, Guid userId, Guid companyId)
            {
                Id = id;
                AgentId = agentId;
                UserId = userId;
                CompanyId = companyId;
            }

            public ValidationResult Validate()
            {
                return new Validator().Validate(this);
            }

            private class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(a => a.Id).NotEqual(Guid.Empty).WithMessage("Identificador do item é inválido");
                    RuleFor(a => a.AgentId).NotEqual(Guid.Empty).WithMessage("Identificador do agent é inválido");
                    RuleFor(a => a.UserId).NotEqual(Guid.Empty).WithMessage("Identificador do usuário é inválido");
                    RuleFor(a => a.CompanyId).NotEqual(Guid.Empty).WithMessage("Identificador da empresa é inválido");
                }
            }
        }

        public class Handler : IRequestHandler<Command, Result<Exception, Unit>>
        {
            private readonly IItemRepository _repository;
            private readonly ILogRepository _logRepository;

            public Handler(IItemRepository repository, ILogRepository logRepository)
            {
                _repository = repository;
                _logRepository = logRepository;
            }

            public async Task<Result<Exception, Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                Result<Exception, Item> ItemCallback = await _repository.GetByIdAsync(request.AgentId, request.Id);

                if (ItemCallback.IsFailure)
                    return ItemCallback.Failure;

                if (ItemCallback.Success.CompanyId != request.CompanyId)
                    return new ForbiddenException("Usuário não autorizado a deletar um item no agent informado.");

                ItemCallback.Success.Removed = true;

                var itemUpdatedCallback = await _repository.UpdateAsync(ItemCallback.Success);

                if (itemUpdatedCallback.IsFailure)
                    return itemUpdatedCallback.Failure;

                Log log = new Log
                {
                    UserId = request.UserId,
                    UserCompanyId = request.CompanyId,
                    TargetId = ItemCallback.Success.Id,
                    EntityType = ETypeEntity.Monitoring,
                    TypeLogMethod = ETypeLogMethod.Remove,
                    CreatedIn = DateTime.Now
                };

                await _logRepository.CreateAsync(log);

                return itemUpdatedCallback.Success;
            }
        }
    }
}
