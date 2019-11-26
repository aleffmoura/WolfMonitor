using System;
using System.Collections.Generic;
using System.Text;

namespace Totten.Solutions.WolfMonitor.Client.Domain.Interfaces
{
    public interface IMonitoring
    {
        bool Exists(string serviceName, string serviceDisplayName);
        string GetStatus(string serviceName, string serviceDisplayName);
        bool Start(string serviceName, string serviceDisplayName);
        bool Stop(string serviceName, string serviceDisplayName);
    }
}
