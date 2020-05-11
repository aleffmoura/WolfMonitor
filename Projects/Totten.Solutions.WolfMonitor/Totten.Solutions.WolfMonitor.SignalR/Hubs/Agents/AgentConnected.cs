using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Totten.Solutions.WolfMonitor.SignalR.Hubs.Agents
{
    public class AgentConnected
    {
        public string Token { get; set; }
        public string ConnectId { get; set; }
    }
}
