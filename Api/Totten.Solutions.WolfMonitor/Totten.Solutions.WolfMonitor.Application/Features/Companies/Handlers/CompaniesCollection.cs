using MediatR;
using System;
using System.Linq;
using Totten.Solutions.WolfMonitor.Domain.Features.Companies;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Application.Features.Companies.Handlers
{
    public class CompaniesCollection
    {
        public class Query : IRequest<Result<Exception, IQueryable<Company>>>
        {
            public Query()
            {
            }
        }

        public class QueryHandler : RequestHandler<Query, Result<Exception, IQueryable<Company>>>
        {
            private readonly ICompanyRepository _repository;

            public QueryHandler(ICompanyRepository repository)
            {
                _repository = repository;
            }

            protected override Result<Exception, IQueryable<Company>> Handle(Query request)
            {
                return _repository.GetAll();
            }
        }
    }
}
