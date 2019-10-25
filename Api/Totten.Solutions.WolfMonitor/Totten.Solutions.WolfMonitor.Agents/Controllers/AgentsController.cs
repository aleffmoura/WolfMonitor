using MediatR;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Application.Features.Agents.Handlers;
using Totten.Solutions.WolfMonitor.Application.Features.Agents.ViewModels;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Base;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Filters;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation;

namespace Totten.Solutions.WolfMonitor.Agents.Controllers
{
    [Route("clients")]
    public class AgentsController : ApiControllerBase
    {
        private IMediator _mediator;

        public AgentsController(IMediator mediator) => _mediator = mediator;

        #region HTTP POST
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]AgentCreate.Command command)
            => HandleCommand(await _mediator.Send(command));
        #endregion

        #region HTTP PATCH
        [HttpPatch]
        public async Task<IActionResult> PatchClient([FromBody]ClientUpdate.Command command)
            => HandleCommand(await _mediator.Send(command));
        #endregion

        #region HTTP GET
        [HttpGet("{companyId}")]
        [ODataQueryOptionsValidate]
        public async Task<IActionResult> Get([FromRoute]string companyId, ODataQueryOptions<Agent> queryOptions)
            => await HandleQueryable<Agent, Agent>(await _mediator.Send(new AgentCollection.Query(companyId)), queryOptions);

        [HttpGet("{id}")]
        public async Task<IActionResult> ReadById([FromRoute]string id)
            => HandleQuery<Agent, AgentConfigViewModel>(await _mediator.Send(new AgentResume.Query(id)));

        [HttpGet("{agentId}/items")]
        [ODataQueryOptionsValidate]
        public async Task<IActionResult> GetItems([FromRoute]string agentId, ODataQueryOptions<Agent> queryOptions)
            => await HandleQueryable<Agent, Item>(await _mediator.Send(new AgentCollection.Query(agentId)), queryOptions);

        #endregion
    }
}
