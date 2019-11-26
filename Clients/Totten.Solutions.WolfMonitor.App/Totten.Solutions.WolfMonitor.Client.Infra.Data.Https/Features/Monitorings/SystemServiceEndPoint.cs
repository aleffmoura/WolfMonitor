using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Client.Domain.Features.Monitorings;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Base;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Monitorings
{
    public class SystemServiceEndPoint : BaseEndPoint, ISystemServiceEndPoint
    {
        public SystemServiceEndPoint(CustomHttpCliente customHttpCliente) : base(customHttpCliente)
        {

        }
        public void Patch(SystemService systemService)
        {
            InnerAsync(systemService, HttpMethod.Patch).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private async Task<Result<Exception, Unit>> InnerAsync(SystemService agent, HttpMethod httpMethod)
        {
            return await InnerAsync<Result<Exception, Unit>, SystemService>("monitoring/systemservices", agent, httpMethod);
        }
    }
}
