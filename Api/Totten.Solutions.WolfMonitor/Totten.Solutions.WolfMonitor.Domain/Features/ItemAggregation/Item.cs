using System;
using System.Collections.Generic;
using System.Text;
using Totten.Solutions.WolfMonitor.Domain.Base;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;

namespace Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation
{
    public class Item : Entity
    {
        public Guid AgentId { get; set; }
        public Agent Agent { get; set; }
        public string Name { get; set; }
        public ETypeItem Type { get; set; }
        public string Value { get; set; }
        public string LastValue { get; set; }
        public string DefaultValue { get; set; }

        public DateTime LastModify { get; set; }
        public DateTime LastVerify { get; set; }

        public override void Validate()
        {
        }
    }
}
