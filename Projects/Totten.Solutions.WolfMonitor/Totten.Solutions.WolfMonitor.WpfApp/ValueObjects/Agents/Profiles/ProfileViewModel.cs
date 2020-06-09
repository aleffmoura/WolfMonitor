using System;
using System.Collections.Generic;

namespace Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Agents.Profiles
{
    public class ProfileViewItem
    {
        public Guid ProfileIdentifier { get; set; }
        public Guid AgentId { get; set; }
        public string Name { get; set; }

        public override string ToString() => Name;
    }

    public class ProfileViewModel
    {
        public string Name { get; set; }
        public List<ProfileViewItem> ProfileViewItem { get; set; }

        public override string ToString() => Name;
    }
}
