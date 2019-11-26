using System;
using System.Collections.Generic;
using System.Text;

namespace Totten.Solutions.WolfMonitor.Client.Domain.Features.Monitorings
{
    public interface ISystemServiceEndPoint
    {
        void Patch(SystemService systemService);
    }
}
