using System;
using System.Collections.Generic;
using System.Text;
using Totten.Solutions.WolfMonitor.Domain.Base;

namespace Totten.Solutions.WolfMonitor.Domain.Features.Logs
{
    public interface ILogMonitoringRepository : IRepository<MonitoringLog>
    {
    }
}
