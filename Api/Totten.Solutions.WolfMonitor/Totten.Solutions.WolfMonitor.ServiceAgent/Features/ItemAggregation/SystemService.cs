﻿using System;
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
            this.Default = item.Default;
            this.DisplayName = item.DisplayName;
            this.LastValue = item.LastValue;
            this.Interval = item.Interval;
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
            return true;
        }

        public override void Change(string newStatus)
        {
            if (ServiceControllerStatus.Running.ToString().Equals(this.Value))
                SystemServicesService.Stop(this.Name, this.DisplayName);
            else
                SystemServicesService.Start(this.Name, this.DisplayName);
        }
    }
}
