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
            var json = File.ReadAllText(".\\AgentSettings.json");
            var agent = JsonConvert.DeserializeObject<Agent>(json);
            var login = new UserLogin { Login = agent.Login, Password = agent.Password };

            ServiceProvider serviceProvider = new ServiceCollection()
                                    .AddSingleton<IHelper, Helper>()
                                    .AddSingleton(typeof(CustomHttpCliente), new CustomHttpCliente(agent.urlApi, login))
                                    .AddSingleton<Agent>(agent)
                                    .AddSingleton<AgentInformation>()
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
