using Hangfire;
using Hangfire.Annotations;
using Hangfire.Dashboard;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using System.Collections.Generic;
using System.Linq;
using Totten.Solutions.WolfMonitor.Cfg.Startup;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.RabbitMQ;
using Totten.Solutions.WolfMonitor.Domain.Features.Jobs;

namespace Totten.Solutions.WolfMonitor.Jobs
{
    public class Startup
    {
        private readonly Container container = new Container();

        public void ConfigureServices(IServiceCollection services)
        {
            services.DefaultServiceSetup(container);
            var configuration = (IConfigurationRoot)services.First(f => f.ServiceType == typeof(IConfigurationRoot)).ImplementationInstance;
            services.AddHangfire(_ => _.UsePostgreSqlStorage(configuration["connectionString"]));
        }


        public void Configure(IApplicationBuilder app,
                              IHostApplicationLifetime lifetime,
                              ILoggerFactory loggerFactory,
                              IWebHostEnvironment env,
                              ILoggerFactory logger, IRabbitMQ rabbitMQ, IConfigurationRoot configuration)
        {
            app.DefaultApplicationSetup(lifetime, logger, env, container);
            app.UseHangfireDashboard("/hangfire", new DashboardOptions()
            {
                Authorization = new List<IDashboardAuthorizationFilter>() { new NoAuthFilter() },
                StatsPollingInterval = 30000 //Tempo atualização do dashboard
            });
            app.UseHangfireServer();

            StartupJobs(rabbitMQ, configuration);

        }

        private void StartupJobs(IRabbitMQ broker, IConfigurationRoot configuration)
        {
            var jobs = configuration.GetSection("jobs").Get<Job[]>();
            foreach (var job in jobs)
            {
                RecurringJob.AddOrUpdate(
                    job.TargetServiceName + "/" + job.MessageType, () => broker.RouteMessage(job.TargetServiceName, job), Cron.MinuteInterval(job.IntervalMinutes));
            }
        }

        class NoAuthFilter : IDashboardAuthorizationFilter
        {
            public bool Authorize([NotNull] DashboardContext context)
            {
                //Code auth logic if this service will have external ip address and port
                return true;
            }
        }
    }
}
