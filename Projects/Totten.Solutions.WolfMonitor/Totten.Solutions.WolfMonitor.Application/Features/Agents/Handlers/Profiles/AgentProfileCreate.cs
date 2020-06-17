using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Enums;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents.Profiles;
using Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation;
using Totten.Solutions.WolfMonitor.Domain.Features.Logs;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;
using Profile = Totten.Solutions.WolfMonitor.Domain.Features.Agents.Profiles.Profile;

namespace Totten.Solutions.WolfMonitor.Application.Features.Agents.Handlers.Profiles
{
    public class AgentProfileCreate
    {
        public class Command : IRequest<Result<Exception, Guid>>
        {
            public Guid AgentId { get; set; }
            public Guid CompanyId { get; set; }
            public Guid UserWhoCreatedId { get; set; }
            public string Name { get; set; }
            public string Value { get; set; }

            public Command(Guid agentId,
                           Guid companyId,
                           Guid userWhoCreatedId,
                           string name)
            {
                AgentId = agentId;
                CompanyId = companyId;
                UserWhoCreatedId = userWhoCreatedId;
                Name = name;
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
                    RuleFor(a => a.UserWhoCreatedId).NotEqual(Guid.Empty)
                        .WithMessage("Identificador do usuario ao qual esta criando o profile é inválido");
                    RuleFor(a => a.AgentId).NotEqual(Guid.Empty)
                        .WithMessage("Identificador do agent ao qual esta criando o profile é inválido");
                    RuleFor(a => a.Name).Length(4, 100).WithMessage("Nome deve possuir entre 4 e 100 caracteres.");
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

            public Handler(IProfileRepository repository,
                           IAgentRepository agentRepository,
                           IUserRepository userRepository,
                           IItemRepository itemRepository,
                           ILogRepository logRepository)
            {
                _repository = repository;
                _agentRepository = agentRepository;
                _userRepository = userRepository;
                _itemRepository = itemRepository;
                _logRepository = logRepository;
            }

            public async Task<Result<Exception, Guid>> Handle(Command request, CancellationToken cancellationToken)
            {
                Result<Exception, Agent> agentVerify = await _agentRepository.GetByIdAsync(request.CompanyId, request.AgentId);

                if (agentVerify.IsFailure)
                    return new DuplicateException("Agent não encontrado na empresa do usuário.");

                var userCallback = await _userRepository.GetByIdAsync(request.UserWhoCreatedId);

                if (userCallback.IsFailure)
                    return new NotFoundException("Não foi encontrado um usúario com o id da requisição.");

                Profile profile = Mapper.Map<Command, Profile>(request);

                profile.ProfileIdentifier = Guid.NewGuid();

                var items = _itemRepository.GetAll(request.AgentId);

                if (items.IsFailure)
                    return items.Failure;

                Result<Exception, Profile> callback = new BusinessException(ErrorCodes.ServiceUnavailable, "ocorreram falhas na criação do perfil");

                foreach (var item in items.Success)
                {
                    var pro = Mapper.Map<Profile, Profile>(profile);
                    pro.Value = item.Value;
                    pro.ItemId = item.Id;
                    
                    callback = await _repository.CreateAsync(pro);
                }

                if (callback.IsFailure)
                    return callback.Failure;

                Log log = new Log
                {
                    UserId = request.UserWhoCreatedId,
                    UserCompanyId = request.CompanyId,
                    NewValue = $"{profile.Name}",
                    OldValue = "Não existe",
                    TargetId = profile.ProfileIdentifier,
                    EntityType = ETypeEntity.AgentProfiles,
                    TypeLogMethod = ETypeLogMethod.Create,
                    CreatedIn = DateTime.Now
                };

                await _logRepository.CreateAsync(log);

                return profile.ProfileIdentifier;
            }
        }
    }
}
