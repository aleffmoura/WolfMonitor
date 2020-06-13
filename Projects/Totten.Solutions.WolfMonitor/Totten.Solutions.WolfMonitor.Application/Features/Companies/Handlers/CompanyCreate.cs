using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Enums;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.Companies;
using Totten.Solutions.WolfMonitor.Domain.Features.Logs;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Application.Features.Companies.Handlers
{
    public class CompanyCreate
    {
        public class Command : IRequest<Result<Exception, Guid>>
        {
            public Guid UserCreatedId { get; set; }
            public Guid UserCreatedCompanyId { get; set; }

            public string Name { get; set; }
            public string FantasyName { get; set; }
            public string Cnpj { get; set; }
            public string StateRegistration { get; set; }
            public string MunicipalRegistration { get; set; }
            public string Cnae { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Address { get; set; }


            public ValidationResult Validate()
            {
                return new Validator().Validate(this);
            }

            private class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(a => a.Name).NotEmpty().Length(4, 200).WithMessage("Razão social deve conter entre 4 e 100 caracteres");
                    RuleFor(a => a.FantasyName).NotEmpty().Length(2, 150).WithMessage("Nome fantasia deve conter entre 2 e 100 caracteres");
                    RuleFor(a => a.Cnpj).NotEmpty().Length(18).WithMessage("CNPJ é inválido");
                    RuleFor(a => a.Email).NotEmpty().Length(5, 150).WithMessage("Email deve conter entre 5 e 150 caracteres");
                    RuleFor(a => a.Phone).NotEmpty().Length(11, 15).WithMessage("Telefone deve conter entre 11 e 15 caracteres");
                    RuleFor(a => a.Address).NotEmpty().Length(5, 250);
                }
            }
        }

        public class Handler : IRequestHandler<Command, Result<Exception, Guid>>
        {
            private readonly ICompanyRepository _repository;
            private readonly ILogRepository _logRepository;

            public Handler(ICompanyRepository repository, ILogRepository logRepository)
            {
                _repository = repository;
                _logRepository = logRepository;
            }

            public async Task<Result<Exception, Guid>> Handle(Command request, CancellationToken cancellationToken)
            {
                var companyCallback = await _repository.GetByNameOrCnpjAsync(request.Name, request.Cnpj);

                if (companyCallback.IsSuccess)
                    return new DuplicateException("Já existe uma empresa/cnpj cadastrada com esse nome");

                Company company = Mapper.Map<Command, Company>(request);

                Result<Exception, Company> callback = await _repository.CreateAsync(company);

                if (callback.IsFailure)
                    return callback.Failure;

                Log log = new Log
                {
                    UserId = request.UserCreatedId,
                    UserCompanyId = request.UserCreatedCompanyId,
                    TargetId = callback.Success.Id,
                    EntityType = ETypeEntity.Companies,
                    TypeLogMethod = ETypeLogMethod.Create,
                    CreatedIn = DateTime.Now
                };

                await _logRepository.CreateAsync(log);

                return callback.Success.Id;
            }
        }
    }
}
