using System;
using System.Collections.Generic;
using System.Text;

namespace Totten.Solutions.WolfMonitor.Application.Features.Agents.ViewModels
{
    public class AgentResumeViewModel
    {
        public Guid Id { get; set; }
        public string Company { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedIn { get; set; }
        public string LastUpdate { get; set; }
    }
}
