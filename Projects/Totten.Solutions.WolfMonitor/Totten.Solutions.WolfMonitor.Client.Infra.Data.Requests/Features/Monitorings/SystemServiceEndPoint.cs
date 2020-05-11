using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Requests;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Monitorings.NovaPasta;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Monitorings
{
    public class SystemServiceEndPoint : BaseEndPoint
    {
        public SystemServiceEndPoint(CustomHttpCliente customHttpCliente) : base(customHttpCliente)
        {

        }
        //public void Patch(SystemService systemService)
        //{
        //    InnerAsync(systemService, HttpMethod.Patch).ConfigureAwait(false).GetAwaiter().GetResult();
        //}

        //private async Task<Result<Exception, Unit>> InnerAsync(SystemService services, HttpMethod httpMethod)
        //{
        //    return await InnerAsync<Result<Exception, Unit>, SystemService>("monitoring/systemservices", agent, httpMethod);
        //}

        public async Task<Result<Exception, PageResult<SystemServiceViewModel>>> GetServicesByAgentId(Guid guid)
        {
            return await InnerGetAsync<PageResult<SystemServiceViewModel>>($"monitoring/SystemServices/{guid}");
        }
    }
}
