using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Totten.Solutions.WolfMonitor.SignalR.Hubs.Agents
{
    public class AgentHub : Hub
    {
        /// <summary>
        /// Lista para gerenciar os agentes conectados
        /// </summary>
        public static readonly ConcurrentDictionary<string, AgentConnected> HubConnectedAgents = new ConcurrentDictionary<string, AgentConnected>();

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();

            var connectedAgent = new AgentConnected { Token = GetAgentTokenFromRequest(), ConnectId = Context.ConnectionId };

            HubConnectedAgents.TryAdd(connectedAgent.Token, connectedAgent);
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await base.OnDisconnectedAsync(ex);

            HubConnectedAgents.TryRemove(GetAgentTokenFromRequest(), out _);
        }

        /// <summary>
        /// Obtém o AgentToken que foi passado como parâmetro na rota
        /// </summary>
        /// <returns>AgentToken</returns>
        private string GetAgentTokenFromRequest()
        {
            var httpContext = Context.GetHttpContext();

            return httpContext.Request.Query["AgentId"].ToString();
        }
    }
}
