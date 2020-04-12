using System;

namespace Totten.Solutions.WolfMonitor.Agents.Commands
{
    public class ChangeServiceStatusCommand
    {
        public Guid AgentID  { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string NewStatus { get; set; }
    }
}
