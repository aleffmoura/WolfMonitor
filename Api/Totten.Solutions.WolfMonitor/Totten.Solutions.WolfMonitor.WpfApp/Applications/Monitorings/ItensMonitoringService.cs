using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Base;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Monitorings;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.SystemServices;

namespace Totten.Solutions.WolfMonitor.WpfApp.Applications.Monitorings
{
    public class ItensMonitoringService
    {
        private SystemServiceEndPoint _endPoint;

        public ItensMonitoringService(SystemServiceEndPoint endPoint)
        {
            _endPoint = endPoint;
        }

        public async Task<Result<Exception, PageResult<SystemServiceViewModel>>> GetSystemServices(Guid id)
        {
            return await _endPoint.GetServicesByAgentId<SystemServiceViewModel>(id);
        }

        public async Task<Result<Exception, Unit>> Delete(Guid agentId,Guid id)
        {
            return await _endPoint.Delete(agentId,id);
        }
    }
}
