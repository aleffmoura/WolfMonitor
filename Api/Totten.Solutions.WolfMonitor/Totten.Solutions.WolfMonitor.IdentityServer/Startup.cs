using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.Consul;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.Logging;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.Metrics;
using Totten.Solutions.WolfMonitor.IdentityServer.Configs;
using Totten.Solutions.WolfMonitor.IdentityServer.Extensions;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Helpers;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Interfaces;

namespace Totten.Solutions.WolfMonitor.IdentityServer
{
    public class Startup
    {
        private static readonly Helper _helper = new Helper(null);
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddConsulServiceConfigurations(new string[] { "Global", _helper.GetServiceName() });
            services.AddMetric();
            services.AddSingleton<IHelper, Helper>();
            services.AddScoped<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            services.AddMvcCore().AddJsonFormatters();

            IConfigurationRoot configOnConsul = services.BuildServiceProvider().GetService<IConfigurationRoot>();
            Config conf = new Config(configOnConsul);
            services.AddDependencies(configOnConsul);

            services.AddIdentityServer()
                    .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                    .AddDeveloperSigningCredential()
                    .AddInMemoryIdentityResources(conf.GetIdentityResources())
                    .AddInMemoryApiResources(conf.GetApiResources())
                    .AddInMemoryClients(conf.GetClients());
        }

        public void Configure(IApplicationBuilder app,
                             IHostingEnvironment env,
                             ILoggerFactory loggerFactory,
                             IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            IdentityModelEventSource.ShowPII = true;
            app.UseConsul(lifetime);
            app.UseLogging(loggerFactory);
            app.UseMetrics();
            app.UseIdentityServer();
            app.UseMvc();
        }
    }
}
