using System;
using System.Collections.Generic;
using System.Text;
using Totten.Solutions.WolfMonitor.Domain.Base;

namespace Totten.Solutions.WolfMonitor.Domain.Features.SystemServices
{
    public class SystemService : Entity
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Value { get; set; }


        public Guid UserIdWhoAdd { get; set; }
        public Guid AgentId { get; set; }

        public override void Validate()
        {
        }
    }
}
