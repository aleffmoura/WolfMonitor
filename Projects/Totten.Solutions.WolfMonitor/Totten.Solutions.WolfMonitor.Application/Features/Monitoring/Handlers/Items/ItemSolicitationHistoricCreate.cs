using AutoMapper;
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
            public string NewValue { get; set; }

            public Command(Guid userId,
                           Guid agentId,
                           Guid companyId,
                           Guid itemId,
                           SolicitationType solicitationType,
                           string name,
                           string displayName,
                           string newValue)
            {
                UserId = userId;
                AgentId = agentId;
                CompanyId = companyId;
                ItemId = itemId;
                SolicitationType = solicitationType;
                Name = name;
                DisplayName = displayName;
                NewValue = newValue;
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
                    RuleFor(a => a.ItemId).NotEqual(Guid.Empty)
                        .WithMessage("Identificador do item é invalido");
                    RuleFor(a => a.Name).Length(4, 250).WithMessage("Nome deve conter entre 4 e 250 caracteres");
                    RuleFor(a => a.DisplayName).Length(4, 250).WithMessage("Nome de exibição deve conter entre 4 e 250 caracteres"); ;
                    RuleFor(a => a.NewValue).MinimumLength(1).WithMessage("Novo valor deve conter no minimo 1 caracter"); ;
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

                var item = await _repository.GetByIdAsync(request.AgentId, request.ItemId);

                if (item.IsFailure)
                    return item.Failure;
                
                ItemSolicitationHistoric itemSolicitation = Mapper.Map<Command, ItemSolicitationHistoric>(request);
                itemSolicitation.ItemId = item.Success.Id;

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
