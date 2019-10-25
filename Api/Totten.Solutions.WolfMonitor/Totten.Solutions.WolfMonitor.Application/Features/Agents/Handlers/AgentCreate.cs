﻿using AutoMapper;
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
            public string CompanyId { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }

            public ValidationResult Validate() => new Validator().Validate(this);

            private class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(a => a.CompanyId).NotEmpty().Length(38);
                    RuleFor(a => a.UserName).NotEmpty().Length(4, 100);
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
                Agent agent = Mapper.Map<Command, Agent>(request);

                Result<Exception, Agent> callback = await _repository.CreateAsync(agent);

                if (callback.IsFailure)
                    return callback.Failure;

                return callback.Success.Id;
            }
        }
    }
}
