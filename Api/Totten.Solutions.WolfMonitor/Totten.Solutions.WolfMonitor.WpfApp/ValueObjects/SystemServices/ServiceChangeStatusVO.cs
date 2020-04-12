using System;
using System.Collections.Generic;
using System.Text;

namespace Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.SystemServices
{
    public class ServiceChangeStatusVO
    {
        public Guid AgentID { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string NewStatus { get; set; }
    }
}
