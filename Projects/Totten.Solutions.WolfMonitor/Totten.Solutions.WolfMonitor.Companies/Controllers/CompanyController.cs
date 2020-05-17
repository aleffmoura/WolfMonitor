using MediatR;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Application.Features.Companies.Handlers;
using Totten.Solutions.WolfMonitor.Application.Features.Companies.ViewModels;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Base;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Filters;
using Totten.Solutions.WolfMonitor.Domain.Features.Companies;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;

namespace Totten.Solutions.WolfMonitor.Companies.Controllers
{
    [Route("")]
    [CustomAuthorizeAttributte(RoleLevelEnum.User, RoleLevelEnum.Admin, RoleLevelEnum.System)]
    public class CompanyController : ApiControllerBase
    {
        private IMediator _mediator;
        public CompanyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region HTTP POST
        [HttpPost]
        [CustomAuthorizeAttributte(RoleLevelEnum.System)]
        public async Task<IActionResult> Create([FromBody]CompanyCreate.Command command)
            => HandleCommand(await _mediator.Send(command));
        #endregion

        #region HTTP PATCH
        #endregion

        #region HTTP GET
        [HttpGet]
        [ODataQueryOptionsValidate(AllowedQueryOptions.Top | AllowedQueryOptions.Skip | AllowedQueryOptions.Count)]
        [CustomAuthorizeAttributte(RoleLevelEnum.System)]
        public async Task<IActionResult> ReadAll(ODataQueryOptions<CompanyResumeViewModel> queryOptions)
        {
            return await HandleQueryable<CompanyResumeViewModel, CompanyResumeViewModel>(await _mediator.Send(new CompaniesCollection.Query()), queryOptions);
        }


        [HttpGet("{companyId}")]
        [CustomAuthorizeAttributte(RoleLevelEnum.System, RoleLevelEnum.Admin, RoleLevelEnum.User)]
        public async Task<IActionResult> ReadAllInfo([FromRoute]Guid companyId)
            => HandleQuery<Company, CompanyDetailViewModel>(await _mediator.Send(new CompanyDetail.Query(companyId, CompanyId, Role)));
        
        #endregion

    }
}
