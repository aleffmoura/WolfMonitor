using Microsoft.Extensions.DependencyInjection;

namespace Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.RabbitMQ
{
    public static class ServicesExtensions
    {
        public static void AddRabbitMQ(this IServiceCollection services)
        {
            services.AddSingleton<IRabbitMQ, RabbitMQ>();
            services.AddHostedService<BrokerReceiver>();
        }
    }
}
