﻿using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.RabbitMQService;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;
using Unit = Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs.Unit;

namespace Totten.Solutions.WolfMonitor.Application.Features.Monitoring.Handlers.Items
{
    public class ItemSolicitationHistoricCreate
    {
        public class Command : IRequest<Result<Exception, Unit>>
        {
            public Guid ItemId { get; set; }
            public Guid UserId { get; set; }
            public Guid AgentId { get; set; }
            public Guid CompanyId { get; set; }
            public SolicitationType SolicitationType { get; set; }
            public string Name { get; set; }
            public string DisplayName { get; set; }
            public string NewStatus { get; set; }

            public Command(Guid userId,
                           Guid agentId,
                           Guid companyId,
                           SolicitationType solicitationType,
                           string name,
                           string displayName,
                           string newStatus)
            {
                UserId = userId;
                AgentId = agentId;
                CompanyId = companyId;
                SolicitationType = solicitationType;
                Name = name;
                DisplayName = displayName;
                NewStatus = newStatus;
            }

            public ValidationResult Validate()
            {
                return new Validator().Validate(this);
            }

            private class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(a => a.AgentId).NotEqual(Guid.Empty)
                        .WithMessage("Identificador do agent é invalido");
                    RuleFor(a => a.CompanyId).NotEqual(Guid.Empty)
                        .WithMessage("Identificador da empresa é invalido");
                    RuleFor(a => a.UserId).NotEqual(Guid.Empty)
                        .WithMessage("Identificador do usuario ao qual esta adicionando o serviço é invalido");
                    RuleFor(a => a.Name).NotEmpty().Length(4, 250);
                    RuleFor(a => a.DisplayName).NotEmpty().Length(4, 250);
                    RuleFor(a => a.NewStatus).NotEmpty().Length(1, 250);
                }
            }
        }

        public class Handler : IRequestHandler<Command, Result<Exception, Unit>>
        {
            private IRabbitMQ _rabbitMQ;
            private readonly IItemRepository _repository;
            private readonly IAgentRepository _agentRepository;
            private readonly IUserRepository _userRepository;

            public Handler(IItemRepository repository,
                           IAgentRepository agentRepository,
                           IUserRepository userRepository,
                           IRabbitMQ rabbitMQ)
            {
                _repository = repository;
                _agentRepository = agentRepository;
                _userRepository = userRepository;
                _rabbitMQ = rabbitMQ;
            }

            public async Task<Result<Exception, Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var agentCallback = await _agentRepository.GetByIdAsync(request.AgentId);

                if (agentCallback.IsFailure)
                    return agentCallback.Failure;

                if (agentCallback.Success.CompanyId != request.CompanyId)
                    return new NotAllowedException("Usuário não pode salvar serviços no agent informado, a empresa do usuario e do agent não são iguais");

                var userCallback = await _userRepository.GetByIdAsync(request.UserId);

                if (userCallback.IsFailure)
                    return userCallback.Failure;

                if (userCallback.Success.CompanyId != agentCallback.Success.CompanyId)
                    return new NotAllowedException("Usuário não pode salvar serviços no agent informado, a empresa do usuario e do agent não são iguais");

                var item = await _repository.GetByNameWithAgentId(request.Name, request.AgentId);

                if (item.IsFailure)
                    return item.Failure;

                ItemSolicitationHistoric itemSolicitation = Mapper.Map<Command, ItemSolicitationHistoric>(request);
                itemSolicitation.ItemId = item.Success.Id;

                if (request.SolicitationType == SolicitationType.ChangeStatus)
                    itemSolicitation.NewValue = item.Success.Value.Equals("Running", StringComparison.OrdinalIgnoreCase) ? "Stopped" : "Running";
                
                var callback = await _repository.CreateSolicitationAsync(itemSolicitation);

                if (callback.IsFailure)
                    return callback.Failure;

                request.ItemId = item.Success.Id;

                _rabbitMQ.RouteMessage(channel: request.AgentId.ToString(), request);

                return Unit.Successful;
            }
        }
    }
}