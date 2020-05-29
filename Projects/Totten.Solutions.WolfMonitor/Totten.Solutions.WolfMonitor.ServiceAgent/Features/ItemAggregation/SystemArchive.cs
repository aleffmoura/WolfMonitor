﻿using System;
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
        }

        public override bool VerifyChanges()
        {
            var value = SystemArchivesService.GetCurrentValue(this.Name);

            if (!this.Value.Equals(value))
            {
                this.LastValue = this.Value;
                this.Value = value;
                this.MonitoredAt = DateTime.Now;
                this.AboutCurrentValue = "Alterado sistematicamente.";
                return true;
            }

            return false;
        }
        public override void Change(string newValue)
        {
            if (!string.IsNullOrEmpty(newValue) && !this.Value.Equals(newValue))
            {
                this.LastValue = this.Value;
                this.Value = newValue;
                this.MonitoredAt = DateTime.Now;
                this.AboutCurrentValue = "Alterado por solicitação";
            }
        }
    }
}