using System;

namespace Totten.Solutions.WolfMonitor.Monitoring.Commands
{
    public class SystemServiceCreateCommand
    {
        public Guid AgentId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}
