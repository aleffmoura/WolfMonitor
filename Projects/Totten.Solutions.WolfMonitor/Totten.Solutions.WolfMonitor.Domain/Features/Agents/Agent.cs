using System;
using System.Collections.Generic;
using Totten.Solutions.WolfMonitor.Domain.Base;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents.Profiles;
using Totten.Solutions.WolfMonitor.Domain.Features.Companies;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;

namespace Totten.Solutions.WolfMonitor.Domain.Features.Agents
{
    public class Agent : Entity
    {
        public Agent()
        {
            Items = new Dictionary<int, int>();
        }
        public Guid CompanyId { get; set; }
        public Guid UserWhoCreatedId { get; set; }
        public string UserWhoCreatedName { get; set; }
        public string DisplayName { get; set; }
        public string MachineName { get; set; }
        public string LocalIp { get; set; }
        public string HostName { get; set; }
        public string HostAddress { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }
        public bool Configured { get; set; }
        public bool ReadItemsMonitoringByArchive { get; set; }

        public DateTime? FirstConnection { get; set; }
        public DateTime? LastConnection { get; set; }
        public DateTime? LastUpload { get; set; }

        public Guid ProfileIdentifier { get; set; }
        public string ProfileName { get; set; }

        public virtual Company Company { get; set; }
        public virtual User UserWhoCreated { get; set; }


        public Dictionary<int, int> Items { get; set; }

        public override void Validate() { }
    }
}
