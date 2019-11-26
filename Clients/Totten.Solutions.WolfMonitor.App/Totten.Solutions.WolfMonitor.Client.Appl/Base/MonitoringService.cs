using System;
using System.Linq;
using System.ServiceProcess;
using Totten.Solutions.WolfMonitor.Client.Domain.Interfaces;

namespace Totten.Solutions.WolfMonitor.Client.Appl.Base
{
    public class MonitoringService : IMonitoring
    {
        private ServiceController GetService(string serviceName, string serviceDisplayName)
        {
            ServiceController serviceController = null;
            if (!string.IsNullOrWhiteSpace(serviceName) || !string.IsNullOrWhiteSpace(serviceDisplayName))
            {
                serviceController = ServiceController.GetServices().FirstOrDefault(s => s.DisplayName.Trim().Equals(serviceDisplayName, StringComparison.InvariantCultureIgnoreCase) ||
                                                                                        s.ServiceName.Trim().Equals(serviceName, StringComparison.InvariantCultureIgnoreCase));
            }

            return serviceController;
        }
        public bool Exists(string serviceName, string serviceDisplayName)
        {
            using (ServiceController service = GetService(serviceName, serviceDisplayName))
            {
                return service != null;
            }
        }
        public string GetStatus(string serviceName, string serviceDisplayName)
        {
            using (ServiceController serviceControler = GetService(serviceName, serviceDisplayName))
            {
                return Exists(serviceName, serviceDisplayName) ? serviceControler.Status.ToString() : "Service Not Found";
            }
        }
        public bool Start(string serviceName, string serviceDisplayName)
        {
            try
            {
                using (ServiceController serviceController = GetService(serviceName, serviceDisplayName))
                {
                    if (serviceController == null)
                    {
                        return false;
                    }
                    serviceController.Refresh();
                    if (serviceController.Status != ServiceControllerStatus.Running)
                    {
                        serviceController.Start();
                        serviceController.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(15));
                    }
                }
            }
            catch (System.ServiceProcess.TimeoutException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public bool Stop(string serviceName, string serviceDisplayName)
        {
            using (ServiceController serviceController = GetService(serviceName, serviceDisplayName))
            {
                if (serviceController == null || !serviceController.CanStop)
                {
                    return false;
                }
                try
                {
                    serviceController.Refresh();
                    if (serviceController.Status != ServiceControllerStatus.Stopped)
                    {
                        serviceController.Stop();
                        serviceController.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(120));
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }
}
