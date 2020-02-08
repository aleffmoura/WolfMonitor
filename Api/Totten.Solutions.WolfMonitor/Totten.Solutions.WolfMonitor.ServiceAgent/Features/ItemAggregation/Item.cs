using System;
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
        SystemConfig = 1
    }
    public class Item
    {
        public Guid Id { get; set; }
        public Guid AgentId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int Interval { get; set; }
        public DateTime? MonitoredAt { get; set; }
        public string Value { get; set; }
        public string Default { get; set; }
        public string LastValue { get; set; }
        public ETypeItem Type { get; set; }

        public virtual bool VerifyChanges() { return false; }

        public bool ShouldBeMonitoring() => MonitoredAt.HasValue ? MonitoredAt.Value.AddMinutes(Interval) < DateTime.Now : true;
    }
    public static class ETypeItemExtensionsMethod
    {
        public static Item GetInstance(this ETypeItem eTypeItem, Item item)
        {
            switch (eTypeItem)
            {
                case ETypeItem.SystemService: return new SystemService(item);
                case ETypeItem.SystemConfig: return new SystemConfig(item);
                default: throw new Exception("Não existe um tipo cadastrado que corresponda ao informado.");
            };
        }
    }
}
