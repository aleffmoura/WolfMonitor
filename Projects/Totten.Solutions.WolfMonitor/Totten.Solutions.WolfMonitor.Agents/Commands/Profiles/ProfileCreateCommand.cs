using System;

namespace Totten.Solutions.WolfMonitor.Agents.Commands.Profiles
{
    public class ProfileCreateCommand
    {
        public Guid AgentId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
