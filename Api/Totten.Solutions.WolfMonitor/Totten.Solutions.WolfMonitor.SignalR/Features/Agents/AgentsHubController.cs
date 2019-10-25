using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Base;
using Totten.Solutions.WolfMonitor.SignalR.Features.Agents.Commands;
using Totten.Solutions.WolfMonitor.SignalR.Hubs.Agents;

namespace Totten.Solutions.WolfMonitor.SignalR.Features.Agents
{
    [Route("agents")]
    public class AgentsHubController : ApiControllerBase
    {
        private readonly IHubContext<AgentHub> _agentHubContext;
        public AgentsHubController(IHubContext<AgentHub> agentsHubContext)
        {
            _agentHubContext = agentsHubContext;
        }
        [HttpPost]
        [Route("request-log")]
        public IActionResult RequestAgentLog([FromHeader] AgentRequestLogCommand command)
        {
            var validator = command.Validate();

            if (!validator.IsValid)
                return HandleValidationFailure(validator.Errors);

            AgentConnected connectedAgent;
            if (AgentHub.HubConnectedAgents.TryGetValue(command.AgentToken, out connectedAgent))
            {
                _agentHubContext.Clients.Client(connectedAgent.ConnectId).SendAsync("RequestLog", command.ClientToken);

                return Ok();
            }
            else
            {
                return StatusCode(HttpStatusCode.NotFound.GetHashCode(), "Agent not found on the agents connected");
            }
        }

        [HttpGet]
        [Route("{agentToken}/logs")]
        public IActionResult LogConfigUpdated([FromRoute] string agentToken)
        {
            AgentConnected connectedAgent;
            if (AgentHub.HubConnectedAgents.TryGetValue(agentToken, out connectedAgent))
            {
                _agentHubContext.Clients.Client(connectedAgent.ConnectId).SendAsync("LogConfigUpdated");
            }

            return Ok();
        }
    }
}
