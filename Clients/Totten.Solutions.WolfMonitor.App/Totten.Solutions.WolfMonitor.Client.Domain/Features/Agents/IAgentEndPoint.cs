using System;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Client.Domain.Features.Agents
{
    public interface IAgentEndPoint
    {
        bool Update(Agent agent);
        Result<Exception, Agent> GetInfo();
    }
}
