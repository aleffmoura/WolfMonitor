using System;

namespace Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Items
{
    public class ItemHistoricViewModel
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public string MonitoredAt { get; set; }
    }
}
