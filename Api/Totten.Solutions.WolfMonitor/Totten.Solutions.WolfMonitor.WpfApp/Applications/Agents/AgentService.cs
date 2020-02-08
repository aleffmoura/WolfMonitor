using System;
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
            => await _endPoint.GetAllAgents<AgentResumeViewModel>();

        public async Task<Result<Exception, Guid>> Post(AgentCreateVO agent)
            => await _endPoint.Post<AgentCreateVO>(agent);

        public async Task<Result<Exception, AgentDetailViewModel>> GetDetail(Guid id)
            => await _endPoint.GetDetail<AgentDetailViewModel>(id);

        public async Task<Result<Exception, Unit>> Delete(Guid agentId)
            => await _endPoint.Delete(agentId);
    }
}
