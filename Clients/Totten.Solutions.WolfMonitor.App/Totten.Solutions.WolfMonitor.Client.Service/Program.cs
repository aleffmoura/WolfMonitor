using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.IO;
using Topshelf;
using Totten.Solutions.WolfMonitor.Client.Appl.Base;
using Totten.Solutions.WolfMonitor.Client.Appl.Features.Agents;
using Totten.Solutions.WolfMonitor.Client.Appl.Features.Monitorings;
using Totten.Solutions.WolfMonitor.Client.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Client.Domain.Features.Monitorings;
using Totten.Solutions.WolfMonitor.Client.Domain.Interfaces;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Base;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Authentication;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Monitorings;
using Totten.Solutions.WolfMonitor.Client.Service.Base;
using Totten.Solutions.WolfMonitor.Client.Service.Dtos;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Helpers;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Interfaces;

namespace Totten.Solutions.WolfMonitor.Client.Service
{
    internal class Program
    {

        private static void Main(string[] args)
        {
            var json = File.ReadAllText(".\\AgentConfiguration.json");
            AgentConfiguration agentConfiguration = JsonConvert.DeserializeObject<AgentConfiguration>(json);

            ServiceProvider serviceProvider = new ServiceCollection()
                                    .AddSingleton<IHelper, Helper>()
                                    .AddSingleton(typeof(AgentConfiguration), agentConfiguration)
                                    .AddSingleton(typeof(CustomHttpCliente), new CustomHttpCliente(agentConfiguration.UrlApi, agentConfiguration.User))
                                    .AddSingleton<IAgentEndPoint, AgentEndPoint>()
                                    .AddSingleton<IAgentService, AgentService>()
                                    .AddSingleton<IMonitoring, MonitoringService>()
                                    .AddSingleton<ISystemServiceEndPoint, SystemServiceEndPoint>()
                                    .AddSingleton<ISystemServicesService, SystemServicesService>()
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
