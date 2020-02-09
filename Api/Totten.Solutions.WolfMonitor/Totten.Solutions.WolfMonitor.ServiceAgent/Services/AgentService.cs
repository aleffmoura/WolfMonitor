using System;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;
using Totten.Solutions.WolfMonitor.ServiceAgent.Base;
using Totten.Solutions.WolfMonitor.ServiceAgent.Features.ItemAggregation;
using Totten.Solutions.WolfMonitor.ServiceAgent.Infra.Base;
using Totten.Solutions.WolfMonitor.ServiceAgent.Infra.Features.Agents;

namespace Totten.Solutions.WolfMonitor.ServiceAgent.Services
{
    public class AgentService
    {
        private AgentInformationEndPoint _agentInformations;
        public AgentService(AgentInformationEndPoint agentEndPoint)
        {
            _agentInformations = agentEndPoint;
        }
        public Result<Exception, Agent> Login()
        {
            var token = _agentInformations.Login();

            if (!string.IsNullOrEmpty(token))
                UserLogin.Token = token;

            return _agentInformations.GetInfo();
        }

        public Result<Exception, PageResult<Item>> GetItems()
                => _agentInformations.GetItems();

        public Result<Exception, Unit> Send(Item item)
                => _agentInformations.Send(item);

        public Result<Exception, Unit> Update(AgentUpdateVO agent)
                => _agentInformations.Update(agent);
        

        public Result<Exception, Agent> GetInfo()
                => _agentInformations.GetInfo();
        
    }
}
