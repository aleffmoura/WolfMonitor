using System;

namespace Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Agents.Profiles
{
    public class ProfileViewItem
    {
        public Guid ProfileIdentifier { get; set; }
        public Guid AgentId { get; set; }
        public Guid ItemId { get; set; }
        public string Name { get; set; }
    }

    public class ProfileViewModel
    {
        public Guid ProfileIdentifier { get; set; }
        public ProfileViewItem ProfileViewItem { get; set; }

    }
}
