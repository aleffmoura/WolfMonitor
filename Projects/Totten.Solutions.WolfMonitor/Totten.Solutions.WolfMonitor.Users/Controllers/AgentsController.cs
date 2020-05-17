using MediatR;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Application.Features.UsersAggregation.Handlers;
using Totten.Solutions.WolfMonitor.Application.Features.UsersAggregation.ViewModels;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Base;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Filters;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;

namespace Totten.Solutions.WolfMonitor.Users.Controllers
{
    [Route("agents")]
    public class AgentsController : ApiControllerBase
    {
        private IMediator _mediator;

        public AgentsController(IMediator mediator)
            => _mediator = mediator;

        #region GET
        [HttpGet()]
        [ODataQueryOptionsValidate(AllowedQueryOptions.Top | AllowedQueryOptions.Count | AllowedQueryOptions.Skip)]
        [CustomAuthorizeAttributte(RoleLevelEnum.System, RoleLevelEnum.Admin, RoleLevelEnum.User)]
        public async Task<IActionResult> ReadAll([FromQuery]ODataQueryOptions<AgentForUserViewModel> queryOptions)
            => await HandleQueryable<AgentForUserViewModel, AgentForUserViewModel>(await _mediator.Send(new UserAgentsCollection.Query(UserId)), queryOptions);
        #endregion
    }
}
