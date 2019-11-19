using MediatR;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Application.Features.Agents.Handlers;
using Totten.Solutions.WolfMonitor.Application.Features.Agents.ViewModels;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Base;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Filters;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;

namespace Totten.Solutions.WolfMonitor.Agents.Controllers
{
    [Route("monitoring")]
    public class MonitoringController : ApiControllerBase
    {
        private IMediator _mediator;

        public MonitoringController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region HTTP GET
        [Route("services")]
        [ODataQueryOptionsValidate]
        [CustomAuthorizeAttributte(RoleLevelEnum.System, RoleLevelEnum.Admin, RoleLevelEnum.User)]
        public async Task<IActionResult> ReadAll(ODataQueryOptions<Agent> queryOptions)
        {
            return await HandleQueryable<Agent, AgentResumeViewModel>(await _mediator.Send(new AgentCollection.Query(CompanyId)), queryOptions);
        }
        #endregion
    }
}
