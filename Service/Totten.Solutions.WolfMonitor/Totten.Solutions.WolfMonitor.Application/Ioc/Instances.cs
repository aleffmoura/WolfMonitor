using System;
using System.Collections.Generic;
using System.Text;
using Totten.Solutions.WolfMonitor.Application.Features.Agents;
using Totten.Solutions.WolfMonitor.Infra.Base;
using Totten.Solutions.WolfMonitor.Infra.Configurations;
using Totten.Solutions.WolfMonitor.Infra.Features.Agents;

namespace Totten.Solutions.WolfMonitor.Application.Ioc
{
    public static class Instances
    {
        public static AgentService AgentService { get; private set; }
    }
}
