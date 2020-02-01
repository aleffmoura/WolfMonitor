using System;
using Totten.Solutions.WolfMonitor.ServiceAgent.Services;

namespace Totten.Solutions.WolfMonitor.ServiceAgent.Features.ItemAggregation
{
    public class SystemService : Item
    {

        public SystemService() { }

        public SystemService(Item item)
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
            var status = SystemServicesService.GetStatus(this.Name, this.DisplayName);
            if (this.Value.Equals(status))
            {
                return false;
            }
            this.LastValue = this.Value;
            this.Value = status;
            this.MonitoredAt = DateTime.Now;
            return true;
        }
    }
}
