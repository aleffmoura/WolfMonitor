using System;
using System.Collections.Generic;
using System.Text;
using Totten.Solutions.WolfMonitor.ServiceAgent.Services;

namespace Totten.Solutions.WolfMonitor.ServiceAgent.Features.ItemAggregation
{
    public class SystemArchive : Item
    {

        public SystemArchive() { }

        public SystemArchive(Item item)
        {
            this.Id = item.Id;
            this.Name = item.Name;
            this.DisplayName = item.Name;
            this.Default = item.Default;
            this.LastValue = item.LastValue;
            this.Interval = item.Interval;
            this.Type = item.Type;
            this.Value = item.Value;
        }

        public override bool VerifyChanges()
        {
            var value = SystemArchiveService.GetCurrentValue(this.Name);

            if (!this.Value.Equals(value))
            {
                this.LastValue = this.Value;
                this.Value = value;
                this.MonitoredAt = DateTime.Now;
                return true;
            }

            return false;
        }
    }
}
