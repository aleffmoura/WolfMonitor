using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Totten.Solutions.WolfMonitor.Monitoring.Controllers
{
    public class ItemCreateVO
    {
        public Guid AgentId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string AboutCurrentValue { get; set; }
    }
}
