
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using Totten.Solutions.WolfMonitor.Cfg.Startup;

namespace Totten.Solutions.WolfMonitor.Agents
{
    public class Startup
    {
        private readonly Container container = new Container();
        public void ConfigureServices(IServiceCollection services)
        {
            services.DefaultServiceSetup(container);
        }

        public void Configure(IApplicationBuilder app,
                              IApplicationLifetime lifetime,
                              ILoggerFactory loggerFactory,
                              IHostingEnvironment env)
        {
            app.DefaultApplicationSetup(lifetime, loggerFactory, env, container);
        }
    }
}
