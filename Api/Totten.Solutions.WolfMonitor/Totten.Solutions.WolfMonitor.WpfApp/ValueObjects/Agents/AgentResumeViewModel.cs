using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
