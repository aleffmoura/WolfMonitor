using App.Metrics;
using Microsoft.Extensions.DependencyInjection;

namespace Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.Metrics
{
    public static class ServiceExtension
    {
        public static void AddMetric(this IServiceCollection services)
        {
            IMetricsRoot metrics = new MetricsBuilder()
                .OutputMetrics.AsPrometheusPlainText()
                .Build();

            services.AddMetrics(metrics);
            services.AddMetricsTrackingMiddleware();
            services.AddMetricsEndpoints();
            services.AddHealth();
            services.AddHealthEndpoints();
        }
    }
}
