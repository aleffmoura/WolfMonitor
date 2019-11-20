using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Application.Features.Monitoring.Handlers;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Base;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Filters;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;
using Totten.Solutions.WolfMonitor.Monitoring.Commands;

namespace Totten.Solutions.WolfMonitor.Monitoring.Controllers
{
    [Route("SystemServices")]
    public class SystemServicesController : ApiControllerBase
    {
        private IMediator _mediator;

        public SystemServicesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region HTTP POST
        [HttpPost]
        [CustomAuthorizeAttributte(RoleLevelEnum.Admin)]
        public async Task<IActionResult> Create([FromBody]SystemServiceCreateCommand command)
        {
            return HandleCommand(await _mediator.Send(new SystemServiceCreate.Command(CompanyId, command.AgentId, UserId, command.Name, command.DisplayName)));
        }
        #endregion
    }
}
