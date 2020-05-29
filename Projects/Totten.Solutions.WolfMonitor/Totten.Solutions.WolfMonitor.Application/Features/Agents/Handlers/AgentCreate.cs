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
using Totten.Solutions.WolfMonitor.Domain.Features.Logs;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Application.Features.Agents.Handlers
{
    public class AgentCreate
    {
        public class Command : IRequest<Result<Exception, Guid>>
        {
            public Guid CompanyId { get; set; }
            public Guid UserWhoCreatedId { get; set; }
            public string UserWhoCreatedName { get; set; }
            public string DisplayName { get; set; }
            public string Login { get; set; }
            public string Password { get; set; }
            public bool ReadItemsMonitoringByArchive { get; set; }

            public Command(Guid companyId,
                           Guid userWhoCreatedId,
                           string displayName,
                           string login,
                           string password,
                           bool readItemsMonitoringByArchive)
            {
                CompanyId = companyId;
                UserWhoCreatedId = userWhoCreatedId;
                DisplayName = displayName;
                Login = login;
                Password = password;
                ReadItemsMonitoringByArchive = readItemsMonitoringByArchive;
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
                    RuleFor(a => a.DisplayName).NotEmpty().Length(4, 100);
                    RuleFor(a => a.Login).NotEmpty().Length(4, 100);
                    RuleFor(a => a.Password).NotEmpty().Length(4, 100);
                }
            }
        }

        public class Handler : IRequestHandler<Command, Result<Exception, Guid>>
        {
            private readonly IAgentRepository _repository;
            private readonly IUserRepository _userRepository;
            private readonly ILogRepository _logRepository;

            public Handler(IAgentRepository repository,
                           IUserRepository userRepository,
                           ILogRepository logRepository)
            {
                _repository = repository;
                _userRepository = userRepository;
                _logRepository = logRepository;
            }

            public async Task<Result<Exception, Guid>> Handle(Command request, CancellationToken cancellationToken)
            {
                Result<Exception, Agent> agentVerify = await _repository.GetByLogin(request.CompanyId, request.Login);

                if (agentVerify.IsSuccess)
                    return new DuplicateException("Já existe um agente com esse login cadastrado.");

                var userCallback = await _userRepository.GetByIdAsync(request.UserWhoCreatedId);

                if (userCallback.IsFailure)
                    return new NotFoundException("Não foi encontrado um usúario com o id da requisição.");

                request.UserWhoCreatedName = userCallback.Success.Login;

                Agent agent = Mapper.Map<Command, Agent>(request);

                Result<Exception, Agent> callback = await _repository.CreateAsync(agent);

                if (callback.IsFailure)
                {
                    return callback.Failure;
                }

                Log log = new Log
                {
                    UserId = request.UserWhoCreatedId,
                    UserCompanyId = request.CompanyId,
                    TargetId = callback.Success.Id,
                    EntityType = ETypeEntity.Agents,
                    TypeLogMethod = ETypeLogMethod.Create,
                    CreatedIn = DateTime.Now
                };

                await _logRepository.CreateAsync(log);

                return callback.Success.Id;
            }
        }
    }
}
