using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.RabbitMQService;

namespace Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.RabbitMQ
{
    public static class ServiceExtension
    {
        public static void AddRabbitMQ(this IServiceCollection services)
        {
            services.AddSingleton<IRabbitMQ, Rabbit>();
            //services.AddHostedService<BrokerReceiver>();
        }
    }
}
