using System;

namespace Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.SystemServices
{
    public enum EStatusService
    {
        Running,
        Stopped,
        Active,
        Inactive,
        Failed
    }

    public class SystemServiceViewModel
    {
        public Guid Id { get; set; }
        public string DisplayName
        {
            get;
            set;
        }
        public string Name { get; set; }
        public string Value { get; set; }
        public string MonitoredAt { get; set; }

        public string GetDisplayNameFormated()
        {
            if (DisplayName.Length > 9)
                return $"{DisplayName.Substring(0, 9)}...";

            return DisplayName;
        }
        public string GetNameFormated()
        {
            if (Name.Length > 12)
                return $"{Name.Substring(0, 12)}...";

            return DisplayName;
        }

    }
}
