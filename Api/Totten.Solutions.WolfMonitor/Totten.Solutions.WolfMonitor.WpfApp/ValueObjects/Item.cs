using System;
using System.Collections.Generic;
using System.Text;

namespace Totten.Solutions.WolfMonitor.WpfApp.ValueObjects
{
    public enum ETypeItem
    {
        SystemService = 0,
        SystemConfig = 1
    }
    public class Item
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int Interval { get; set; }
        public string Default { get; set; }
        public ETypeItem Type { get; set; }
        public Guid AgentId { get; set; }
    }
}
