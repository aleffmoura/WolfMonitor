using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Totten.Solutions.WolfMonitor.Monitoring.Controllers
{
    public class ItemUpdateVO
    {
        public Guid AgentId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string LastValue { get; set; }
        public DateTime MonitoredAt { get; set; }
    }
}
