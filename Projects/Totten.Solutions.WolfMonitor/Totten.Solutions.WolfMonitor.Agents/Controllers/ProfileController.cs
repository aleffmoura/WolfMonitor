using MediatR;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Agents.Commands.Profiles;
using Totten.Solutions.WolfMonitor.Application.Features.Agents.Handlers.Profiles;
using Totten.Solutions.WolfMonitor.Application.Features.Agents.ViewModels.Profiles;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Base;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Filters;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents.Profiles;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;

namespace Totten.Solutions.WolfMonitor.Agents.Controllers
{
    [Route("")]
    public class ProfileController : ApiControllerBase
    {
        private IMediator _mediator;

        public ProfileController(IMediator mediator)
            => _mediator = mediator;

        #region HTTP POST
        [HttpPost]
        [CustomAuthorizeAttributte(RoleLevelEnum.System, RoleLevelEnum.Admin)]
        public async Task<IActionResult> Create([FromBody]ProfileCreateCommand command)
            => HandleCommand(await _mediator.Send(new AgentProfileCreate.Command(command.AgentId, CompanyId, UserId, command.Name)));

        #endregion
        #region HTTP Delete

        //[HttpDelete("{Id}")]
        //[CustomAuthorizeAttributte(RoleLevelEnum.System, RoleLevelEnum.Admin)]
        //public async Task<IActionResult> RemoveItem([FromRoute]Guid id)
        //    => HandleCommand(await _mediator.Send(new AgentRemove.Command(id, CompanyId, UserId)));

        #endregion

        #region HTTP GET
        [HttpGet()]
        [ODataQueryOptionsValidate(AllowedQueryOptions.Top | AllowedQueryOptions.Skip | AllowedQueryOptions.Count)]
        [CustomAuthorizeAttributte(RoleLevelEnum.System, RoleLevelEnum.Admin, RoleLevelEnum.User)]
        public async Task<IActionResult> ReadAll([FromRoute]Guid agentId, ODataQueryOptions<Profile> queryOptions)
            => await HandleQueryable<Profile, ProfileViewModel>(await _mediator.Send(new ProfileCollection.Query(agentId)), queryOptions);

        #endregion
    }
}
