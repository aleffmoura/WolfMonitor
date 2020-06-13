using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Application.Features.Monitoring.Handlers.Items;
using Totten.Solutions.WolfMonitor.Domain.Enums;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents.Profiles;
using Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation;
using Totten.Solutions.WolfMonitor.Domain.Features.Logs;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.RabbitMQService;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;
using Unit = Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs.Unit;

namespace Totten.Solutions.WolfMonitor.Application.Features.Agents.Handlers.Profiles
{
    public class ProfileRemove
    {
        public class Command : IRequest<Result<Exception, Unit>>
        {
            public Guid UserId { get; set; }
            public Guid ProfileIdentifier { get; set; }
            public Guid CompanyId { get; set; }
            public Guid AgentId { get; set; }

            public Command(Guid userId, Guid profileIdentifier, Guid companyId, Guid agentId)
            {
                UserId = userId;
                ProfileIdentifier = profileIdentifier;
                CompanyId = companyId;
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
                    RuleFor(d => d.UserId).NotEqual(Guid.Empty).WithMessage("Id do usuário é inválido");
                    RuleFor(d => d.CompanyId).NotEqual(Guid.Empty).WithMessage("Id da empresa é inválido");
                    RuleFor(d => d.AgentId).NotEqual(Guid.Empty).WithMessage("Id do agent é inválido");
                }
            }
        }

        public class Handler : IRequestHandler<Command, Result<Exception, Unit>>
        {
            private readonly IProfileRepository _repository;
            private readonly IAgentRepository _agentRepository;
            private readonly ILogRepository _logRepository;
            private readonly IItemRepository _itemRepository;
            private readonly IUserRepository _userRepository;
            private readonly IRabbitMQ _rabbitMQ;

            public Handler(IAgentRepository agentRepository,
                           IProfileRepository repository,
                           ILogRepository logRepository,
                           IUserRepository userRepository,
                           IItemRepository itemRepository,
                           IRabbitMQ rabbitMQ)
            {
                _repository = repository;
                _agentRepository = agentRepository;
                _itemRepository = itemRepository;
                _logRepository = logRepository;
                _userRepository = userRepository;
                _rabbitMQ = rabbitMQ;
            }

            public async Task<Result<Exception, Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var agentCallback = await _agentRepository.GetByIdAsync(request.CompanyId, request.AgentId);

                if (agentCallback.IsFailure)
                    return new NotFoundException("Não foi encontrado o agent informado na empresa do usuário");

                var profilesCallback = _repository.GetAllByProfileIdentifier(request.AgentId, request.ProfileIdentifier);

                if (profilesCallback.IsFailure)
                    return profilesCallback.Failure;

                var profiles = profilesCallback.Success.ToList();

                foreach (var profile in profiles)
                {
                    profile.Removed = true;
                    await _repository.UpdateAsync(profile);
                }

                if (profiles.Count() < 0)
                    return Unit.Successful;

                Log log = new Log
                {
                    UserId = request.UserId,
                    UserCompanyId = request.CompanyId,
                    TargetId = request.ProfileIdentifier,
                    NewValue = "true",
                    OldValue = "false",
                    EntityType = ETypeEntity.AgentProfiles,
                    TypeLogMethod = ETypeLogMethod.Remove,
                    CreatedIn = DateTime.Now
                };

                await _logRepository.CreateAsync(log);

                if (request.ProfileIdentifier != Guid.Empty &&
                    agentCallback.Success.ProfileIdentifier.Equals(request.ProfileIdentifier.ToString()))
                {

                    var logAgent = new Log
                    {
                        UserId = request.UserId,
                        UserCompanyId = request.CompanyId,
                        TargetId = request.AgentId,
                        NewValue = $"{Guid.Empty}",
                        OldValue = $"{agentCallback.Success.ProfileIdentifier}",
                        EntityType = ETypeEntity.AgentProfiles,
                        TypeLogMethod = ETypeLogMethod.Remove,
                        CreatedIn = DateTime.Now
                    };

                    agentCallback.Success.ProfileName = "Sem perfil";

                    agentCallback.Success.ProfileIdentifier = Guid.Empty;

                    await _agentRepository.UpdateAsync(agentCallback.Success);

                    await _logRepository.CreateAsync(logAgent);

                    var command = new ItemSolicitationHistoricCreate.Command(request.UserId, request.AgentId, request.CompanyId,
                                        profiles.FirstOrDefault().ItemId, SolicitationType.ChangeContainsProfile, "", "", "");

                    var handle = new ItemSolicitationHistoricCreate.Handler(_itemRepository, _agentRepository, _userRepository, _rabbitMQ);

                    await handle.Handle(command, cancellationToken);
                }

                return Unit.Successful;
            }
        }
    }
}
