using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Enums;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.Logs;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;
using Unit = Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs.Unit;

namespace Totten.Solutions.WolfMonitor.Application.Features.UsersAggregation.Handlers
{
    public class UserRemove
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
                    RuleFor(d => d.Id).NotEqual(Guid.Empty).WithMessage("Id do usuário a ser removido está inválido");
                    RuleFor(d => d.UserId).NotEqual(Guid.Empty).WithMessage("Id do usuário da solicitação está inválido");
                    RuleFor(d => d.CompanyId).NotEqual(Guid.Empty).WithMessage("Id da empresa da solicitação está inválido");
                }
            }
        }

        public class Handler : IRequestHandler<Command, Result<Exception, Unit>>
        {
            private readonly ILogRepository _logRepository;
            private readonly IUserRepository _repository;

            public Handler( IUserRepository repository, ILogRepository logRepository)
            {
                _repository = repository;
                _logRepository = logRepository;
            }

            public async Task<Result<Exception, Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var userCallback = await _repository.GetByIdAsync(request.Id);

                if (userCallback.IsFailure)
                    return userCallback.Failure;

                if (userCallback.Success.Id == request.UserId)
                    throw new BusinessException(ErrorCodes.NotFound, "Um usuário não pode excluir sua propria conta, contact um administrador.");

                if (userCallback.Success.CompanyId != request.CompanyId)
                    throw new BusinessException(ErrorCodes.NotFound, "Usuário não pertence a sua empresa, contact um administrador.");

                userCallback.Success.Removed = true;

                var updatedCallBack = await _repository.UpdateAsync(userCallback.Success);

                if (updatedCallBack.IsFailure)
                    return updatedCallBack.Failure;

                Log log = new Log
                {
                    UserId = request.UserId,
                    UserCompanyId = request.CompanyId,
                    TargetId = request.Id,
                    EntityType = ETypeEntity.Users,
                    TypeLogMethod = ETypeLogMethod.Remove,
                    CreatedIn = DateTime.Now
                };

                await _logRepository.CreateAsync(log);

                return Unit.Successful;
            }
        }
    }
}
