using Microsoft.AspNetCore.Builder;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Interfaces;

namespace Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.Swagger
{
    public static class AppBuilder
    {
        public static void UseSwaggerCustom(this IApplicationBuilder app)
        {
            IHelper helper = app.ApplicationServices.GetService(typeof(IHelper)) as IHelper;

            string version = helper.GetAssemblyVersion();
            string name = helper.GetServiceName();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(string.Format($"/swagger/{version}/swagger.json"), string.Format($"{name} {version}"));
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
