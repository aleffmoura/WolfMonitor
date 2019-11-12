using System;
using Totten.Solutions.WolfMonitor.Domain.Base;
using Totten.Solutions.WolfMonitor.Domain.Features.Companies;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;

namespace Totten.Solutions.WolfMonitor.Domain.Features.Agents
{
    public class Agent : Entity
    {
        public Guid CompanyId { get; set; }
        public Guid UserWhoCreatedId { get; set; }

        public string Name { get; set; }
        public string LocalIp { get; set; }
        public string HostName { get; set; }
        public string HostAddress { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }
        public bool Configured { get; set; }

        public DateTime? FirstConnection { get; set; }
        public DateTime? LastConnection { get; set; }
        public DateTime? LastUpload { get; set; }
        //public List<Item> Items { get; set; }

        public Company Company { get; set; }
        public User UserWhoCreated { get; set; }

        public override void Validate() { }
    }
}
