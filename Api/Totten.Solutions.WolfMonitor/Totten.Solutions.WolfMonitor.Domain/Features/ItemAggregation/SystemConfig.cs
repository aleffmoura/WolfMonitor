using System;
using System.Collections.Generic;
using System.Text;

namespace Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation
{
    public class SystemConfig : Item
    {
        public SystemConfig() { }
        public SystemConfig(Item item)
        {
            this.Id = item.Id;
            this.Name = item.Name;
            this.DisplayName = item.Name;
            this.Default = item.Default;
            this.LastValue = item.LastValue;
            this.Interval = item.Interval;
            this.Type = item.Type;
            this.Value = item.Value;
            this.Removed = item.Removed;
            this.UpdatedIn = item.UpdatedIn;
            this.UserIdWhoAdd = item.UserIdWhoAdd;
            this.AgentId = item.AgentId;
        }
        public override void Validate() { }
    }
}
