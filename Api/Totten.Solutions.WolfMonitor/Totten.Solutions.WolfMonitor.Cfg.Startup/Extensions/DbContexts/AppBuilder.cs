using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.DbContexts
{
    public static class AppBuilder
    {
        public static void ApplyMigrations(this IApplicationBuilder app, Container container, IHostingEnvironment environment)
        {
            using (AsyncScopedLifestyle.BeginScope(container))
            {
                //var database = container.GetInstance<PCDoctorContext>().Database;
                //if (!database.IsInMemory())
                //{
                //    database.Migrate();
                //}
            }
        }
    }
}
