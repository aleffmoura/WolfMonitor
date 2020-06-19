using System;
using System.Collections.Generic;

namespace Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Agents
{
    public class AgentResumeViewModel
    {
        public AgentResumeViewModel()
        {
            Items = new Dictionary<int, int>();
        }

        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string UserWhoCreated { get; set; }
        public string CreatedIn { get; set; }
        public string LastUpdate { get; set; }
        public Dictionary<int, int> Items { get; set; }

        public string GetDisplayNameFormated()
        {
            if (DisplayName.Length > 12)
                return $"{DisplayName.Substring(0, 12)}...";

            return DisplayName;
        }
    }
}
