using System;
using System.Collections.Generic;
using Totten.Solutions.WolfMonitor.Client.Domain.Base;
using Totten.Solutions.WolfMonitor.Client.Domain.Features.Monitorings;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Client.Domain.Features.Agents
{
    public interface IAgentEndPoint
    {
        bool Update(Agent agent);
        Result<Exception, Agent> GetInfo();
        Result<Exception, ApiResult<SystemService>> GetServices();
    }
}
