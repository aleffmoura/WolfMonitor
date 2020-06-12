using MediatR;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Application.Features.UsersAggregation.Handlers;
using Totten.Solutions.WolfMonitor.Application.Features.UsersAggregation.ViewModels;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Base;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Filters;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;

namespace Totten.Solutions.WolfMonitor.Companies.Controllers
{
    [Route("")]
    public class UsersController : ApiControllerBase
    {
        private IMediator _mediator;
        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{companyId}/users")]
        [ODataQueryOptionsValidate(AllowedQueryOptions.Top | AllowedQueryOptions.Count | AllowedQueryOptions.Skip)]
        [CustomAuthorizeAttributte(RoleLevelEnum.System, RoleLevelEnum.Admin, RoleLevelEnum.User)]
        public async Task<IActionResult> ReadAll([FromRoute]Guid companyId, [FromQuery]ODataQueryOptions<User> queryOptions)
        => await HandleQueryable<User, UserResumeViewModel>(await _mediator.Send(new UsersCollection.Query(companyId, CompanyId, Role)), queryOptions);

    }
}
