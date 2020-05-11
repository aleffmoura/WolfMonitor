using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Interfaces;

namespace Totten.Solutions.WolfMonitor.Cfg.Startup.Extensions.Swagger
{
    public static class ServiceExtension
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            IHelper helper = services.BuildServiceProvider().GetService<IHelper>();
            string version = helper.GetAssemblyVersion();
            string serviceName = helper.GetServiceName();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(version, new Info { Title = serviceName, Version = version });

                string xmlFile = $"{helper.GetAssemblyName()}.xml";
                string xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

            });
        }
    }
}
