using System;
using System.Collections.Generic;
using System.Text;

namespace Totten.Solutions.WolfMonitor.Domain.Features.Agents
{
    public class AgentConfig
    {
        public string CompanyId { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string ApiUrl { get; set; }
    }
}
