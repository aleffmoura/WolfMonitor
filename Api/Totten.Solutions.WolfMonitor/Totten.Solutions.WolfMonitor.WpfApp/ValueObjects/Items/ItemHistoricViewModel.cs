using System;
using System.Collections.Generic;
using System.Text;

namespace Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Items
{
    public class ItemHistoricViewModel
    {
        public Guid ItemId { get; set; }
        public string Value { get; set; }
        public string MonitoredAt { get; set; }
    }
}
