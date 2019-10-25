using Microsoft.Extensions.DependencyInjection;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Filters;

namespace Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.Filters
{
    public static class ServiceExtension
    {
        public static void AddFilters(this IServiceCollection services)
        {
            services.AddMvc(options => options.Filters.Add(new ExceptionHandlerAttribute()));
        }
    }
}
