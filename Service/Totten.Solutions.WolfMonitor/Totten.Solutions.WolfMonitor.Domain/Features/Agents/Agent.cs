using System;
using System.Collections.Generic;
using System.Text;
using Totten.Solutions.WolfMonitor.Domain.Base;

namespace Totten.Solutions.WolfMonitor.Domain.Features.Agents
{
    public class Agent : Entity
    {
        public Guid CompanyId { get; set; }
        public string HostName { get; set; }
        public string HostAddress { get; set; }
        public string Name { get; set; }
        public string LocalIp { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
