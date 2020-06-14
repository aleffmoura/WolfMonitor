using System;

namespace Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Archives
{
    public class ArchiveViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string AboutCurrentValue { get; set; }
        public string DisplayName { get; set; }
        public string MonitoredAt { get; set; }


        public string GetDisplayNameFormated()
        {
            if (DisplayName.Length > 15)
                return $"{DisplayName.Substring(0, 15)}...";

            return DisplayName;
        }
    }
}
