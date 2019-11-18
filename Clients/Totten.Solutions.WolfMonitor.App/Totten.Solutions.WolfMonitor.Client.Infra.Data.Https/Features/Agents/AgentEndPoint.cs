using System;
using System.Net.Http;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Client.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Base;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Authentication
{
    public class AgentEndPoint : BaseEndPoint, IAgentEndPoint
    {
        public AgentEndPoint(CustomHttpCliente customHttpCliente) : base(customHttpCliente)
        {

        }
        public bool Update(Agent agent)
        {
            return PostAsync(agent).ConfigureAwait(false).GetAwaiter().GetResult().IsSuccess;
        }

        public Result<Exception, Agent> GetInfo()
        {
            return InnerGetAsync<Result<Exception, Agent>>("agents/info").ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private async Task<Result<Exception, Unit>> PostAsync(Agent agent)
        {
            return await InnerAsync<Result<Exception, Unit>, Agent>("agents", agent, HttpMethod.Patch);
        }

    }
}
