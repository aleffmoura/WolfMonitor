using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Features.SystemServices;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;
using Unit = Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs.Unit;

namespace Totten.Solutions.WolfMonitor.Application.Features.Monitoring.Handlers.SystemServices
{
    public class SystemServiceUpdate
    {
        public class Command : IRequest<Result<Exception, Unit>>
        {
            public Guid Id { get; set; }
            public string Value { get; set; }


            public Command(Guid id, string value)
            {
                Id = id;
                Value = value;
            }

            public ValidationResult Validate()
            {
                return new Validator().Validate(this);
            }

            private class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(a => a.Id).NotEqual(Guid.Empty);
                    RuleFor(a => a.Value).NotEmpty().Length(4, 100);
                }
            }
        }

        public class Handler : IRequestHandler<Command, Result<Exception, Unit>>
        {
            private readonly ISystemServiceRepository _repository;

            public Handler(ISystemServiceRepository repository)
            {
                _repository = repository;
            }

            public async Task<Result<Exception, Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                Result<Exception, SystemService> systemServiceCallback = await _repository.GetByIdAsync(request.Id);


                if (systemServiceCallback.IsFailure)
                {
                    return systemServiceCallback.Failure;
                }

                SystemService systemService = systemServiceCallback.Success;
                Mapper.Map(request, systemService);

                return await _repository.UpdateAsync(systemService);
            }
        }
    }
}
