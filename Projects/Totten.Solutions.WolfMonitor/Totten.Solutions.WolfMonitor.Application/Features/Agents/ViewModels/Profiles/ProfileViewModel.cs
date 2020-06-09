using System;

namespace Totten.Solutions.WolfMonitor.Application.Features.Agents.ViewModels.Profiles
{
    public class ProfileViewModel
    {
        public Guid ProfileIdentifier { get; set; }
        public Guid AgentId { get; set; }
        public string Name { get; set; }
    }
}
