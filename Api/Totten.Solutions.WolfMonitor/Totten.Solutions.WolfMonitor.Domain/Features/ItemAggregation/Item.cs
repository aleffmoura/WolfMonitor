using System;
using System.Collections.Generic;
using System.Text;
using Totten.Solutions.WolfMonitor.Domain.Base;
using Totten.Solutions.WolfMonitor.Domain.Enums;

namespace Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation
{
    public class Item : Entity
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int Interval { get; set; }
        public string Default { get; set; }
        public string Value { get; set; }
        public string LastValue { get; set; }
        public ETypeItem Type { get; set; }

        public Guid UserIdWhoAdd { get; set; }
        public Guid AgentId { get; set; }
        
        public DateTime? MonitoredAt { get; set; }


        public List<ItemHistoric> Historic { get; set; }
        public List<ItemSolicitationHistoric> SolicitationHistoric { get; set; }

        public override void Validate() {}

        public bool VerifyIfChange(Item item) => !this.Value.Equals(item.Value);
    }
}
