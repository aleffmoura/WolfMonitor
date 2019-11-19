using System;
using System.Collections.Generic;
using System.Text;
using Totten.Solutions.WolfMonitor.Client.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Client.Domain.Features.Monitorings;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Client.Appl.Features.Agents
{
    public interface IAgentService
    {
        bool Update(Agent agent);
        Result<Exception, Agent> GetInfo();
        Result<Exception, List<SystemService>> GetServicesMonitoring();
    }
}
