﻿using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
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
                    RuleFor(d => d.Id).NotEqual(Guid.Empty);
                    RuleFor(d => d.UserId).NotEqual(Guid.Empty);
                    RuleFor(d => d.CompanyId).NotEqual(Guid.Empty);
                }
            }
        }

        public class Handler : IRequestHandler<Command, Result<Exception, Unit>>
        {
            private readonly IUserRepository _repository;

            public Handler( IUserRepository repository)
                => _repository = repository;

            public async Task<Result<Exception, Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var userCallback = await _repository.GetByIdAsync(request.Id);

                if (userCallback.IsFailure)
                    return userCallback.Failure;

                if (userCallback.Success.Id == request.UserId)
                    throw new BusinessException(Domain.Enums.ErrorCodes.NotFound, "Um usuário não pode excluir sua propria conta, contact um administrador.");

                if (userCallback.Success.CompanyId != request.CompanyId)
                    throw new BusinessException(Domain.Enums.ErrorCodes.NotFound, "Usuário não pertence a sua empresa, contact um administrador.");

                userCallback.Success.Removed = true;

                var updatedCallBack = await _repository.UpdateAsync(userCallback.Success);

                if (updatedCallBack.IsFailure)
                    return updatedCallBack.Failure;

                return Unit.Successful;
            }
        }
    }
}