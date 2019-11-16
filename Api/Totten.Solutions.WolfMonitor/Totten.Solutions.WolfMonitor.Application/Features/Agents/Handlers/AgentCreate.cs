using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Application.Features.Agents.Handlers
{
    public class AgentCreate
    {
        public class Command : IRequest<Result<Exception, Guid>>
        {
            public Guid CompanyId { get; set; }
            public Guid UserWhoCreatedId { get; set; }
            public string Login { get; set; }
            public string Password { get; set; }

            public Command(Guid companyId,
                           Guid userWhoCreated,
                           string login,
                           string password)
            {
                CompanyId = companyId;
                UserWhoCreatedId = userWhoCreated;
                Login = login;
                Password = password;
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
                        .WithMessage("Identificador da empresa é invalido");
                    RuleFor(a => a.UserWhoCreatedId).NotEqual(Guid.Empty)
                        .WithMessage("Identificador do usuario ao qual esta criando o agente é invalido");
                    RuleFor(a => a.Login).NotEmpty().Length(4, 100);
                    RuleFor(a => a.Password).NotEmpty().Length(4, 100);
                }
            }
        }

        public class Handler : IRequestHandler<Command, Result<Exception, Guid>>
        {
            private readonly IAgentRepository _repository;

            public Handler(IAgentRepository repository)
            {
                _repository = repository;
            }

            public async Task<Result<Exception, Guid>> Handle(Command request, CancellationToken cancellationToken)
            {
                Result<Exception, Agent> agentVerify = await _repository.GetByLogin(request.CompanyId, request.Login);

                if (agentVerify.IsSuccess)
                {
                    return new Exception("Já existe um agente com esse login cadastrado.");
                }

                Agent agent = Mapper.Map<Command, Agent>(request);
                Result<Exception, Agent> callback = await _repository.CreateAsync(agent);

                if (callback.IsFailure)
                {
                    return callback.Failure;
                }

                return callback.Success.Id;
            }
        }
    }
}
