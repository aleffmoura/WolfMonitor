using System;

namespace Totten.Solutions.WolfMonitor.ServiceAgent.Base
{
    public class Agent
    {
        public Guid Id { get; set; }
        public string ProfileName { get; set; }
        public string ProfileIdentifier { get; set; }
        public bool Configured { get; set; }
    }
}
