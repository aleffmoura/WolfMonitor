using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Totten.Solutions.WolfMonitor.Authorize.Extensions;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.Consul;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.Logging;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.Metrics;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Helpers;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Interfaces;

namespace Totten.Solutions.WolfMonitor.Authorize
{
    public class Startup
    {
        private static readonly Helper _helper = new Helper(null);

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddConsulServiceConfigurations(new string[] { "Global", _helper.GetServiceName() });
            services.AddMetric();
            services.AddSingleton<IHelper, Helper>();
            services.AddMvcCore().AddJsonFormatters();

            var configOnConsul = services.BuildServiceProvider().GetService<IConfigurationRoot>();
            services.AddDependencies(configOnConsul);
            services.AddMediator();
            services.AddJwtServer().AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

        }

        public void Configure(IApplicationBuilder app,
                             IHostingEnvironment env,
                             ILoggerFactory loggerFactory,
                             IApplicationLifetime lifetime,
                             IConfigurationRoot configurationRoot)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            IdentityModelEventSource.ShowPII = true;
            app.UseConsul(lifetime);
            app.UseLogging(loggerFactory);
            app.UseMetrics();
            app.UseMvc();
            app.UseJWTServer(configurationRoot);
        }
    }
}
