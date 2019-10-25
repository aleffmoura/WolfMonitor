using Newtonsoft.Json;
using System;
using System.Configuration;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Infra.Helpers;

namespace Totten.Solutions.WolfMonitor.Application.Features.Agents
{
    public class AgentService
    {
        private IAgentRepository _agentRepository;
        public AgentConfig AgentConfig { get; private set; }
        public AgentService(IAgentRepository agentRepository)
        {
            _agentRepository = agentRepository;
        }
        public Guid Add(Agent agent)
        {
            var id = _agentRepository.AddAsync(agent).ConfigureAwait(false).GetAwaiter().GetResult();

            return id;
        }

        public Agent MakeAgent()
        {
            AgentConfig = JsonConvert.DeserializeObject<AgentConfig>(ConfigurationManager.AppSettings["agentConfig"])
                          ?? throw new Exception("O Agente não foi configurado, por favor baixe a configuração no site.");

            return new Agent
            {
                CompanyId = Guid.Parse(AgentConfig.CompanyId),
                HostName = NetworkHelper.HostName,
                HostAddress = NetworkHelper.HostAddress,
                Name = Environment.MachineName,
                LocalIp = NetworkHelper.LocalIpAddress,
                Password = AgentConfig.Password,
                UserName = AgentConfig.UserName
            };
        }
    }
}
