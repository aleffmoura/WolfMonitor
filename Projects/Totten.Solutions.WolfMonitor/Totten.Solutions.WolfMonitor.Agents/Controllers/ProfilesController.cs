using MediatR;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using System;
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
    [Route("profiles")]
    public class ProfilesController : ApiControllerBase
    {
        private IMediator _mediator;

        public ProfilesController(IMediator mediator)
            => _mediator = mediator;

        #region HTTP POST
        [HttpPost]
        [CustomAuthorizeAttributte(RoleLevelEnum.System, RoleLevelEnum.Admin)]
        public async Task<IActionResult> CreateProfile([FromBody]ProfileCreateCommand command)
            => HandleCommand(await _mediator.Send(new AgentProfileCreate.Command(command.AgentId, CompanyId, UserId, command.Name)));

        #endregion

        [HttpPatch]
        [CustomAuthorizeAttributte(RoleLevelEnum.System, RoleLevelEnum.Admin)]
        public async Task<IActionResult> ApplyProfile([FromBody]ProfileApplyCommand command)
            => HandleCommand(await _mediator.Send(new ProfileApply.Command(command.ProfileIdentifier, command.AgentId, CompanyId, UserId)));

        #region HTTP Delete

        [HttpDelete("{agentId}/{profileIdentifier}")]
        [CustomAuthorizeAttributte(RoleLevelEnum.System, RoleLevelEnum.Admin)]
        public async Task<IActionResult> RemoveItem([FromRoute]Guid agentId, [FromRoute]Guid profileIdentifier)
            => HandleCommand(await _mediator.Send(new ProfileRemove.Command(UserId, profileIdentifier, CompanyId, agentId)));

        #endregion

        #region HTTP GET
        [HttpGet("{agentId}")]
        [ODataQueryOptionsValidate(AllowedQueryOptions.Top | AllowedQueryOptions.Skip | AllowedQueryOptions.Count)]
        [CustomAuthorizeAttributte(RoleLevelEnum.System, RoleLevelEnum.Admin, RoleLevelEnum.User)]
        public async Task<IActionResult> ReadAll([FromRoute]Guid agentId, ODataQueryOptions<Profile> queryOptions)
            => await HandleQueryable<Profile, ProfileViewModel>(await _mediator.Send(new ProfileCollection.Query(UserId, CompanyId, agentId)), queryOptions);

        #endregion
    }
}
