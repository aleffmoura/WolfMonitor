using MediatR;
using System;
using System.Linq;
using Totten.Solutions.WolfMonitor.Domain.Features.Users;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Application.Features.Users.Handlers
{
    public class UsersCollection
    {
        public class Query : IRequest<Result<Exception, IQueryable<User>>>
        {
            public Guid CompanyId { get; set; }

            public Query(Guid companyId)
            {
                CompanyId = companyId;
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
                return _repository.GetAll(request.CompanyId);
            }
        }
    }
}
