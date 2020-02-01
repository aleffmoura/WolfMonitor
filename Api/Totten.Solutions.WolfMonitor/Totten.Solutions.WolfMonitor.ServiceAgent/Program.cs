using Microsoft.Extensions.DependencyInjection;
using System;
using Topshelf;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Helpers;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Interfaces;
using Totten.Solutions.WolfMonitor.ServiceAgent.Base;
using Totten.Solutions.WolfMonitor.ServiceAgent.Services;

namespace Totten.Solutions.WolfMonitor.ServiceAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                                    .AddSingleton<IHelper, Helper>()
                                    .AddSingleton<AgentService>()
                                    .AddSingleton<WolfService>()
                                    .BuildServiceProvider();

            HostFactory.Run(service =>
            {
                service.Service(() => new TopShelfService(serviceProvider.GetService<WolfService>()));
                service.EnableServiceRecovery(conf => conf.RestartService(TimeSpan.FromSeconds(10)));
                service.SetServiceName("Totem.Solutions.WolfMonitor.Agent");
                service.SetDisplayName("Totem Solutions - Wolf Monitor");
                service.RunAsLocalService();
                service.StartAutomatically();
            });
        }
    }
}
