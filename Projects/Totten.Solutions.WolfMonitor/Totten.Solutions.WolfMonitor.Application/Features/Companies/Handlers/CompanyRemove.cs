using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.Companies;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;
using Unit = Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs.Unit;

namespace Totten.Solutions.WolfMonitor.Application.Features.Companies.Handlers
{
    public class CompanyRemove
    {
        public class Command : IRequest<Result<Exception, Unit>>
        {
            public Guid Id { get; set; }
            public Guid UserId { get; set; }

            public Command(Guid id, Guid userId)
            {
                Id = id;
                UserId = userId;
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
                    RuleFor(a => a.UserId).NotEqual(Guid.Empty);
                }
            }
        }

        public class Handler : IRequestHandler<Command, Result<Exception, Unit>>
        {
            private readonly ICompanyRepository _repository;
            private readonly IUserRepository _userRepository;
            //private readonly ILogMonitoringRepository _logMonitoringRepository;

            public Handler(ICompanyRepository repository,
                           IUserRepository userRepository
                /*, ILogMonitoringRepository logMonitoringRepository*/)
            {
                _repository = repository;
                _userRepository = userRepository;
                //_logMonitoringRepository = logMonitoringRepository;
            }

            public async Task<Result<Exception, Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                Result<Exception, Unit> returned = Unit.Successful;
                Result<Exception, Company> companyCallback = await _repository.GetByIdAsync(request.Id);

                if (companyCallback.IsFailure)
                    return companyCallback.Failure;

                var userCallback = await _userRepository.GetByIdAsync(request.UserId);

                if (userCallback.IsSuccess)
                    return new BusinessException(Domain.Enums.ErrorCodes.NotAllowed, "Usuário não permitido para essa requisição.");

                if(userCallback.Success.Role.Level != RoleLevelEnum.System)
                    return new BusinessException(Domain.Enums.ErrorCodes.NotAllowed, "Usuário não permitido para essa requisição.");

                companyCallback.Success.Removed = true;
                /*
                 var log = new Log()
                 */
                //await _logMonitoringRepository.CreateAsync(log);

                return await _repository.UpdateAsync(companyCallback.Success);
            }
        }
    }
}
