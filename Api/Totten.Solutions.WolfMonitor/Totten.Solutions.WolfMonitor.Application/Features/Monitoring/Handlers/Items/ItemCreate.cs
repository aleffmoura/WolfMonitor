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
            public string Default { get; set; }
            public int Interval { get; set; }

            public Command(Guid companyId,
                           Guid userIdWhoAdd,
                           Guid agentId,
                           string name,
                           string displayName,
                           string defaultValue,
                           int interval)
            {
                CompanyId = companyId;
                AgentId = agentId;
                UserIdWhoAdd = userIdWhoAdd;
                Name = name;
                DisplayName = displayName;
                Default = defaultValue;
                Interval = interval;
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
                    RuleFor(a => a.Name).NotEmpty().Length(4, 250);
                    RuleFor(a => a.DisplayName).NotEmpty().Length(4, 250);
                }
            }
        }

        public class Handler : IRequestHandler<Command, Result<Exception, Guid>>
        {
            private readonly IMapper _mapper;
            private readonly IAgentRepository _agentRepository;
            private readonly IItemRepository _repository;

            public Handler(IMapper mapper, IItemRepository repository, IAgentRepository agentRepository)
            {
                _mapper = mapper;
                _repository = repository;
                _agentRepository = agentRepository;
            }

            public async Task<Result<Exception, Guid>> Handle(Command request, CancellationToken cancellationToken)
            {
                var agentCallback = await _agentRepository.GetByIdAsync(request.AgentId);
                if (agentCallback.IsFailure)
                {
                    return agentCallback.Failure;
                }
                if (agentCallback.Success.CompanyId != request.CompanyId)
                {
                    return new NotAllowedException("Usuário não pode salvar serviços no agent informado, a empresa do usuario e do agent não são iguais");
                }

                var ItemVerify = await _repository.GetByNameWithAgentId(request.Name, request.AgentId);
                if (ItemVerify.IsSuccess)
                {
                    return new DuplicateException("Já existe um Item com esse nome cadastrado nesse agent.");
                }

                Item Item = _mapper.Map<Command, Item>(request);

                var callback = await _repository.CreateAsync(Item);
                if (callback.IsFailure)
                {
                    return callback.Failure;
                }

                return callback.Success.Id;
            }
        }
    }
}
