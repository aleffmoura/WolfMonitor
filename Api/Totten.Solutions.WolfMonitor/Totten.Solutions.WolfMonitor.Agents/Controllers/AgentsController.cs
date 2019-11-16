using MediatR;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Agents.Commands;
using Totten.Solutions.WolfMonitor.Application.Features.Agents.Handlers;
using Totten.Solutions.WolfMonitor.Application.Features.Agents.ViewModels;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Base;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Filters;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;

namespace Totten.Solutions.WolfMonitor.Agents.Controllers
{
    [Route("")]
    public class AgentsController : ApiControllerBase
    {
        private IMediator _mediator;

        public AgentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region HTTP POST
        [HttpPost]
        [CustomAuthorizeAttributte(RoleLevelEnum.System, RoleLevelEnum.Admin)]
        public async Task<IActionResult> Create([FromBody]AgentCreateCommand command)
        {
            return HandleCommand(await _mediator.Send(new AgentCreate.Command(CompanyId, UserId, command.Login, command.Password)));
        }
        #endregion

        #region HTTP PATCH
        [HttpPatch]
        [CustomAuthorizeAttributte(RoleLevelEnum.Agent)]
        public async Task<IActionResult> PatchClient([FromBody]AgentUpdateCommand command)
        {
            return HandleCommand(await _mediator.Send(new AgentUpdate.Command(UserId, command.Name, command.LocalIp, command.HostName, command.HostAddress)));
        }
        #endregion

        #region HTTP GET
        [ODataQueryOptionsValidate]
        [CustomAuthorizeAttributte(RoleLevelEnum.System, RoleLevelEnum.Admin, RoleLevelEnum.User)]
        public async Task<IActionResult> ReadAll(ODataQueryOptions<Agent> queryOptions)
        {
            return await HandleQueryable<Agent, AgentResumeViewModel>(await _mediator.Send(new AgentCollection.Query(CompanyId)), queryOptions);
        }
        [HttpGet("{agentId}")]
        [CustomAuthorizeAttributte(RoleLevelEnum.System, RoleLevelEnum.Admin, RoleLevelEnum.User)]
        public async Task<IActionResult> ReadById([FromRoute]Guid agentId)
        {
            return HandleQuery<Agent, AgentDetailViewModel>(await _mediator.Send(new AgentResume.Query(CompanyId, agentId)));
        }
        [HttpGet("info")]
        [CustomAuthorizeAttributte(RoleLevelEnum.Agent)]
        public async Task<IActionResult> ReadInfoAgent()
        {
            return HandleQuery<Agent, AgentDetailViewModel>(await _mediator.Send(new AgentResume.Query(CompanyId, UserId)));
        }
        #endregion
    }
}
