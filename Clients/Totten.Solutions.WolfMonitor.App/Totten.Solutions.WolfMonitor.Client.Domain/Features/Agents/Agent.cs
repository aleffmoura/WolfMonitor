using System;
using System.Collections.Generic;
using System.Text;

namespace Totten.Solutions.WolfMonitor.Client.Domain.Features.Agents
{
    public class Agent
    {
        public string Name { get; set; }
        public string LocalIp { get; set; }
        public string HostName { get; set; }
        public string HostAddress { get; set; }
        public bool Configured { get; set; }
    }
}
