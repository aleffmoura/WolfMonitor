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
        SystemArchive = 1
    }
    public abstract class Item
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int Interval { get; set; }
        public DateTime? MonitoredAt { get; set; }
        public string DefaultValue { get; set; }
        public string Value { get; set; }
        public string Default { get; set; }
        public string LastValue { get; set; }
        public ETypeItem Type { get; set; }

        public abstract bool VerifyChanges();

        public bool ShouldBeMonitoring() => MonitoredAt.HasValue ? MonitoredAt.Value.AddMinutes(Interval) < DateTime.Now : true;
    }
    public static class ETypeItemExtensionsMethod
    {
        public static Item GetInstance(this ETypeItem eTypeItem, Item item) =>
            eTypeItem switch
            {
                ETypeItem.SystemService => new SystemService(item),
                ETypeItem.SystemArchive => new SystemArchive(item),
                _ => throw new Exception("Não existe um tipo cadastrado que corresponda ao informado.")
            };
    }
}
