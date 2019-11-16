using System;
using System.Collections.Generic;
using System.Text;
using Totten.Solutions.WolfMonitor.Client.Domain.Features.Agents;

namespace Totten.Solutions.WolfMonitor.Client.Appl.Features.Agents
{
    public interface IAgentService
    {
        bool Update(Agent agent);
    }
}
