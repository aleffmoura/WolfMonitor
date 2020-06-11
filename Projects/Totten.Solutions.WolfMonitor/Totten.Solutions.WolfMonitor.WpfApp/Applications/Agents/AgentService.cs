using System;
using System.Net.Http;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Base;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Authentication;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Agents;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Agents.Profiles;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Items;

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
            => await _endPoint.Post(agent);

        public async Task<Result<Exception, Guid>> PostProfile(ProfileCreateVO profile)
            => await _endPoint.Send<Guid, ProfileCreateVO>("profiles", profile, HttpMethod.Post);
        public async Task<Result<Exception, Guid>> ApplyProfile(ProfileApplyVO profile)
            => await _endPoint.Send<Guid, ProfileApplyVO>("profiles", profile, HttpMethod.Patch);
        public async Task<Result<Exception, Unit>> DeleteProfile(Guid agentId, Guid profileIdentifier)
            => await _endPoint.Send<Unit, object>($"profiles/{agentId}/{profileIdentifier}", null, HttpMethod.Delete);

        public async Task<Result<Exception, Unit>> PostSolicitation(ItemSolicitationVO solicitation)
            => await _endPoint.Send<Unit, ItemSolicitationVO>("item-solicitation", solicitation, HttpMethod.Patch);

        public async Task<Result<Exception, PageResult<ProfileViewItem>>> GetAllProfiles(Guid agentId)
            => await _endPoint.GetAllProfiles<ProfileViewItem>(agentId);

        public async Task<Result<Exception, AgentDetailViewModel>> GetDetail(Guid id)
            => await _endPoint.GetDetail<AgentDetailViewModel>(id);

        public async Task<Result<Exception, Unit>> Delete(Guid agentId)
            => await _endPoint.Delete(agentId);
    }
}
