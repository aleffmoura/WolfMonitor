using System;
using Totten.Solutions.WolfMonitor.ServiceAgent.Infra.Features.Monitorings.VOs;
using Totten.Solutions.WolfMonitor.ServiceAgent.Services;

namespace Totten.Solutions.WolfMonitor.ServiceAgent.Features.ItemAggregation
{
    public class SystemArchive : Item
    {

        public SystemArchive() { }

        public SystemArchive(Item item)
        {
            this.Id = item.Id;
            this.AgentId = item.AgentId;
            this.Name = item.Name;
            this.DisplayName = item.Name;
            this.AboutCurrentValue = item.AboutCurrentValue;
            this.LastValue = item.LastValue;
            this.Type = item.Type;
            this.Value = item.Value;
            this.Default = item.Default;
        }

        public override bool VerifyChanges()
        {
            if (this.NextMonitoring.HasValue && DateTime.Now < this.NextMonitoring.Value)
                return false;

            var value = SystemArchivesService.GetCurrentValue(this.Name);

            if (this.Value.Equals(value))
                return false;

            this.LastValue = this.Value;
            this.Value = value;
            this.MonitoredAt = DateTime.Now;
            this.AboutCurrentValue = "Alterado sistematicamente.";
            this.NextMonitoring = null;
            return true;

        }
        public override bool Change(string newValue, SolicitationType solicitationType)
        {
            if (this.NextMonitoring.HasValue && DateTime.Now < this.NextMonitoring.Value)
                return false;

            if (!string.IsNullOrEmpty(newValue) && !this.Value.Equals(newValue))
            {
                SystemArchivesService.ChangeValue(this.Name, newValue: newValue);

                this.LastValue = this.Value;
                this.Value = newValue;
                this.MonitoredAt = DateTime.Now;
                this.AboutCurrentValue = solicitationType.ToString();
                this.NextMonitoring = null;
                return true;
            }

            return false;
        }
    }
}
