using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.Logging
{
    public static class ServiceExtension
    {
        public static void AddLog(this IServiceCollection services)
        {
            services.AddLogging(config =>
            {
                config.ClearProviders();
            });
        }
    }
}
