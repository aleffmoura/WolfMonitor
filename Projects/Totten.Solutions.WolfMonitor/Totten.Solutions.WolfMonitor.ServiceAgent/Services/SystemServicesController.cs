using System;

namespace Totten.Solutions.WolfMonitor.ServiceAgent.Services
{
    public class SystemServicesController
    {
        static bool _isLinux;

        static SystemServicesController()
        {
            _isLinux = Environment.OSVersion.Platform == PlatformID.Unix;
        }

        public static string GetStatus(string name, string displayName)
        {
            if (_isLinux)
                return LinuxService.GetStatus(name, displayName);

            return SystemServicesService.GetStatus(name, displayName);
        }
        public static bool Start(string name, string displayName)
        {
            if (_isLinux)
                return LinuxService.Start(name, displayName);

            return SystemServicesService.Start(name, displayName);
        }
        public static bool Stop(string name, string displayName)
        {
            if (_isLinux)
                return LinuxService.Stop(name, displayName);

            return SystemServicesService.Stop(name, displayName);
        }
    }
}
