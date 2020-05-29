using System;
using System.ServiceProcess;
using Totten.Solutions.WolfMonitor.ServiceAgent.Services;

namespace Totten.Solutions.WolfMonitor.ServiceAgent.Features.ItemAggregation
{
    public class SystemService : Item
    {

        public SystemService() { }

        public SystemService(Item item)
        {
            this.Id = item.Id;
            this.AgentId = item.AgentId;
            this.Name = item.Name;
            this.AboutCurrentValue = item.AboutCurrentValue;
            this.DisplayName = item.DisplayName;
            this.LastValue = item.LastValue;
            this.MonitoredAt = item.MonitoredAt;
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
            this.AboutCurrentValue = "Alterado sistematicamente.";
            return true;
        }

        public override void Change(string newValue)
        {
            if (ServiceControllerStatus.Running.ToString().Equals(this.Value))
                SystemServicesService.Stop(this.Name, this.DisplayName);
            else
                SystemServicesService.Start(this.Name, this.DisplayName);

            this.LastValue = this.Value;
            this.Value = SystemServicesService.GetStatus(this.Name, this.DisplayName);
            this.MonitoredAt = DateTime.Now;
            this.AboutCurrentValue = "Alterado por solicitação";
        }
    }
}
