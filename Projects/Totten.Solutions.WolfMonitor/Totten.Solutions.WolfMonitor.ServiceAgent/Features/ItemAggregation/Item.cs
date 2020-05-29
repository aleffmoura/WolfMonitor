﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Totten.Solutions.WolfMonitor.ServiceAgent.Features.ItemAggregation
{
    public enum ETypeItem
    {
        [Description("Serviço")]
        SystemService = 0,
        [Description("Arquivo")]
        SystemArchive = 1
    }
    public class Item
    {
        public Guid Id { get; set; }
        public Guid AgentId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public DateTime? MonitoredAt { get; set; }
        public string Value { get; set; }
        public string AboutCurrentValue { get; set; }
        public string LastValue { get; set; }
        public ETypeItem Type { get; set; }

        public virtual bool VerifyChanges() { return false; }
        public virtual void Change(string newStatus) { }

    }
    public static class ETypeItemExtensionsMethod
    {
        public static Item GetInstance(this ETypeItem eTypeItem, Item item)
        {
            switch (eTypeItem)
            {
                case ETypeItem.SystemService: return new SystemService(item);
                case ETypeItem.SystemArchive: return new SystemArchive(item);
                default: throw new Exception("Não existe um tipo cadastrado que corresponda ao informado.");
            };
        }
    }
}
