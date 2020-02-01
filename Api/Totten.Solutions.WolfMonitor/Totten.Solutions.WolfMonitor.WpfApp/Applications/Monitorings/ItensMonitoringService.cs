using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Base;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Monitorings;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Monitorings.NovaPasta;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.WpfApp.Applications.Monitorings
{
    public class ItensMonitoringService
    {
        private SystemServiceEndPoint _endPoint;
        public ItensMonitoringService(SystemServiceEndPoint endPoint)
        {
            _endPoint = endPoint;
        }

        public async Task<Result<Exception, PageResult<SystemServiceViewModel>>> GetSystemServices()
        {
            return await _endPoint.GetServicesByAgentId(Guid.Parse("7819adf4-0a58-4fed-e856-08d79f81f999"));
        }
    }
}
