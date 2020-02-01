using System;
using System.Net.Http;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Requests;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Authentication
{
    public class AgentEndPoint : BaseEndPoint
    {
        public AgentEndPoint(CustomHttpCliente customHttpCliente) : base(customHttpCliente)
        {

        }
        public bool Update(Agent agent)
        {
            return InnerAsync(agent, HttpMethod.Patch).ConfigureAwait(false).GetAwaiter().GetResult().IsSuccess;
        }

        public Result<Exception, Agent> GetInfo()
        {
            return InnerGetAsync<Agent>("agents/info").ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private async Task<Result<Exception, Unit>> InnerAsync(Agent agent, HttpMethod httpMethod)
        {
            return await InnerAsync<Result<Exception, Unit>, Agent>("agents", agent, httpMethod);
        }

        //public Result<Exception, ApiResult<SystemService>> GetServices()
        //{
        //    return InnerGetListAsync<SystemService>("monitoring/systemservices").ConfigureAwait(false).GetAwaiter().GetResult();
        //}

    }
}
