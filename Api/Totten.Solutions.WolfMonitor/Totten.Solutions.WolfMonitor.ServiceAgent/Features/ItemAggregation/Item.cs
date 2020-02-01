using System;
using System.Collections.Generic;
using System.Text;

namespace Totten.Solutions.WolfMonitor.ServiceAgent.Features.ItemAggregation
{
    public abstract class Item
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Interval { get; set; }
        public string DefaultValue { get; set; }
        public string Value { get; set; }

        public abstract bool VerifyIfChange();
    }
}
