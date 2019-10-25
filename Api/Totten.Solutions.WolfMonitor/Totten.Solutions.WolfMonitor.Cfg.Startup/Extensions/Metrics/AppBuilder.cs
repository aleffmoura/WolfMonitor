using Microsoft.AspNetCore.Builder;

namespace Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.Metrics
{
    public static class AppBuilder
    {
        public static void UseMetrics(this IApplicationBuilder app)
        {
            app.UseMetricsAllMiddleware();
            app.UseMetricsAllEndpoints();
            app.UseHealthAllEndpoints();
        }
    }
}
