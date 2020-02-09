using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;
using Unit = Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs.Unit;

namespace Totten.Solutions.WolfMonitor.Application.Features.Agents.Handlers
{
    public class AgentRemove
    {
        public class Command : IRequest<Result<Exception, Unit>>
        {
            public Guid Id { get; set; }
            public Guid UserId { get; set; }
            public Guid CompanyId { get; set; }

            public Command(Guid id,Guid companyId, Guid userId)
            {
                Id = id;
                UserId = userId;
                CompanyId = companyId;
            }

            public ValidationResult Validate()
            {
                return new Validator().Validate(this);
            }

            private class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(d => d.Id).NotEqual(Guid.Empty);
                    RuleFor(d => d.UserId).NotEqual(Guid.Empty);
                    RuleFor(d => d.CompanyId).NotEqual(Guid.Empty);
                }
            }
        }

        public class Handler : IRequestHandler<Command, Result<Exception, Unit>>
        {
            private readonly IAgentRepository _repository;
            private readonly IUserRepository _userRepository;

            public Handler(IAgentRepository repository, IUserRepository userRepository)
            {
                _repository = repository;
                _userRepository = userRepository;
            }

            public async Task<Result<Exception, Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var agentCallback = await _repository.GetByIdAsync(request.CompanyId, request.Id);

                if (agentCallback.IsFailure)
                    return agentCallback.Failure;

                var userCallback = await _userRepository.GetByIdAsync(request.UserId);

                if (agentCallback.Success.CompanyId == userCallback.Success.CompanyId)
                    throw new BusinessException(Domain.Enums.ErrorCodes.NotFound, "Usuário não pertence a empresa do agent informado.");

                agentCallback.Success.Removed = true;

                return await _repository.UpdateAsync(agentCallback.Success);
            }
        }
    }
}
