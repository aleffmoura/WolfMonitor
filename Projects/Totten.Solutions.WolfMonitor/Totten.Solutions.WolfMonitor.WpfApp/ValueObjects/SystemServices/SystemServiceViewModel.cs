using System;

namespace Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.SystemServices
{
    public enum EStatusService
    {
        Running,
        Stopped
    }

    public class SystemServiceViewModel
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string MonitoredAt { get; set; }

    }
}
