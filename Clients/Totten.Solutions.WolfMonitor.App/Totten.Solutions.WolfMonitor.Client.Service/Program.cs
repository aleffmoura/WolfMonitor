using System;
using Topshelf;

namespace Totten.Solutions.WolfMonitor.Client.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(service =>
            {
                service.Service<TopShelfService>();
                service.EnableServiceRecovery(conf => conf.RestartService(TimeSpan.FromSeconds(10)));
                service.SetServiceName("Totem.Solutions.WolfMonitor.Agent");
                service.SetDisplayName("Totem Solutions - Wolf Monitor");
                service.RunAsLocalService();
                service.StartAutomatically();
            });
        }
    }
}
