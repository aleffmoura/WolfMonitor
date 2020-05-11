using System;
using System.Collections.Generic;

namespace Totten.Solutions.WolfMonitor.Application.Features.Agents.ViewModels
{
    public class AgentResumeViewModel
    {
        public AgentResumeViewModel()
        {
            Items = new Dictionary<int, int>();
        }

        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string Company { get; set; }
        public string UserWhoCreated { get; set; }
        public string CreatedIn { get; set; }
        public string LastUpdate { get; set; }
        public Dictionary<int, int> Items { get; set; }
    }
}
