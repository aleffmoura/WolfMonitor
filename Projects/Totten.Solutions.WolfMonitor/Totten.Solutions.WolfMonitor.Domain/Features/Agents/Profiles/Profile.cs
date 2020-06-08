using System;
using Totten.Solutions.WolfMonitor.Domain.Base;

namespace Totten.Solutions.WolfMonitor.Domain.Features.Agents.Profiles
{
    public class Profile : Entity
    {
        public Guid ProfileIdentifier { get; set; }
        public Guid ItemId { get; set; }
        public Guid AgentId { get; set; }
        public Guid CompanyId { get; set; }
        public Guid UserWhoCreatedId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public override void Validate()
        {
        }
    }
}
