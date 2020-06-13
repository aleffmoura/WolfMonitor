using MediatR;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Agents.Commands;
using Totten.Solutions.WolfMonitor.Application.Features.Agents.Handlers;
using Totten.Solutions.WolfMonitor.Application.Features.Agents.ViewModels;
using Totten.Solutions.WolfMonitor.Application.Features.Monitoring.Handlers.Items;
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
            => _mediator = mediator;

        #region HTTP Delete

        [HttpDelete("{Id}")]
        [CustomAuthorizeAttributte(RoleLevelEnum.System, RoleLevelEnum.Admin)]
        public async Task<IActionResult> RemoveItem([FromRoute]Guid id)
            => HandleCommand(await _mediator.Send(new AgentRemove.Command(id, CompanyId, UserId)));

        #endregion

        #region HTTP POST
        [HttpPost]
        [CustomAuthorizeAttributte(RoleLevelEnum.System, RoleLevelEnum.Admin)]
        public async Task<IActionResult> Create([FromBody]AgentCreateCommand command)
            => HandleCommand(await _mediator.Send(new AgentCreate.Command(CompanyId, UserId, command.DisplayName,
                                                                            command.Login, command.Password,
                                                                            command.ReadItemsMonitoringByArchive)));

        #endregion

        #region HTTP PATCH

        [HttpPatch("item-solicitation")]
        [CustomAuthorizeAttributte(RoleLevelEnum.System, RoleLevelEnum.Admin)]
        public async Task<IActionResult> ChangeValue([FromBody]SolicitationCommand command)
            => HandleCommand(await _mediator.Send(new ItemSolicitationHistoricCreate.Command(UserId, command.AgentId,
                             CompanyId, command.ItemId, command.SolicitationType, command.Name, command.DisplayName, command.NewValue)));

        [HttpPatch]
        [CustomAuthorizeAttributte(RoleLevelEnum.Agent)]
        public async Task<IActionResult> PatchClient([FromBody]AgentUpdateCommand command)
            => HandleCommand(await _mediator.Send(new AgentUpdate.Command(UserId, command.MachineName, command.LocalIp, command.HostName, command.HostAddress)));

        #endregion

        #region HTTP GET
        [HttpGet()]
        [ODataQueryOptionsValidate(AllowedQueryOptions.Top | AllowedQueryOptions.Skip | AllowedQueryOptions.Count)]
        [CustomAuthorizeAttributte(RoleLevelEnum.System, RoleLevelEnum.Admin, RoleLevelEnum.User)]
        public async Task<IActionResult> ReadAll(ODataQueryOptions<Agent> queryOptions)
            => await HandleQueryable<Agent, AgentResumeViewModel>(await _mediator.Send(new AgentCollection.Query(CompanyId)), queryOptions);

        [HttpGet("{agentId}")]
        [CustomAuthorizeAttributte(RoleLevelEnum.System, RoleLevelEnum.Admin, RoleLevelEnum.User)]
        public async Task<IActionResult> ReadById([FromRoute]Guid agentId)
            => HandleQuery<Agent, AgentDetailViewModel>(await _mediator.Send(new AgentResume.Query(agentId, CompanyId, UserId)));

        [HttpGet("info")]
        [CustomAuthorizeAttributte(RoleLevelEnum.Agent)]
        public async Task<IActionResult> ReadInfoAgent()
            => HandleQuery<Agent, AgentDetailViewModel>(await _mediator.Send(new AgentResume.Query(UserId, CompanyId, UserId)));
        #endregion
    }
}
