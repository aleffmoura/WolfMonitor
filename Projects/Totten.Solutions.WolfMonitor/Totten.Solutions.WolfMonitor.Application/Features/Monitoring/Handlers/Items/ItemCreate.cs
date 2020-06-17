using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Enums;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation;
using Totten.Solutions.WolfMonitor.Domain.Features.Logs;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Application.Features.Monitoring.Handlers.Items
{
    public class ItemCreate
    {
        public class Command : IRequest<Result<Exception, Guid>>
        {
            public Guid CompanyId { get; set; }
            public Guid UserIdWhoAdd { get; set; }
            public Guid AgentId { get; set; }
            public string Name { get; set; }
            public string DisplayName { get; set; }
            public string AboutCurrentValue { get; set; }
            public ETypeItem Type { get; set; }


            public Command(Guid companyId,
                           Guid userIdWhoAdd,
                           Guid agentId,
                           string name,
                           string displayName,
                           string aboutCurrentValue,
                           ETypeItem typeItem)
            {
                CompanyId = companyId;
                AgentId = agentId;
                UserIdWhoAdd = userIdWhoAdd;
                Name = name;
                DisplayName = displayName;
                AboutCurrentValue = aboutCurrentValue;
                Type = typeItem;
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
                        .WithMessage("Identificador da impresa é invalido");
                    RuleFor(a => a.UserIdWhoAdd).NotEqual(Guid.Empty)
                        .WithMessage("Identificador do usuario ao qual esta adicionando o serviço é invalido");
                    RuleFor(a => a.Name).Length(4, 250).WithMessage("Nome do item deve conter entre 4 e 250 caracteres");
                    RuleFor(a => a.DisplayName).Length(4, 250).WithMessage("Nome de exibição do item deve conter entre 4 e 250 caracteres");
                    RuleFor(a => a.Type).NotNull().WithMessage("Tipo do item é inválido");
                }
            }
        }

        public class Handler : IRequestHandler<Command, Result<Exception, Guid>>
        {
            private readonly IAgentRepository _agentRepository;
            private readonly IItemRepository _repository;
            private readonly ILogRepository _logRepository;

            public Handler(IItemRepository repository, IAgentRepository agentRepository, ILogRepository logRepository)
            {
                _repository = repository;
                _agentRepository = agentRepository;
                _logRepository = logRepository;
            }

            public async Task<Result<Exception, Guid>> Handle(Command request, CancellationToken cancellationToken)
            {
                var agentCallback = await _agentRepository.GetByIdAsync(request.AgentId);

                if (agentCallback.IsFailure)
                    return agentCallback.Failure;

                if (agentCallback.Success.CompanyId != request.CompanyId)
                    return new NotAllowedException("Usuário não pode salvar items no agent informado, a empresa do usuario e do agent não são iguais");

                var ItemVerify = await _repository.GetByNameOrDisplayNameWithAgentId(request.AgentId, request.Name, request.DisplayName );

                if (ItemVerify.IsSuccess)
                    return new DuplicateException("Já existe um item com esse nome/nome de exibição cadastrado nesse agent.");

                Item Item = Mapper.Map<Command, Item>(request);

                var callback = await _repository.CreateAsync(Item);

                if (callback.IsFailure)
                    return callback.Failure;

                Log log = new Log
                {
                    UserId = request.UserIdWhoAdd,
                    UserCompanyId = request.CompanyId,
                    TargetId = callback.Success.Id,
                    EntityType = ETypeEntity.Monitoring,
                    TypeLogMethod = ETypeLogMethod.Create,
                    CreatedIn = DateTime.Now
                };

                await _logRepository.CreateAsync(log);

                return callback.Success.Id;
            }
        }
    }
}
