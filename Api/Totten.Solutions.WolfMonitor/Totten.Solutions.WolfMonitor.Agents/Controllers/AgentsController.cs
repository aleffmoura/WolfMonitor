using MediatR;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Application.Features.Agents.Handlers;
using Totten.Solutions.WolfMonitor.Application.Features.Agents.ViewModels;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Base;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Filters;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;

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
        public async Task<IActionResult> Create([FromBody]AgentCreate.Command command)
        {
            command.UserWhoCreated = UserId;
            return HandleCommand(await _mediator.Send(command));
        }
        #endregion

        #region HTTP PATCH
        [HttpPatch]
        public async Task<IActionResult> PatchClient([FromBody]AgentUpdate.Command command)
        {
            return HandleCommand(await _mediator.Send(command));
        }
        #endregion

        #region HTTP GET
        [HttpGet("{companyId}")]
        [ODataQueryOptionsValidate]
        public async Task<IActionResult> ReadAll([FromRoute]string companyId, ODataQueryOptions<Agent> queryOptions)
        {
            return await HandleQueryable<Agent, AgentResumeViewModel>(await _mediator.Send(new AgentCollection.Query(companyId)), queryOptions);
        }
        [HttpGet("{companyId}/{agentId}")]
        public async Task<IActionResult> ReadById([FromRoute]Guid companyId, [FromRoute]Guid agentId)
        {
            return HandleQuery<Agent, AgentDetailViewModel>(await _mediator.Send(new AgentResume.Query(companyId, agentId)));
        }
        #endregion
    }
}
