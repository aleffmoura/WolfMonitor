using System;
using System.Collections.Generic;
using System.Text;

namespace Totten.Solutions.WolfMonitor.WpfApp.ViewModels.Agents
{
    public class AgentViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime LastConnection { get; set; }
        public DateTime LastUpdated { get; set; }
        public string CreatedBy { get; set; }
    }
}
