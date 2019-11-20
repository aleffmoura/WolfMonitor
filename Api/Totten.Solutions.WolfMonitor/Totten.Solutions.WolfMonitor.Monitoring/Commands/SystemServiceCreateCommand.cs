using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Totten.Solutions.WolfMonitor.Monitoring.Commands
{
    public class SystemServiceCreateCommand
    {
        public Guid AgentId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}
