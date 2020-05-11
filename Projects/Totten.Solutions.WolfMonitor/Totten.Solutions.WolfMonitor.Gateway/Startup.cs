using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using SimpleInjector;
using System.Linq;
using Totten.Solutions.WolfMonitor.Cfg.Startup;

namespace Totten.Solutions.WolfMonitor.Gateway
{
    public class Startup
    {
        private readonly Container container = new Container();
        public void ConfigureServices(IServiceCollection services)
        {
            services.DefaultServiceSetup(container);
            var configuration = (IConfigurationRoot)services.First(f => f.ServiceType == typeof(IConfigurationRoot)).ImplementationInstance;

            services.AddOcelot(configuration).AddConsul();
        }

        public void Configure(IApplicationBuilder app,
                              IApplicationLifetime lifetime,
                              ILoggerFactory loggerFactory,
                              IHostingEnvironment env)
        { 
            app.DefaultApplicationSetup(lifetime,
                                        loggerFactory,
                                        env,
                                        container);
            app.UseWebSockets();
            app.UseOcelot().Wait();
        }
    }
}
