using Totten.Solutions.WolfMonitor.Client.Domain.Features.Agents;

namespace Totten.Solutions.WolfMonitor.Client.Appl.Features.Agents
{
    public class AgentService : IAgentService
    {
        private IAgentEndPoint _agentEndPoint;
        public AgentService(IAgentEndPoint agentEndPoint)
        {
            _agentEndPoint = agentEndPoint;
        }

        public bool Update(Agent agent)
        {
            return _agentEndPoint.Update(agent);
        }
    }
}
