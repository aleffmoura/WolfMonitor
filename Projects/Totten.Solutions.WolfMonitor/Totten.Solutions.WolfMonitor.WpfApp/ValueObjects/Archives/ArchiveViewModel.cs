using System;

namespace Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Archives
{
    public class ArchiveViewModel
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string MonitoredAt { get; set; }
    }
}
