using System;
using System.ServiceProcess;
using Totten.Solutions.WolfMonitor.ServiceAgent.Infra.Features.Monitorings.VOs;
using Totten.Solutions.WolfMonitor.ServiceAgent.Services;
using static Totten.Solutions.WolfMonitor.ServiceAgent.Services.LinuxService;

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
            if (this.NextMonitoring.HasValue && DateTime.Now < this.NextMonitoring.Value)
                return false;

            var status = SystemServicesController.GetStatus(this.Name, this.DisplayName);

            if (this.Value.Equals(status))
                return false;

            this.LastValue = this.Value;
            this.Value = status;
            this.MonitoredAt = DateTime.Now;
            this.AboutCurrentValue = "Alterado sistematicamente.";
            this.NextMonitoring = null;

            return true;
        }

        public override bool Change(string newValue, SolicitationType solicitationType)
        {
            if (this.NextMonitoring.HasValue && DateTime.Now < this.NextMonitoring.Value && solicitationType != SolicitationType.ChangeStatus)
                return false;

            var returned = false;

            var status = SystemServicesController.GetStatus(this.Name, this.DisplayName);

            if (status.ToString().Equals(newValue))
                return returned;

            if (ServiceControllerStatus.Running.ToString().Equals(newValue) ||
                StatusLinux.Active.ToString().Equals(newValue) || StatusLinux.Failed.ToString().Equals(newValue))
                returned = SystemServicesController.Start(this.Name, this.DisplayName);

            else if (ServiceControllerStatus.Stopped.ToString().Equals(newValue) ||
                     StatusLinux.Inactive.ToString().Equals(newValue))
                returned = SystemServicesController.Stop(this.Name, this.DisplayName);

            var statusAfterCommand = SystemServicesController.GetStatus(this.Name, this.DisplayName);

            if (returned || !statusAfterCommand.Equals(this.Value))
            {
                this.LastValue = this.Value;
                this.Value = statusAfterCommand;
                this.MonitoredAt = DateTime.Now;
                this.AboutCurrentValue = solicitationType.ToString();
                this.NextMonitoring = null;
            }

            return returned;
        }
    }
}
