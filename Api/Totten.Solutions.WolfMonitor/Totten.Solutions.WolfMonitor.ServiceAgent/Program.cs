using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.IO;
using Topshelf;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Helpers;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Interfaces;
using Totten.Solutions.WolfMonitor.ServiceAgent.Base;
using Totten.Solutions.WolfMonitor.ServiceAgent.Infra.Base;
using Totten.Solutions.WolfMonitor.ServiceAgent.Infra.Features.Agents;
using Totten.Solutions.WolfMonitor.ServiceAgent.Services;

namespace Totten.Solutions.WolfMonitor.ServiceAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            var agentSettings = JsonConvert.DeserializeObject<AgentSettings>(File.ReadAllText(".\\AgentSettings.json")) ?? new AgentSettings
            {
                User = new Infra.Base.UserLogin(),
                Company = "Error",
                urlApi = "error",
                RetrySendIfFailInHours = 1
            };

            ServiceProvider serviceProvider = new ServiceCollection()
                                    .AddSingleton<IHelper>(x => new Helper((IConfigurationRoot)null))
                                    .AddSingleton<AgentSettings>(agentSettings)
                                    .AddSingleton(typeof(CustomHttpCliente), new CustomHttpCliente(agentSettings.urlApi))
                                    .AddSingleton<AgentInformationEndPoint>()
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
