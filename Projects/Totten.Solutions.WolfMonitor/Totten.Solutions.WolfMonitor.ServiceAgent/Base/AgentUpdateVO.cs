using System;
using System.Collections.Generic;
using System.Text;

namespace Totten.Solutions.WolfMonitor.ServiceAgent.Base
{
    public class AgentUpdateVO
    {
        public string MachineName { get; set; }
        public string LocalIp { get; set; }
        public string HostName { get; set; }
        public string HostAddress { get; set; }
    }
}
