using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using Winton.Extensions.Configuration.Consul;

namespace Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.Consul
{
    public static class ServiceExtension
    {
        public static void AddConsulServiceConfigurations(this IServiceCollection services, string[] configurations)
        {
            CancellationTokenSource consulCancellationSource = new CancellationTokenSource();

            ConfigurationBuilder builder = new ConfigurationBuilder();
            foreach (string confName in configurations)
            {
                builder.AddConsul(
                    confName,
                    consulCancellationSource.Token,
                    options =>
                    {
                        options.ConsulConfigurationOptions = config =>
                        {
                            config.Address = new Uri(Infra.CrossCutting.Configurations.Cfg.CONSUL_URL);
                        };
                        options.Optional = false;
                        options.ReloadOnChange = true;
                    }
                );
            }
            IConfigurationRoot configuration = builder.Build();
            services.AddSingleton(configuration);
        }

    }
}
