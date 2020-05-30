using System;

namespace Totten.Solutions.WolfMonitor.Monitoring.Controllers
{
    public class ItemUpdateVO
    {
        public Guid Id { get; set; }
        public Guid AgentId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string LastValue { get; set; }
        public string AboutCurrentValue { get; set; }
        public DateTime MonitoredAt { get; set; }
    }
}
