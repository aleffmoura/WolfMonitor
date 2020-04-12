using System;
using System.Collections.Generic;
using System.Text;

namespace Totten.Solutions.WolfMonitor.ServiceAgent.Infra.Features.Monitorings.VOs
{
    public class ChangeStatusService
    {
        public Guid AgentID { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string NewStatus { get; set; }
    }
}
