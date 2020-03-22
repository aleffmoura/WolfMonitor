using System;
using System.Collections.Generic;
using System.Text;
using Totten.Solutions.WolfMonitor.Domain.Base;
using Totten.Solutions.WolfMonitor.Domain.Enums;

namespace Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation
{
    public class ItemHistoric : Entity
    {
        public Guid ItemId { get; set; }
        public string Value { get; set; }
        public string MonitoredAt { get; set; }

        public override void Validate() { }
    }
}
