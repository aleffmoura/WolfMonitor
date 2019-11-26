using System;
using System.Collections.Generic;
using System.Text;
using Totten.Solutions.WolfMonitor.Client.Domain.Features.Monitorings;

namespace Totten.Solutions.WolfMonitor.Client.Appl.Features.Monitorings
{
    public class SystemServicesService : ISystemServicesService
    {
        private ISystemServiceEndPoint _agentEndPoint;
        public SystemServicesService(ISystemServiceEndPoint agentEndPoint)
        {
            _agentEndPoint = agentEndPoint;
        }

        public void Post(List<SystemService> systemServices)
        {
            foreach (var service in systemServices)
            {
                _agentEndPoint.Patch(service);
            }
        }
    }
}
