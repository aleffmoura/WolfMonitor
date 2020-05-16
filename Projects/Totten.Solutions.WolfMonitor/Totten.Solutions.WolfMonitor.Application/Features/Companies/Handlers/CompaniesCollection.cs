using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using Totten.Solutions.WolfMonitor.Application.Features.Companies.ViewModels;
using Totten.Solutions.WolfMonitor.Domain.Features.Companies;
using Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Application.Features.Companies.Handlers
{
    public class CompaniesCollection
    {
        public class Query : IRequest<Result<Exception, IQueryable<CompanyResumeViewModel>>>
        {
            public Query()
            {
            }
        }

        public class QueryHandler : RequestHandler<Query, Result<Exception, IQueryable<CompanyResumeViewModel>>>
        {
            private readonly ICompanyRepository _repository;
            private readonly IItemRepository _itemRepository;
            private readonly IUserRepository _userRepository;

            public QueryHandler(ICompanyRepository repository, IItemRepository itemRepository, IUserRepository userRepository)
            {
                _repository = repository;
                _itemRepository = itemRepository;
                _userRepository = userRepository;
            }

            protected override Result<Exception, IQueryable<CompanyResumeViewModel>> Handle(Query request)
            {
                var returned = new List<CompanyResumeViewModel>();

                var allCompany =  _repository.GetAll();

                if (allCompany.IsFailure)
                    return allCompany.Failure;

                foreach (var company in allCompany.Success)
                {

                    var users = _userRepository.GetAllByCompanyId(company.Id);

                    if (users.IsFailure)
                        continue;

                    var items = _itemRepository.GetAllByCompanyId(company.Id);
                    
                    if (items.IsFailure)
                        continue;

                    var listItems = items.Success.ToList();

                    returned.Add(new CompanyResumeViewModel
                    {
                        Id = company.Id,
                        Company = company.FantasyName,
                        QtdAgents = company.Agents.Count,
                        QtdServices = listItems.Where(i => i.Type == Domain.Enums.ETypeItem.SystemService).Count(),
                        QtdArchives = listItems.Where(i => i.Type == Domain.Enums.ETypeItem.SystemService).Count(),
                        QtdUsers = users.Success.Count()
                    });

                }

                return Result<Exception, IQueryable<CompanyResumeViewModel>>.Of(returned.AsQueryable());
            }
        }
    }
}
