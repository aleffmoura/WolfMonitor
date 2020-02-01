using System;
using System.Collections.Generic;
using System.Text;

namespace Totten.Solutions.WolfMonitor.ServiceAgent.Base
{
    public class Agent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LocalIp { get; set; }
        public string HostName { get; set; }
        public string HostAddress { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }
        public bool Configured { get; set; }


        public string urlApi { get; set; }
        public string Company { get; set; }
    }
}
