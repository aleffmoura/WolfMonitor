using System;
using System.Net.Http;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Infra.Base;
using Totten.Solutions.WolfMonitor.Infra.Configurations;

namespace Totten.Solutions.WolfMonitor.Infra.Features.Agents
{
    public class AgentApiRepository : BaseEndPoint, IAgentRepository
    {
        public AgentApiRepository(CustomHttpCliente customHttpCliente) : base(customHttpCliente)
        {
            base._endPoint = $"{Cfg.ApiUrl}{(Cfg.ApiUrl[Cfg.ApiUrl.Length - 1].Equals("/") ? "agents" : " /agents" )}";
        }
        public async Task<Guid> AddAsync(Agent agent)
        {
            return await SendAsync<Guid, Agent>(_endPoint, HttpMethod.Post, agent);
        }
        public async Task<Agent> GetById(Guid id)
        {
            return await SendAsync<Agent, Agent>($"{_endPoint}/{id}", HttpMethod.Get, null);
        }
    }
}
