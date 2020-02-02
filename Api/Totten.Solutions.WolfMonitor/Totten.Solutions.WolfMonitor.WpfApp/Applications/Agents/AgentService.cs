using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Base;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Authentication;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Agents;

namespace Totten.Solutions.WolfMonitor.WpfApp.Applications.Agents
{
    public class AgentService
    {
        private AgentEndPoint _endPoint;

        public AgentService(AgentEndPoint endPoint)
        {
            _endPoint = endPoint;
        }

        public async Task<Result<Exception, PageResult<AgentResumeViewModel>>> GetAllAgentsByCompany()
        {
            return await _endPoint.GetAllAgents<AgentResumeViewModel>();
        }

        public async Task<Result<Exception, Unit>> Delete(Guid agentId)
        {
            return await _endPoint.Delete(agentId);
        }
    }
}
