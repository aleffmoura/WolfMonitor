using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Domain.Features.SystemServices;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Application.Features.Monitoring.Handlers.SystemServices
{
    public class SystemServiceCreate
    {
        public class Command : IRequest<Result<Exception, Guid>>
        {
            public Guid CompanyId { get; set; }
            public Guid AgentId { get; set; }
            public Guid UserIdWhoAdd { get; set; }
            public string Name { get; set; }
            public string DisplayName { get; set; }

            public Command(Guid companyId,
                           Guid agentId,
                           Guid userIdWhoAdd,
                           string name,
                           string displayName)
            {
                CompanyId = companyId;
                AgentId = agentId;
                UserIdWhoAdd = userIdWhoAdd;
                Name = name;
                DisplayName = displayName;
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
            private readonly IAgentRepository _agentRepository;
            private readonly ISystemServiceRepository _repository;

            public Handler(ISystemServiceRepository repository, IAgentRepository agentRepository)
            {
                _repository = repository;
                _agentRepository = agentRepository;
            }

            public async Task<Result<Exception, Guid>> Handle(Command request, CancellationToken cancellationToken)
            {
                Result<Exception, Agent> agentCallback = await _agentRepository.GetByIdAsync(request.AgentId);
                if (agentCallback.IsFailure)
                {
                    return agentCallback.Failure;
                }
                if (agentCallback.Success.CompanyId != request.CompanyId)
                {
                    return new NotAllowedException("Usuário não pode salvar serviços no agent informado, a empresa do usuario e do agent não são iguais");
                }

                Result<Exception, SystemService> SystemServiceVerify = await _repository.GetByNameWithAgentId(request.Name, request.AgentId);
                if (SystemServiceVerify.IsSuccess)
                {
                    return new DuplicateException("Já existe um SystemService com esse nome cadastrado nesse agent.");
                }

                SystemService SystemService = Mapper.Map<Command, SystemService>(request);

                Result<Exception, SystemService> callback = await _repository.CreateAsync(SystemService);
                if (callback.IsFailure)
                {
                    return callback.Failure;
                }

                return callback.Success.Id;
            }
        }
    }
}
