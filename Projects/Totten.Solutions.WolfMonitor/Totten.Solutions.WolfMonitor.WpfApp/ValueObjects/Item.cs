using System;

namespace Totten.Solutions.WolfMonitor.WpfApp.ValueObjects
{
    public enum ETypeItem
    {
        SystemService = 0,
        SystemConfig = 1,
        SystemArchive = 2
    }
    public class Item
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string AboutCurrentValue { get; set; }
        public ETypeItem Type { get; set; }
        public Guid AgentId { get; set; }
    }
}
