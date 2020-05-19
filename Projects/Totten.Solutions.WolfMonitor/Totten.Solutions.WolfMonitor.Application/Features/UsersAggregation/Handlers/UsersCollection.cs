﻿using MediatR;
using System;
using System.Linq;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Application.Features.UsersAggregation.Handlers
{
    public class UsersCollection
    {
        public class Query : IRequest<Result<Exception, IQueryable<User>>>
        {
            public Guid CompanyId { get; set; }
            public Guid UserCompany { get; set; }
            public string Role { get; set; }

            public Query(Guid companyId, Guid userCompany, string role)
            {
                CompanyId = companyId;
                UserCompany = userCompany;
                Role = role;
            }
        }

        public class QueryHandler : RequestHandler<Query, Result<Exception, IQueryable<User>>>
        {
            private readonly IUserRepository _repository;

            public QueryHandler(IUserRepository repository)
            {
                _repository = repository;
            }

            protected override Result<Exception, IQueryable<User>> Handle(Query request)
            {
                var roleEnum = Enum.Parse<RoleLevelEnum>(request.Role);

                if (request.CompanyId != request.UserCompany && roleEnum != (RoleLevelEnum.System | RoleLevelEnum.Admin))
                    return new BusinessException(Domain.Enums.ErrorCodes.Unauthorized, "Empresa não permitida ser acessada por seu usuário");

                return _repository.GetAll(request.CompanyId);
            }
        }
    }
}
