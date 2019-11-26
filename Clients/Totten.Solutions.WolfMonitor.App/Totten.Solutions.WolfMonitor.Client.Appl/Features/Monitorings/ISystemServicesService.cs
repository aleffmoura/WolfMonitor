using System;
using System.Collections.Generic;
using System.Text;
using Totten.Solutions.WolfMonitor.Client.Domain.Features.Monitorings;

namespace Totten.Solutions.WolfMonitor.Client.Appl.Features.Monitorings
{
    public interface ISystemServicesService
    {
        void Post(List<SystemService> systemServices);
    }
}
