using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Interfaces;

namespace Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.Consul
{
    public static class AppBuilder
    {
        public async static void UseConsul(this IApplicationBuilder app, IApplicationLifetime lifetime)
        {
            var configuration = (IConfigurationRoot)app.ApplicationServices.GetService(typeof(IConfigurationRoot));
            var helpers = (IHelper)app.ApplicationServices.GetService(typeof(IHelper));
            string serviceName = helpers.GetServiceName();

            if (configuration == null || string.IsNullOrEmpty(configuration["Tags"]))
            {
                Console.Error.WriteLine($"No 'tags' configuration found for service {serviceName}!");
            }

            string[] tags = string.IsNullOrEmpty(configuration["Tags"]) ? null : configuration["Tags"].Split(",");

            string id = Guid.NewGuid().ToString();
            var serverAddressesFeature = app.ServerFeatures.Get<IServerAddressesFeature>();
            var endereco = new Uri(serverAddressesFeature.Addresses.First());


            var registration = new AgentServiceRegistration
            {
                Name = serviceName,
                Address = endereco.Host,
                Port = endereco.Port,
                ID = id,
                Tags = tags,

                Check = new AgentServiceCheck
                {
                    HTTP = endereco.AbsoluteUri + "health",
                    Interval = new TimeSpan(0, 0, 0, int.Parse(configuration["healthCheckIntervalSegs"]), 0),
                    Timeout = new TimeSpan(0, 0, 0, 0, 10000),
                    DeregisterCriticalServiceAfter = new TimeSpan(0, 0, 0, int.Parse(configuration["deregisterCriticalServiceAfterSegs"]), 0),
                }
            };

            using (var client = new ConsulClient(c => c.Address = new Uri(Infra.CrossCutting.Configurations.Cfg.CONSUL_URL)))
            {
                Console.WriteLine($"Registering service {serviceName} on consul at {client.Config.Address.AbsoluteUri}...");
                try
                {
                    await client.Agent.ServiceRegister(registration);
                    Console.WriteLine($"Service {serviceName} registered with address {endereco.AbsoluteUri}.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to register service: {ex.Message} \n {ex.InnerException}");
                }

            }

            lifetime.ApplicationStopping.Register(() =>
            {
                using (var client = new ConsulClient(c => c.Address = new Uri(Infra.CrossCutting.Configurations.Cfg.CONSUL_URL)))
                {
                    client.Agent.ServiceDeregister(registration.ID).Wait();
                }
            });

        }
    }
}
