using System;
using System.Collections.Generic;
using System.Text;
using Totten.Solutions.WolfMonitor.Domain.Enums;

namespace Totten.Solutions.WolfMonitor.WpfApp.ValueObjects
{
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
