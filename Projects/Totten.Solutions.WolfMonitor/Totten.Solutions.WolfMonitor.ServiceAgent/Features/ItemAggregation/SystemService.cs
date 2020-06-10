using System;
using System.ServiceProcess;
using Totten.Solutions.WolfMonitor.ServiceAgent.Infra.Features.Monitorings.VOs;
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
            this.Default = item.Default;
        }

        public override bool VerifyChanges()
        {
            var status = SystemServicesService.GetStatus(this.Name, this.DisplayName);

            if (this.Value.Equals(status))
                return false;

            this.LastValue = this.Value;
            this.Value = status;
            this.MonitoredAt = DateTime.Now;
            this.AboutCurrentValue = "Alterado sistematicamente.";
            return true;
        }

        public override bool Change(string newValue, SolicitationType solicitationType)
        {
            var returned = false;

            var status = SystemServicesService.GetStatus(this.Name, this.DisplayName);

            if (status.ToString().Equals(newValue))
                return returned;

            if (ServiceControllerStatus.Stopped.ToString().Equals(newValue))
                returned = SystemServicesService.Stop(this.Name, this.DisplayName);
            else if (ServiceControllerStatus.Running.ToString().Equals(newValue))
                returned = SystemServicesService.Start(this.Name, this.DisplayName);

            if (returned)
            {
                this.LastValue = this.Value;
                this.Value = SystemServicesService.GetStatus(this.Name, this.DisplayName);
                this.MonitoredAt = DateTime.Now;
                this.AboutCurrentValue = solicitationType.ToString();
            }

            return returned;
        }
    }
}
