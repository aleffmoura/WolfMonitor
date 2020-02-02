using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.Consul;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.Filters;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.Logging;
using Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.Metrics;

namespace Totten.Solutions.WolfMonitor.Cfg.Startup
{
    public static class AppBuilder
    {

        public static void DefaultApplicationSetup(this IApplicationBuilder app,
                                                    IApplicationLifetime lifetime,
                                                    ILoggerFactory loggerFactory,
                                                    IHostingEnvironment env,
                                                    Container container)
        {
            app.UseAuthentication();

            //app.UseSwaggerCustom();
            app.UseCorsCustom();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseConsul(lifetime);
            app.UseLogging(loggerFactory);
            app.UseMetrics();
            app.UseMvc();
            app.UseOData();
            container.RegisterMvcControllers(app);
            app.LogErrorResponses();
        }

        private static void UseCorsCustom(this IApplicationBuilder app)
        {
            IConfigurationRoot configuration = (IConfigurationRoot)app.ApplicationServices.GetService(typeof(IConfigurationRoot));
            app.UseCors(builder =>
                builder.WithOrigins(configuration["CORS"].Split(","))
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
        }
    }
}
