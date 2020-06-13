using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.InteropServices;
using Topshelf;
using Topshelf.Runtime.DotNetCore;
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
            var agentSettings = JsonConvert.DeserializeObject<AgentSettings>(File.ReadAllText("./AgentSettings.json")) ?? new AgentSettings
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
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    service.UseEnvironmentBuilder(new Topshelf.HostConfigurators.EnvironmentBuilderFactory(c => {
                        return new DotNetCoreEnvironmentBuilder(c);
                    }));
                }
                service.Service(() => new TopShelfService(serviceProvider.GetService<WolfService>()));
                service.EnableServiceRecovery(conf => conf.RestartService(TimeSpan.FromSeconds(10)));
                service.SetServiceName("WolfMonitor.Agent.Service");
                service.SetDisplayName("Totem Solutions - Wolf Monitor");
                service.RunAsLocalSystem();
                service.StartAutomatically();
            });
        }
    }
}
