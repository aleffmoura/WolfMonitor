using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Profile = Totten.Solutions.WolfMonitor.Domain.Features.Agents.Profiles.Profile;

namespace Totten.Solutions.WolfMonitor.Application.Features.Agents.Handlers.Profiles
{
    public class ProfileApply
    {
        public class Command : IRequest<Result<Exception, Guid>>
        {
            public Guid ProfileIdentifier { get; set; }
            public Guid AgentId { get; set; }
            public Guid CompanyId { get; set; }
            public Guid UserId { get; set; }

            public Command(Guid profileIdentifier,
                           Guid agentId,
                           Guid companyId,
                           Guid userId)
            {
                ProfileIdentifier = profileIdentifier;
                AgentId = agentId;
                CompanyId = companyId;
                UserId = userId;
            }

            public ValidationResult Validate()
            {
                return new Validator().Validate(this);
            }

            private class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(a => a.CompanyId).NotEqual(Guid.Empty)
                        .WithMessage("Identificador da empresa é inválido");
                    RuleFor(a => a.UserId).NotEqual(Guid.Empty)
                        .WithMessage("Identificador do usuário ao qual esta aplicando o perfil é inválido");
                    RuleFor(a => a.AgentId).NotEqual(Guid.Empty)
                        .WithMessage("Identificador do usuário ao qual esta aplicando o perfil é inválido");
                }
            }
        }

        public class Handler : IRequestHandler<Command, Result<Exception, Guid>>
        {
            private readonly IProfileRepository _repository;
            private readonly IAgentRepository _agentRepository;
            private readonly IUserRepository _userRepository;
            private readonly IItemRepository _itemRepository;
            private readonly ILogRepository _logRepository;
            private readonly IRabbitMQ _rabbitMQ;

            public Handler(IProfileRepository repository,
                           IAgentRepository agentRepository,
                           IUserRepository userRepository,
                           IItemRepository itemRepository,
                           ILogRepository logRepository,
                           IRabbitMQ rabbitMQ)
            {
                _repository = repository;
                _agentRepository = agentRepository;
                _userRepository = userRepository;
                _itemRepository = itemRepository;
                _logRepository = logRepository;
                _rabbitMQ = rabbitMQ;
            }

            public async Task<Result<Exception, Guid>> Handle(Command request, CancellationToken cancellationToken)
            {
                Result<Exception, Agent> agentVerify = await _agentRepository.GetByIdAsync(request.CompanyId, request.AgentId);

                if (agentVerify.IsFailure)
                    return new DuplicateException("Agent não encontrado na empresa do usuário.");

                var userCallback = await _userRepository.GetByIdAsync(request.UserId);

                if (userCallback.IsFailure)
                    return new NotFoundException("Não foi encontrado um usúario com o id da requisição.");

                var profileCallback = _repository.GetAllByProfileIdentifier(request.AgentId, request.ProfileIdentifier);

                if (profileCallback.IsFailure)
                    return profileCallback.Failure;

                var items = _itemRepository.GetAll(request.AgentId);

                if (items.IsFailure)
                    return items.Failure;

                #region Atualizando Profile no Agent
                var profiles = profileCallback.Success.ToList();

                agentVerify.Success.ProfileIdentifier = request.ProfileIdentifier;
                agentVerify.Success.ProfileName = request.ProfileIdentifier != Guid.Empty ? profiles.FirstOrDefault().Name : "Sem perfil";

                var updatedAgent = await _agentRepository.UpdateAsync(agentVerify.Success);

                if (updatedAgent.IsFailure)
                    return updatedAgent.Failure;

                var logProfile = new Log
                {
                    UserId = request.UserId,
                    UserCompanyId = request.CompanyId,
                    TargetId = agentVerify.Success.Id,
                    NewValue = request.ProfileIdentifier != Guid.Empty ? $"{profiles.FirstOrDefault().ProfileIdentifier}" : "Sem perfil",
                    OldValue = $"{agentVerify.Success.ProfileIdentifier}",
                    EntityType = ETypeEntity.AgentProfiles,
                    TypeLogMethod = ETypeLogMethod.Apply,
                    CreatedIn = DateTime.Now
                };

                await _logRepository.CreateAsync(logProfile);
                #endregion

                #region Atualizando Items

                Guid itemId = Guid.Empty;

                foreach (var item in items.Success.ToList())
                {
                    itemId = item.Id;

                    string value = null;

                    if (request.ProfileIdentifier != Guid.Empty)
                        value = profiles.FirstOrDefault(x => x.ItemId == item.Id).Value;

                    item.Default = value;

                    await _itemRepository.UpdateAsync(item);

                    Log log = new Log
                    {
                        UserId = request.UserId,
                        UserCompanyId = request.CompanyId,
                        TargetId = item.Id,
                        NewValue = value,
                        OldValue = item.Default,
                        EntityType = ETypeEntity.AgentProfiles,
                        TypeLogMethod = ETypeLogMethod.Update,
                        CreatedIn = DateTime.Now
                    };

                    await _logRepository.CreateAsync(log);
                }

                var command = new ItemSolicitationHistoricCreate.Command(request.UserId, request.AgentId, request.CompanyId,
                                    itemId, SolicitationType.ChangeContainsProfile, "", "", "");

                var handle = new ItemSolicitationHistoricCreate.Handler(_itemRepository, _agentRepository, _userRepository, _rabbitMQ);

                await handle.Handle(command, cancellationToken);
                #endregion


                return request.ProfileIdentifier;
            }
        }
    }
}
